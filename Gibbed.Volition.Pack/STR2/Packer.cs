/* Copyright (c) 2011 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.XPath;
using Gibbed.IO;
using Gibbed.Volition.FileFormats;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using NDesk.Options;
using Package = Gibbed.Volition.FileFormats.Package;
using ZLIB = ComponentAce.Compression.Zlib;

namespace Gibbed.Volition.Pack.STR2
{
    public class Packer<TPackage, TEntry>
        where TPackage : IPackageFile<TEntry>, new()
        where TEntry : IPackageEntry, new()
    {
        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }

        public int Main(string[] args)
        {
            var showHelp = false;
            var verbose = false;

            var options = new OptionSet()
            {
                {
                    "v|verbose",
                    "enable verbose logging", 
                    v => verbose = v != null
                },
                {
                    "h|help",
                    "show this message and exit", 
                    v => showHelp = v != null
                },
            };

            List<string> extras;

            try
            {
                extras = options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("{0}: ", GetExecutableName());
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `{0} --help' for more information.", GetExecutableName());
                return 1;
            }

            if (extras.Count != 2 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ output_str2 input_directory", GetExecutableName());
                Console.WriteLine("Pack directores into specified Volition streams package file.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return 2;
            }

            string outputPath = extras[0];
            string inputPath = extras[1];

            var package = new TPackage();
            var paths = new Dictionary<string, string>();

            bool isCompressed;
            bool isCondensed;

            using (var input = File.OpenRead(Path.Combine(inputPath, "@streams.xml")))
            {
                var doc = new XPathDocument(input);
                var nav = doc.CreateNavigator();

                var root = nav.SelectSingleNode("/streams");

                var _endian = root.GetAttribute("endian", "");
                package.Endian = string.IsNullOrWhiteSpace(_endian) == true ?
                    Endian.Little : (Endian)Enum.Parse(typeof(Endian), _endian);

                var _compressed = root.GetAttribute("compressed", "");
                isCompressed = string.IsNullOrWhiteSpace(_compressed) == true ?
                    false : (bool)Convert.ChangeType(_compressed, typeof(bool));

                var _condensed = root.GetAttribute("condensed", "");
                isCondensed = string.IsNullOrWhiteSpace(_condensed) == true ?
                    false : (bool)Convert.ChangeType(_condensed, typeof(bool));

                var entries = root.Select("entry");
                while (entries.MoveNext() == true)
                {
                    var name = entries.Current.GetAttribute("name", "");
                    if (string.IsNullOrWhiteSpace(name) == true)
                    {
                        throw new FormatException("name cannot be empty");
                    }

                    var path = entries.Current.Value;
                    if (string.IsNullOrWhiteSpace(path) == true)
                    {
                        throw new FormatException("path cannot be empty");
                    }

                    if (Path.IsPathRooted(path) == false)
                    {
                        path = Path.Combine(inputPath, path);
                    }

                    if (paths.ContainsKey(name) == true)
                    {
                        throw new FormatException("duplicate name");
                    }

                    paths.Add(name, path);
                }

                var flags = Package.HeaderFlags.None;

                if (isCompressed == true)
                {
                    flags |= Package.HeaderFlags.Compressed;
                }

                if (isCondensed == true)
                {
                    flags |= Package.HeaderFlags.Condensed;
                }

                package.Flags = flags;
            }

            package.Entries.Clear();
            foreach (var kv in paths)
            {
                package.Entries.Add(new TEntry()
                {
                    Name = kv.Key,
                });
            }

            var baseOffset = package.EstimateHeaderSize();

            package.Entries.Clear();
            using (var output = File.Create(outputPath))
            {
                if (isCondensed == true &&
                    isCompressed == true)
                {
                    output.Seek(baseOffset, SeekOrigin.Begin);

                    using (var compressed = new MemoryStream())
                    {
                        var z = new ZLIB.ZOutputStream(compressed, ZLIB.zlibConst.Z_BEST_COMPRESSION);
                        z.FlushMode = ZLIB.zlibConst.Z_SYNC_FLUSH;
                        
                        long offset = 0;
                        foreach (var kv in paths)
                        {
                            using (var input = File.OpenRead(kv.Value))
                            {
                                var entry = new TEntry();
                                entry.Name = kv.Key;
                                entry.Offset = (uint)offset;
                                entry.UncompressedSize = (uint)input.Length;

                                long size = z.TotalOut;

                                z.WriteFromStream(input, input.Length);

                                size = z.TotalOut - size;

                                entry.CompressedSize = (uint)size;

                                offset += entry.UncompressedSize;

                                package.Entries.Add(entry);
                            }
                        }

                        package.CompressedSize = (uint)compressed.Length;
                        package.UncompressedSize = (uint)offset;

                        compressed.Position = 0;
                        output.WriteFromStream(compressed, compressed.Length);
                    }

                    output.Seek(0, SeekOrigin.Begin);
                    package.Serialize(output);
                }
                else
                {
                    output.Seek(baseOffset, SeekOrigin.Begin);

                    long offset = 0;
                    foreach (var kv in paths)
                    {
                        using (var input = File.OpenRead(kv.Value))
                        {
                            if (isCondensed == false)
                            {
                                var padding = offset.Align(2048) - offset;
                                if (padding > 0)
                                {
                                    offset += padding;
                                    output.Seek(padding, SeekOrigin.Current);
                                }
                            }

                            var entry = new TEntry();
                            entry.Name = kv.Key;
                            entry.Offset = (uint)offset;
                            entry.UncompressedSize = (uint)input.Length;

                            if (isCompressed == true)
                            {
                                using (var compressed = new MemoryStream())
                                {
                                    var zlib = new DeflaterOutputStream(compressed);
                                    zlib.WriteFromStream(input, input.Length);
                                    zlib.Finish();

                                    entry.CompressedSize = (uint)compressed.Length;

                                    compressed.Position = 0;
                                    output.WriteFromStream(compressed, compressed.Length);
                                }

                                offset += entry.CompressedSize;
                            }
                            else
                            {
                                output.WriteFromStream(input, input.Length);
                                entry.CompressedSize = 0xFFFFFFFF;
                                offset += entry.UncompressedSize;
                            }

                            package.Entries.Add(entry);
                        }

                        package.CompressedSize = isCompressed == false ?
                             0xFFFFFFFF : (uint)package.Entries.Sum(e => e.CompressedSize);
                        package.UncompressedSize = (uint)offset;
                    }
                }

                package.TotalSize = (uint)output.Length;
                output.Seek(0, SeekOrigin.Begin);
                package.Serialize(output);
            }

            return 0;
        }
    }
}
