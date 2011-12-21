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
using System.Xml.XPath;
using Gibbed.IO;
using Gibbed.Volition.FileFormats;
using NDesk.Options;
using Package = Gibbed.Volition.FileFormats.Package;

namespace Gibbed.Volition.Pack.STR2
{
    public class Packer<TPackage, TEntry> : PackerBase<TPackage, TEntry>
        where TPackage : IPackageFile<TEntry>, new()
        where TEntry : IPackageEntry, new()
    {
        private static string GetExecutableName()
        {
            var path = System.Reflection.Assembly.GetEntryAssembly().CodeBase;
            return Path.GetFileName(path);
        }

        public override int Main(string[] args)
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

            if (extras.Count < 1 || extras.Count > 2 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_directory", GetExecutableName());
                Console.WriteLine("       {0} [OPTIONS]+ output_str2 input_directory", GetExecutableName());
                Console.WriteLine("Pack directores into specified Volition streams package file.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return 2;
            }

            string outputPath = extras.Count >= 2 ? extras[0] : Path.ChangeExtension(extras[0] + "_PACKED", ".str2_pc");
            string inputPath = extras.Count >= 2 ? extras[1] : extras[0];

            var package = new TPackage();
            var paths = new List<KeyValuePair<string, string>>();

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

                    paths.Add(new KeyValuePair<string, string>(name, path));
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

            this.Build(package, paths, outputPath);

            return 0;
        }
    }
}
