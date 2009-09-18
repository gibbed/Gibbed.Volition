using System;
using System.Collections.Generic;
using System.IO;
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

        private enum Game
        {
            None,
            RedFactionGuerrilla,
            SaintsRow2,
        }

        private static string LookupRFG(string inputPath)
        {
            string basePath = (string)Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 20500", "InstallLocation", null);
            
            if (basePath == null)
            {
                basePath = (string)Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\GameUX\\Games\\{13E51682-ADC7-4E51-9EBA-1C030781E4A2}", "ConfigApplicationPath", null);
                if (basePath == null)
                {
                    return null;
                }
            }

            string tryPath;

            tryPath = Path.Combine(basePath, inputPath);
            if (File.Exists(tryPath) == true)
            {
                return tryPath;
            }

            string[] subPaths = new string[]
            {
                "build",
                "build\\pc",
                "build\\pc\\cache",
                "build\\dlc01",
                "build\\dlc01\\pc",
                "build\\dlc01\\pc\\cache",
                "build\\dlc02",
                "build\\dlc02\\pc",
                "build\\dlc02\\pc\\cache",
                "build\\dlc03",
                "build\\dlc03\\pc",
                "build\\dlc03\\pc\\cache",
            };

            foreach (string subPath in subPaths)
            {
                tryPath = Path.Combine(Path.Combine(basePath, subPath), inputPath);
                if (File.Exists(tryPath) == true)
                {
                    return tryPath;
                }
            }

            return null;
        }

        private static string LookupSR2(string inputPath)
        {
            string basePath = (string)Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 9480", "InstallLocation", null);

            if (basePath == null)
            {
                return null;
            }

            string tryPath;

            tryPath = Path.Combine(basePath, inputPath);
            if (File.Exists(tryPath) == true)
            {
                return tryPath;
            }

            return null;
        }

        public static void Main(string[] args)
        {
            bool showHelp = false;
            bool overwriteFiles = false;
            Game game = Game.None;

            OptionSet options = new OptionSet()
            {
                {
                    "o|overwrite",
                    "overwrite files if they already exist", 
                    v => overwriteFiles = v != null
                },
                {
                    "rfg",
                    "automatically find RF:G packages",
                    v => game = v != null ? Game.RedFactionGuerrilla : Game.None
                },
                {
                    "sr2",
                    "automatically find SR2 packages",
                    v => game = v != null ? Game.SaintsRow2 : Game.None
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
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_vpp output_directory", GetExecutableName());
                Console.WriteLine("Unpack specified Volition package file.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            string inputPath = extra[0];

            if (game != Game.None && File.Exists(inputPath) == false)
            {
                string newPath = null;

                if (game == Game.RedFactionGuerrilla)
                {
                    newPath = LookupRFG(inputPath);
                }
                else if (game == Game.SaintsRow2)
                {
                    newPath = LookupSR2(inputPath);
                }

                if (newPath != null)
                {
                    inputPath = newPath;
                }
            }
            
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
