using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zlib;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using NConsoler;
using System.IO;
using Gibbed.Helpers;
using Gibbed.Volition.FileFormats;

namespace Gibbed.Volition.Packager
{
    internal partial class Program
    {
        [Action(Description = "Unpack a Volition package (*.vpp*)")]
        public static void Unpack(
            [Required(Description = "input vpp file")]
            string inputPath,
            [Required(Description = "output directory")]
            string outputPath,
            [Optional(false, "ow", Description = "overwrite existing files")]
            bool overwrite)
        {
            Stream input = File.OpenRead(inputPath);
            Directory.CreateDirectory(outputPath);

            Package package = new Package(input);

            long counter = 0;
            long skipped = 0;
            long totalCount = package.Keys.Count;

            Console.WriteLine("{0} files in package.", totalCount);

            foreach (string name in package.Keys)
            {
                counter++;

                string entryPath = Path.Combine(outputPath, name);

                if (overwrite == false && File.Exists(entryPath) == true)
                {
                    Console.WriteLine("{1:D4}/{2:D4} !! {0}", name, counter, totalCount);
                    skipped++;
                    continue;
                }
                else
                {
                    Console.WriteLine("{1:D4}/{2:D4} => {0}", name, counter, totalCount);
                }

                Stream output = File.Open(entryPath, FileMode.Create, FileAccess.Write, FileShare.Read);
                package.ExportEntry(name, output);
                output.Flush();
                output.Close();
            }

            input.Close();

            if (skipped > 0)
            {
                Console.WriteLine("{0} files not overwritten.", skipped);
            }
        }
    }
}
