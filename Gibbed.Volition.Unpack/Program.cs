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
            RedFactionArmageddon,
            SaintsRow2,
        }

        private static string LookupSteamId(long id)
        {
            var keys = new string[]
            {
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App {0}",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App {0}",
            };

            foreach (var key in keys)
            {
                var path = (string)Microsoft.Win32.Registry.GetValue(string.Format(key, id), "InstallLocation", null);
                if (path != null)
                {
                    return path;
                }
            }

            return null;
        }

        private static string LookupRedFactionGuerrilla(string inputPath)
        {
            string basePath = LookupSteamId(20500);
            if (basePath == null)
            {
                basePath = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\GameUX\Games\{13E51682-ADC7-4E51-9EBA-1C030781E4A2}", "ConfigApplicationPath", null);
                if (basePath == null)
                {
                    basePath = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{A357EF4C-2B6F-4980-ACA9-B1E42A74D7F3}", "InstallLocation", null);
                    if (basePath == null)
                    {
                        basePath = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{A357EF4C-2B6F-4980-ACA9-B1E42A74D7F3}", "InstallLocation", null);
                        if (basePath == null)
                        {
                            return null;
                        }
                    }
                }
            }

            string tryPath;

            tryPath = Path.Combine(basePath, inputPath);
            if (File.Exists(tryPath) == true)
            {
                return tryPath;
            }

            var subPaths = new string[]
            {
                @"build",
                @"build\pc",
                @"build\pc\cache",
                @"build\dlc01",
                @"build\dlc01\pc",
                @"build\dlc01\pc\cache",
                @"build\dlc02",
                @"build\dlc02\pc",
                @"build\dlc02\pc\cache",
                @"build\dlc03",
                @"build\dlc03\pc",
                @"build\dlc03\pc\cache",
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

        private static string LookupRedFactionArmageddon(string inputPath)
        {
            string basePath = LookupSteamId(55110);
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

            var subPaths = new string[]
            {
                @"build",
                @"build\pc",
                @"build\pc\cache",
                @"build\dlc01",
                @"build\dlc01\pc",
                @"build\dlc01\pc\cache",
                @"build\dlc02",
                @"build\dlc02\pc",
                @"build\dlc02\pc\cache",
                @"build\dlc03",
                @"build\dlc03\pc",
                @"build\dlc03\pc\cache",
                @"build\dlc04",
                @"build\dlc04\pc",
                @"build\dlc04\pc\cache",
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

        private static string LookupSaintsRow2(string inputPath)
        {
            string basePath = LookupSteamId(9480);
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
                    "rfa",
                    "automatically find RF:A packages",
                    v => game = v != null ? Game.RedFactionArmageddon : Game.None
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

            if (extra.Count < 1 || extra.Count > 2 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_vpp [output_dir]", GetExecutableName());
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
                    newPath = LookupRedFactionGuerrilla(inputPath);
                }
                if (game == Game.RedFactionArmageddon)
                {
                    newPath = LookupRedFactionArmageddon(inputPath);
                }
                else if (game == Game.SaintsRow2)
                {
                    newPath = LookupSaintsRow2(inputPath);
                }

                if (newPath != null)
                {
                    inputPath = newPath;
                }
            }

            string outputPath = extra.Count > 1 ?
                extra[1] :
                Path.ChangeExtension(extra[0], null);

            using (var input = File.OpenRead(inputPath))
            {
                Directory.CreateDirectory(outputPath);

                var package = new Package(input);

                long counter = 0;
                long skipped = 0;
                long totalCount = package.Keys.Count;

                var totalLength = totalCount.ToString().Length;

                Console.WriteLine("{0} files in package.", totalCount);

                foreach (string name in package.Keys)
                {
                    counter++;

                    var entryPath = Path.Combine(outputPath, name);

                    if (overwriteFiles == false && File.Exists(entryPath) == true)
                    {
                        Console.WriteLine("{1}/{2} !! {0}", name, counter.ToString().PadLeft(totalLength), totalCount);
                        skipped++;
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("{1}/{2} => {0}", name, counter.ToString().PadLeft(totalLength), totalCount);
                    }

                    using (var output = File.Open(entryPath, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        package.ExportEntry(name, output);
                        output.Flush();
                    }
                }

                if (skipped > 0)
                {
                    Console.WriteLine("{0} files not overwritten.", skipped);
                }
            }
        }
    }
}
