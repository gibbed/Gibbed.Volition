/* Copyright (c) 2017 Rick (rick 'at' gibbed 'dot' us)
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
using NDesk.Options;

namespace Gibbed.RedFaction2.UnpackTOCGroup
{
    internal class Program
    {
        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }

        public static void Main(string[] args)
        {
            var showHelp = false;

            var options = new OptionSet()
            {
                { "h|help", "show this message and exit", v => showHelp = v != null },
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
                return;
            }

            if (extras.Count < 1 || extras.Count > 2 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ input.toc_group [output_dir]", GetExecutableName());
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            var inputPath = Path.GetFullPath(extras[0]);
            var outputBasePath = extras.Count > 1 ? extras[1] : Path.ChangeExtension(inputPath, null) + "_unpack";

            FileFormats.TableOfContentsGroupFile toc;
            using (var input = File.OpenRead(inputPath))
            {
                toc = new FileFormats.TableOfContentsGroupFile();
                toc.Deserialize(input, Endian.Little);
            }

            if (toc.InternalEntries.Count == 0)
            {
                return;
            }

            var inputBasePath = inputPath;

            string inputDataPath = null;
            while (inputBasePath != null)
            {
                inputBasePath = Path.GetDirectoryName(inputBasePath);
                var candidatePath = Path.Combine(inputBasePath ?? "", toc.InternalDataPath);
                if (File.Exists(candidatePath) == true)
                {
                    inputDataPath = candidatePath;
                    break;
                }
            }

            if (inputDataPath == null)
            {
                Console.WriteLine("Could not find packfile.");
                return;
            }

            using (var input = File.OpenRead(inputDataPath))
            {
                foreach (var entry in toc.InternalEntries)
                {
                    var outputPath = Path.Combine(outputBasePath, entry.Name);
                    var outputParentPath = Path.GetDirectoryName(outputPath);
                    if (string.IsNullOrEmpty(outputParentPath) == false)
                    {
                        Directory.CreateDirectory(outputParentPath);
                    }
                    using (var output = File.Create(outputPath))
                    {
                        input.Position = entry.Offset;
                        output.WriteFromStream(input, entry.Size);
                    }
                }
            }
        }
    }
}
