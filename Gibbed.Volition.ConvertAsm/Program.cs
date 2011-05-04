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
using Gibbed.Volition.FileFormats;
using NDesk.Options;

namespace Gibbed.Volition.ConvertAsm
{
    internal class Program
    {
        /*
        private static void WriteSubEntry(XmlWriter writer, AsmFileEntry subEntry)
        {
            writer.WriteElementString("unk0", subEntry.Name);
            writer.WriteElementString("unk1", subEntry.Unk1.ToString());
            writer.WriteElementString("unk2", subEntry.Unk2.ToString());
            writer.WriteElementString("unk3", subEntry.Unk3.ToString());
            writer.WriteElementString("unk4", subEntry.Unk4.ToString());
            writer.WriteElementString("unk5", subEntry.HeaderFileSize.ToString());
            writer.WriteElementString("unk6", subEntry.DataFileSize.ToString());
        }

        private static void WriteEntry(XmlWriter writer, AsmEntry entry)
        {
            writer.WriteElementString("unk0", entry.Unk0);
            writer.WriteElementString("unk1", entry.Unk1.ToString());
            writer.WriteElementString("unk2", entry.Unk2.ToString());
            writer.WriteElementString("unk4", entry.Unk4.ToString());
            writer.WriteElementString("unk6", entry.Unk6.ToString());

            writer.WriteStartElement("unk7s");
            foreach (int unk7 in entry.FileSizes)
            {
                writer.WriteElementString("unk7", unk7.ToString());
            }
            writer.WriteEndElement();

            writer.WriteStartElement("subentries");
            foreach (AsmFileEntry subEntry in entry.Files)
            {
                writer.WriteStartElement("subentry");
                WriteSubEntry(writer, subEntry);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private static void ReadDocument(AsmFile asm, Stream input)
        {
            XmlReader writer = XmlWriter.Create(output, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("entries");
            writer.WriteAttributeString("version", asm.Version.ToString());

            foreach (AsmEntry entry in asm.Entries)
            {
                writer.WriteStartElement("entry");
                WriteEntry(writer, entry);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
        }

        private static void WriteDocument(AsmFile asm, Stream output)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.OmitXmlDeclaration = true;

            XmlWriter writer = XmlWriter.Create(output, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("entries");
            writer.WriteAttributeString("version", asm.Version.ToString());

            foreach (AsmEntry entry in asm.Entries)
            {
                writer.WriteStartElement("entry");
                WriteEntry(writer, entry);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
        }
        */

        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }

        public static void Main(string[] args)
        {
            bool showHelp = false;
            bool overwriteFiles = false;
            bool xml2asm = false;
            bool asm2xml = false;

            OptionSet options = new OptionSet()
            {
                {
                    "o|overwrite",
                    "overwrite files if they already exist", 
                    v => overwriteFiles = v != null
                },
                {
                    "a|xml2asm",
                    "convert xml to asm",
                    v => xml2asm = v != null
                },
                {
                    "x|asm2xml",
                    "convert asm to xml",
                    v => asm2xml = v != null
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

            if (extra.Count != 2 || showHelp == true || asm2xml == xml2asm)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ -a|-x input_file output_file", GetExecutableName());
                Console.WriteLine(".asm_pc file conversions.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            string inputPath = extra[0];
            string outputPath = extra[1];

            if (File.Exists(outputPath) && overwriteFiles == false)
            {
                return;
            }
            
            if (asm2xml == true)
            {
                Stream input = File.OpenRead(inputPath);
                
                AsmFile asm = new AsmFile();
                asm.Deserialize(input);
                
                Stream output = File.Open(outputPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);

                XmlSerializer serializer = new XmlSerializer(typeof(AsmFile));
                serializer.Serialize(output, asm);

                output.Close();
                input.Close();
            }
            else if (xml2asm == true)
            {
                Stream input = File.OpenRead(inputPath);

                XmlSerializer serializer = new XmlSerializer(typeof(AsmFile));
                AsmFile asm = (AsmFile)serializer.Deserialize(input);

                Stream output = File.Open(outputPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                asm.Serialize(output);

                output.Close();
                input.Close();
            }
        }
    }
}
