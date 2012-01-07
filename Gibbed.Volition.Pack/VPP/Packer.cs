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
using Gibbed.IO;
using Gibbed.Volition.FileFormats;
using NDesk.Options;
using Package = Gibbed.Volition.FileFormats.Package;

namespace Gibbed.Volition.Pack.VPP
{
    public class Packer<TPackage, TEntry> : PackerBase<TPackage, TEntry>
        where TPackage : IPackageFile<TEntry>, new()
        where TEntry : IPackageEntry, new()
    {
        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().CodeBase);
        }

        public override int Main(string[] args)
        {
            var showHelp = false;
            var verbose = false;
            var isCompressed = false;
            var isCondensed = false;
            var isCompressedInChunks = false;
            var endian = Endian.Little;

            var package = new TPackage();
            var options = new OptionSet();

            if ((package.SupportedFlags & Package.HeaderFlags.Compressed) != 0)
            {
                options.Add(
                    "c|compress",
                    "compress data",
                    v => isCompressed = v != null
                );
            }

            if ((package.SupportedFlags & Package.HeaderFlags.Condensed) != 0)
            {
                options.Add(
                    "n|condense",
                    "condense data",
                    v => isCondensed = v != null
                );
            }

            if ((package.SupportedFlags & Package.HeaderFlags.CompressedInChunks) != 0)
            {
                options.Add(
                    "y|chunked-compression",
                    "chunked compression mode (PS3)",
                    v => isCompressedInChunks = v != null
                );
            }

            options.Add(
                "l|little-endian",
                "pack data in little-endian mode (default)",
                v => endian = v != null ? Endian.Little : endian
            );

            options.Add(
                "b|big-endian",
                "pack data in big-endian mode",
                v => endian = v != null ? Endian.Big : endian
            );
            
            options.Add(
                "v|verbose",
                "enable verbose logging",
                v => verbose = v != null
            );

            options.Add(
                "h|help",
                "show this message and exit",
                v => showHelp = v != null
            );

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

            if (extras.Count < 1 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_directory", GetExecutableName());
                Console.WriteLine("       {0} [OPTIONS]+ output_vpp input_directory+", GetExecutableName());
                Console.WriteLine("Pack directores into specified Volition package file.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return 2;
            }

            string outputPath;
            var paths = new SortedDictionary<string, string>();

            if (extras.Count == 1)
            {
                outputPath = Path.ChangeExtension(extras[0] + "_PACKED", ".vpp_pc");

                foreach (var path in Directory.GetFiles(extras[0], "*"))
                {
                    var fullPath = Path.GetFullPath(path);
                    var name = Path.GetFileName(fullPath);

                    if (paths.ContainsKey(name) == true)
                    {
                        continue;
                    }

                    paths[name] = fullPath;
                }
            }
            else
            {
                outputPath = extras[0];

                for (int i = 1; i < extras.Count; i++)
                {
                    var directory = extras[i];

                    foreach (var path in Directory.GetFiles(directory, "*"))
                    {
                        var fullPath = Path.GetFullPath(path);
                        var name = Path.GetFileName(fullPath);

                        if (paths.ContainsKey(name) == true)
                        {
                            continue;
                        }

                        paths[name] = fullPath;
                    }
                }
            }

            package.Endian = endian;

            var flags = Package.HeaderFlags.None;

            if (isCompressed == true)
            {
                flags |= Package.HeaderFlags.Compressed;
            }

            if (isCondensed == true)
            {
                flags |= Package.HeaderFlags.Condensed;
            }

            if (isCompressedInChunks == true)
            {
                flags |= Package.HeaderFlags.CompressedInChunks;
            }

            package.Flags = flags;

            this.Build(package, paths, outputPath);
            return 0;
        }
    }
}
