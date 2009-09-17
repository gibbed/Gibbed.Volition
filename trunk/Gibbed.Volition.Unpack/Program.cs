using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gibbed.Volition.FileFormats;
using NDesk.Options;

namespace Gibbed.Volition.Unpack
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
            bool overwriteFiles = false;

            OptionSet options = new OptionSet()
            {
                {
                    "o|overwrite",
                    "overwrite files if they already exist", 
                    v => overwriteFiles = v != null
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

            if (extra.Count != 2 || showHelp == true)
            {
                Console.WriteLine("---");
                Console.WriteLine("argument count == {0}", extra.Count);
                Console.WriteLine("extra = {0}", String.Join(", ", extra.ToArray()));
                Console.WriteLine("help == {0}", showHelp);
                Console.WriteLine("---");

                Console.WriteLine("Usage: {0} [OPTIONS]+ input_vpp output_directory", GetExecutableName());
                Console.WriteLine("Unpack specified Volition package file.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            string inputPath = extra[0];
            string outputPath = extra[1];

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

                if (overwriteFiles == false && File.Exists(entryPath) == true)
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
