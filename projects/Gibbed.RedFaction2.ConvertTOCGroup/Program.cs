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

namespace Gibbed.RedFaction2.ConvertTOCGroup
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
            var dataPath = "pc_media";

            var options = new OptionSet()
            {
                { "d|data-path=", "set data path", v => dataPath = v },
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
                Console.WriteLine("Usage: {0} [OPTIONS]+ input.toc_group [output.toc_group]", GetExecutableName());
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            var inputPath = Path.GetFullPath(extras[0]);
            var outputPath = extras.Count > 1
                                 ? extras[1]
                                 : Path.ChangeExtension(inputPath, null) + "_convert.toc_group";

            FileFormats.TableOfContentsGroupFile toc;
            using (var input = File.OpenRead(inputPath))
            {
                toc = new FileFormats.TableOfContentsGroupFile();
                toc.Deserialize(input, Endian.Little);
            }

            foreach (var entry in toc.InternalEntries)
            {
                toc.ExternalEntries.Add(new FileFormats.TableOfContentsGroupFile.ExternalEntry()
                {
                    Name = entry.Name,
                    Size = entry.Size,
                    Path = Path.Combine(dataPath, entry.Name)
                });
            }

            toc.InternalPriority = 0;
            toc.InternalDataPath = null;
            toc.InternalEntries.Clear();

            using (var output = File.Create(outputPath))
            {
                toc.Serialize(output, Endian.Little);
            }
        }
    }
}
