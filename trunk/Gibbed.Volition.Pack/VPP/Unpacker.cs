﻿/* Copyright (c) 2011 Rick (rick 'at' gibbed 'dot' us)
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
using Gibbed.IO;
using Gibbed.Volition.FileFormats;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using NDesk.Options;
using Package = Gibbed.Volition.FileFormats.Package;

namespace Gibbed.Volition.Pack.VPP
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
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_vpp [output_dir]", GetExecutableName());
                Console.WriteLine("Unpack specified Volition package file.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return 2;
            }

            string inputPath = extras[0];
            string outputPath = extras.Count > 1 ?
                extras[1] :
                Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileNameWithoutExtension(extras[0]));

            var previousNames = new Dictionary<string, long>();

            using (var input = File.OpenRead(inputPath))
            {
                var package = new TPackage();
                package.Deserialize(input);

                long current = 0;
                long total = package.Directory.Count();

                if (total > 0)
                {
                    Stream data = input;
                    var flags = package.Flags;

                    var dataOffset = package.DataOffset;
                    var isCompressed = (package.Flags & Package.HeaderFlags.Compressed) != 0;
                    var isCompressedInChunks = (package.Flags & Package.HeaderFlags.CompressedInChunks) != 0;
                    var isCondensed = (package.Flags & Package.HeaderFlags.Condensed) != 0;

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
                        current++;

                        string outputName;

                        if (previousNames.ContainsKey(entry.Name) == true)
                        {
                            outputName = string.Format("{0} [DUPLICATE_{1}]{2}",
                                Path.ChangeExtension(entry.Name, null),
                                previousNames[entry.Name],
                                Path.GetExtension(entry.Name) ?? "");
                            previousNames[entry.Name]++;
                        }
                        else
                        {
                            outputName = entry.Name;
                            previousNames.Add(entry.Name, 1);
                        }

                        var entryPath = Path.Combine(outputPath, outputName);

                        if (overwriteFiles == true ||
                            File.Exists(entryPath) == false)
                        {
                            if (verbose == true)
                            {
                                Console.WriteLine("[{0}/{1}] {2}",
                                    current.ToString().PadLeft(padding),
                                    total,
                                    entry.Name);
                            }

                            Directory.CreateDirectory(Path.GetDirectoryName(entryPath));

                            var dataStart = dataOffset;

                            Console.WriteLine("data start = {0:X8}", dataStart);

                            data.Seek(dataOffset, SeekOrigin.Begin);
                            using (var output = File.Create(entryPath))
                            {
                                if (isCompressed == false)
                                {
                                    output.WriteFromStream(data, entry.UncompressedSize);
                                }
                                else if (isCompressedInChunks == true)
                                {
                                    long chunkLeft = entry.UncompressedSize;
                                    var chunkStart = input.Position;
                                    var chunkEnd = input.Position + entry.CompressedSize;

                                    while (chunkLeft > 0)
                                    {
                                        if (input.Position >= chunkEnd)
                                        {
                                            throw new InvalidOperationException();
                                        }

                                        var chunkCompressedSize = input.ReadValueU16(package.Endian);
                                        var chunkUnknown = input.ReadValueU16(package.Endian);
                                        var chunkUncompressedSize = input.ReadValueU32(package.Endian);

                                        Console.WriteLine("  @ {0:X8}", input.Position);
                                        Console.WriteLine("  csize = {0}", chunkCompressedSize);
                                        Console.WriteLine("  usize = {0} ", chunkUncompressedSize);

                                        if (input.Position + chunkCompressedSize > chunkEnd)
                                        {
                                            throw new FormatException();
                                        }

                                        if (chunkUnknown != 0)
                                        {
                                            throw new FormatException();
                                        }

                                        if (chunkUncompressedSize > chunkLeft)
                                        {
                                            throw new FormatException();
                                        }

                                        using (var temp = data.ReadToMemoryStream(chunkCompressedSize))
                                        {
                                            var zlib = new InflaterInputStream(temp, new ICSharpCode.SharpZipLib.Zip.Compression.Inflater(true));
                                            output.WriteFromStream(zlib, chunkUncompressedSize);
                                        }

                                        chunkLeft -= chunkUncompressedSize;
                                    }
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

                return 0;
            }
        }
    }
}
