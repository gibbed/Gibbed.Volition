﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gibbed.Volition.FileFormats;
using NDesk.Options;
using System.IO;

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
            bool compressFiles = false;
            bool compressSolid = false;
            uint packageVersion = 3;

            OptionSet options = new OptionSet()
            {
                {
                    "v|version=", 
                    "the version of the package to create, default 3. " + 
                    "this must be an integer.",
                    (uint v) => packageVersion = v
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

            SortedDictionary<string, string> paths = new SortedDictionary<string, string>();

            for (int i = 1; i < extra.Count; i++)
            {
                string directory = extra[i];

                foreach (string path in Directory.GetFiles(directory, "*"))
                {
                    string fullPath = Path.GetFullPath(path);

                    string name = Path.GetFileName(fullPath);

                    if (Path.HasExtension(name) == false)
                    {
                        continue;
                    }
                    else if (paths.ContainsKey(name))
                    {
                        continue;
                    }

                    paths[name] = fullPath;
                    Console.WriteLine(fullPath);
                }
            }

            Stream output = File.Open(outputPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            
            Package package = new Package(output, false);
            package.Version = packageVersion;
            package.LittleEndian = !bigEndian;

            foreach (KeyValuePair<string, string> value in paths)
            {
                package.SetEntry(value.Key, value.Value);
            }

            Gibbed.Volition.FileFormats.Packages.PackageCompressionType compressionType;

            if (compressFiles == true && compressSolid == true)
            {
                compressionType = Gibbed.Volition.FileFormats.Packages.PackageCompressionType.SolidZlib;
            }
            else if (compressFiles == true)
            {
                compressionType = Gibbed.Volition.FileFormats.Packages.PackageCompressionType.Zlib;
            }
            else
            {
                compressionType = Gibbed.Volition.FileFormats.Packages.PackageCompressionType.None;
            }

            package.Commit(compressionType);

            output.Close();
        }
    }
}
