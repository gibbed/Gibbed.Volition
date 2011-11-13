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
using System.Text;
using System.Xml;
using Gibbed.IO;
using Gibbed.Volition.FileFormats;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using NDesk.Options;
using Package = Gibbed.Volition.FileFormats.Package;

namespace Gibbed.Volition.Pack.STR2
{
    public class Unpacker<TPackage>
        where TPackage : IPackageFile, new()
    {
        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }

        public int Main(string[] args)
        {
            var showHelp = false;
            var overwriteFiles = false;
            var verbose = false;

            var options = new OptionSet()
            {
                {
                    "o|overwrite",
                    "overwrite files if they already exist", 
                    v => overwriteFiles = v != null
                },
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

            if (extras.Count < 1 || extras.Count > 2 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_str2 [output_dir]", GetExecutableName());
                Console.WriteLine("Unpack specified Volition streams package file.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return 2;
            }

            string inputPath = extras[0];
            string outputPath = extras.Count > 1 ?
                extras[1] :
                Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileNameWithoutExtension(extras[0]));

            using (var input = File.OpenRead(inputPath))
            {
                var package = new TPackage();
                package.Deserialize(input);

                long current = 0;
                long total = package.Directory.Count();

                Directory.CreateDirectory(outputPath);

                var settings = new XmlWriterSettings()
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                };

                using (var xml = XmlWriter.Create(Path.Combine(outputPath, "@streams.xml"), settings))
                {
                    var isCompressed = (package.Flags & Package.HeaderFlags.Compressed) != 0;
                    var isCondensed = (package.Flags & Package.HeaderFlags.Condensed) != 0;

                    xml.WriteStartDocument();
                    xml.WriteStartElement("streams");
                    xml.WriteAttributeString("endian", package.Endian.ToString());
                    xml.WriteAttributeString("compressed", isCompressed.ToString());
                    xml.WriteAttributeString("condensed", isCondensed.ToString());

                    if (total > 0)
                    {
                        Stream data = input;

                        var dataOffset = package.DataOffset;

                        if (isCondensed == true && isCompressed == true)
                        {
                            isCompressed = false;

                            data = new MemoryStream();

                            input.Seek(package.DataOffset, SeekOrigin.Begin);
                            var zlib = new InflaterInputStream(input);
                            data.WriteFromStream(zlib, package.UncompressedSize);

                            dataOffset = 0;
                        }

                        var padding = total.ToString().Length;

                        foreach (var entry in package.Directory.OrderBy(e => e.Offset))
                        {
                            xml.WriteStartElement("entry");
                            xml.WriteAttributeString("name", entry.Name);
                            xml.WriteValue(entry.Name);
                            xml.WriteEndElement();

                            current++;

                            var entryPath = Path.Combine(outputPath, entry.Name);

                            if (overwriteFiles == false &&
                                File.Exists(entryPath) == true)
                            {
                                continue;
                            }

                            if (verbose == true)
                            {
                                Console.WriteLine("[{0}/{1}] {2}",
                                    current.ToString().PadLeft(padding),
                                    total,
                                    entry.Name);
                            }

                            Directory.CreateDirectory(Path.GetDirectoryName(entryPath));

                            data.Seek(dataOffset, SeekOrigin.Begin);
                            using (var output = File.Create(entryPath))
                            {
                                if (isCompressed == false)
                                {
                                    output.WriteFromStream(data, entry.UncompressedSize);
                                }
                                else
                                {
                                    using (var temp = data.ReadToMemoryStream(entry.CompressedSize))
                                    {
                                        var zlib = new InflaterInputStream(temp);
                                        output.WriteFromStream(zlib, entry.UncompressedSize);
                                    }
                                }
                            }

                            var dataSize = isCompressed == false ?
                                entry.UncompressedSize : entry.CompressedSize;
                            if (isCondensed == false)
                            {
                                dataSize = dataSize.Align(2048);
                            }

                            dataOffset += dataSize;
                        }

                        if (data != input)
                        {
                            data.Close();
                        }
                    }

                    xml.WriteEndElement();
                    xml.WriteEndDocument();
                }

                return 0;
            }
        }
    }
}
