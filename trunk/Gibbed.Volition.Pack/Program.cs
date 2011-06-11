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
using Gibbed.Volition.FileFormats;
using NDesk.Options;
using Packages = Gibbed.Volition.FileFormats.Packages;

namespace Gibbed.Volition.Pack
{
    internal class Program
    {
        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }

        public static void Main(string[] args)
        {
            bool showHelp = false;
            bool bigEndian = false;
            bool verbose = false;
            bool compressFiles = false;
            bool compressSolid = false;
            uint packageVersion = 3;

            OptionSet options = new OptionSet()
            {
                {
                    "p|version=", 
                    "the version of the package to create, default 3. " + 
                    "this must be an integer.",
                    (uint v) => packageVersion = v
                },
                {
                    "v|verbose",
                    "be verbose (list files)",
                    v => verbose = v != null
                },
                {
                    "b|bigendian",
                    "whether the package should be written in big endian mode. " +
                    "this is only useful for non-Windows platforms (such as XBOX).",
                    v => bigEndian = v != null
                },
                {
                    "c|compress",
                    "compress files in the package.",
                    v => compressFiles = v != null
                },
                {
                    "s|solid",
                    "solid compression mode. only used when compression is enabled.",
                    v => compressSolid = v != null
                },
                {
                    "h|help",
                    "show this message and exit", 
                    v => showHelp = v != null
                },
            };

            List<string> extra;

            try
            {
                extra = options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("{0}: ", GetExecutableName());
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `{0} --help' for more information.", GetExecutableName());
                return;
            }

            if (extra.Count < 2 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ output_vpp input_directory+", GetExecutableName());
                Console.WriteLine("Pack files from input directories into a Volition package file.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            string outputPath = extra[0];

            var paths = new SortedDictionary<string, string>();

            if (verbose == true)
            {
                Console.WriteLine("Finding files...");
            }

            for (int i = 1; i < extra.Count; i++)
            {
                var directory = extra[i];

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

            using (var output = File.Open(outputPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var package = new Package(output, false);
                package.Version = packageVersion;
                package.LittleEndian = !bigEndian;

                if (verbose == true)
                {
                    Console.WriteLine("Adding files...");
                }

                foreach (var value in paths)
                {
                    if (verbose == true)
                    {
                        Console.WriteLine(value.Value);
                    }

                    package.SetEntry(value.Key, value.Value);
                }

                Packages.PackageCompressionType compressionType;

                if (compressFiles == true && compressSolid == true)
                {
                    compressionType = Packages.PackageCompressionType.SolidZlib;
                }
                else if (compressFiles == true)
                {
                    compressionType = Packages.PackageCompressionType.Zlib;
                }
                else
                {
                    compressionType = Packages.PackageCompressionType.None;
                }

                if (verbose == true)
                {
                    Console.WriteLine("Writing to disk...");
                }

                package.Commit(compressionType);
            }
        }
    }
}
