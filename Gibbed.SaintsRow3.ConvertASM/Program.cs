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
using System.Xml.Serialization;
using Gibbed.SaintsRow3.FileFormats;
using NDesk.Options;

namespace Gibbed.SaintsRow3.ConvertASM
{
    internal class Program
    {
        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }

        public static void Main(string[] args)
        {
            var mode = Mode.Unknown;
            var showHelp = false;

            var options = new OptionSet()
            {
                {
                    "a|xml2asm",
                    "convert xml to asm",
                    v => mode = v != null ? Mode.ToASM : mode
                },
                {
                    "x|asm2xml",
                    "convert asm to xml",
                    v => mode = v != null ? Mode.ToXML : mode
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
                return;
            }

            if (extras.Count != 2 || showHelp == true || mode == Mode.Unknown)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ -a|-x input_file output_file", GetExecutableName());
                Console.WriteLine(".asm_pc file conversions.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            var inputPath = extras[0];
            var outputPath = extras[1];

            if (mode == Mode.ToXML)
            {
                using (var input = File.OpenRead(inputPath))
                {
                    var asm = new AsmFile();
                    asm.Deserialize(input);

                    using (var output = File.Create(outputPath))
                    {
                        var serializer = new XmlSerializer(typeof(AsmFile));
                        serializer.Serialize(output, asm);
                    }
                }
            }
            else if (mode == Mode.ToASM)
            {
                using (var input = File.OpenRead(inputPath))
                {
                    var serializer = new XmlSerializer(typeof(AsmFile));
                    var asm = (AsmFile)serializer.Deserialize(input);

                    using (var output = File.Create(outputPath))
                    {
                        asm.Serialize(output);
                    }
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
