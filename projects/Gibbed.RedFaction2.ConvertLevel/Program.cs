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
using System.Runtime.InteropServices;
using System.Text;
using Gibbed.IO;
using Gibbed.RedFaction2.FileFormats;
using NDesk.Options;
using Newtonsoft.Json;
using Peg = Gibbed.RedFaction2.FileFormats.Peg;

namespace Gibbed.RedFaction2.ConvertLevel
{
    internal class Program
    {
        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }

        private enum Mode
        {
            Unknown,
            Export,
            Import,
        }

        public static void Main(string[] args)
        {
            var showHelp = false;
            var mode = Mode.Unknown;

            var options = new OptionSet()
            {
                // ReSharper disable AccessToModifiedClosure
                { "e|export", "export level data", v => mode = v != null ? Mode.Export : mode },
                { "i|import", "import level data", v => mode = v != null ? Mode.Import : mode },
                { "h|help", "show this message and exit", v => showHelp = v != null },
                // ReSharper restore AccessToModifiedClosure
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

            if (mode == Mode.Unknown && extras.Count > 0)
            {
                // detect mode
                if (Directory.Exists(extras[0]) == true)
                {
                    mode = Mode.Import;
                }
                else if (File.Exists(extras[0]) == true)
                {
                    mode = Mode.Export;
                }
            }

            if (extras.Count < 1 || extras.Count > 2 ||
                showHelp == true || mode == Mode.Unknown)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ [-a|-d] input [output]", GetExecutableName());
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            if (mode == Mode.Import)
            {
                throw new NotImplementedException();
            }
            else if (mode == Mode.Export)
            {
                var inputPath = Path.GetFullPath(extras[0]);
                var outputBasePath = extras.Count > 1 ? extras[1] : Path.ChangeExtension(inputPath, null);

                var level = new LevelFile();
                using (var input = File.OpenRead(inputPath))
                {
                    level.Deserialize(input);
                }

                Directory.CreateDirectory(outputBasePath);

                foreach (var kv in level.Metadatas)
                {
                    var outputPath = Path.Combine(outputBasePath, "metadata", kv.Key + ".json");
                    
                    var outputParentPath = Path.GetDirectoryName(outputPath);
                    if (string.IsNullOrEmpty(outputParentPath) == false)
                    {
                        Directory.CreateDirectory(outputParentPath);
                    }

                    using (var output = File.Create(Path.Combine(outputPath)))
                    using (var textWriter = new StreamWriter(output, Encoding.UTF8))
                    using (var writer = new JsonTextWriter(textWriter))
                    {
                        writer.Formatting = Formatting.Indented;
                        writer.Indentation = 2;
                        writer.IndentChar = ' ';
                        kv.Value.ExportJson(writer);
                    }
                }

                foreach (var kv in level.Elements)
                {
                    var outputPath = Path.Combine(outputBasePath, "data", kv.Key + ".json");

                    var outputParentPath = Path.GetDirectoryName(outputPath);
                    if (string.IsNullOrEmpty(outputParentPath) == false)
                    {
                        Directory.CreateDirectory(outputParentPath);
                    }

                    using (var output = File.Create(Path.Combine(outputPath)))
                    using (var textWriter = new StreamWriter(output, Encoding.UTF8))
                    using (var writer = new JsonTextWriter(textWriter))
                    {
                        writer.Formatting = Formatting.Indented;
                        writer.Indentation = 2;
                        writer.IndentChar = ' ';
                        kv.Value.ExportJson(writer);
                    }
                }

                using (var output = File.Create(Path.Combine(outputBasePath, "level.json")))
                using (var textWriter = new StreamWriter(output, Encoding.UTF8))
                using (var writer = new JsonTextWriter(textWriter))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.Indentation = 2;
                    writer.IndentChar = ' ';

                    //var serializer = new JsonSerializer();
                    //serializer.Serialize(writer, info);
                }
            }
        }
    }
}
