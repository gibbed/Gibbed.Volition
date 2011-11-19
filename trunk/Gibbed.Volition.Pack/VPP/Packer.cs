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

            var options = new OptionSet()
            {
                {
                    "c|compress",
                    "compress data",
                    v => isCompressed = v != null
                },
                {
                    "n|condense",
                    "condense data",
                    v => isCondensed = v != null
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

            if (extras.Count != 2 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ output_str2 input_directory+", GetExecutableName());
                Console.WriteLine("Pack directores into specified Volition streams package file.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return 2;
            }

            string outputPath = extras[0];

            var package = new TPackage();
            var paths = new SortedDictionary<string, string>();

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

            this.Build(package, paths, outputPath);
            return 0;
        }
    }
}
