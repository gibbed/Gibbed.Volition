using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Gibbed.Helpers;
using Gibbed.Volition.FileFormats;
using NDesk.Options;

namespace Gibbed.Volition.ConvertVintDoc
{
    internal class Program
    {
        private static void WriteProperty(XmlWriter writer, string name, Gibbed.Volition.FileFormats.Vint.Property property)
        {
            writer.WriteElementString("name", name);
            writer.WriteElementString("type", property.Tag);
            writer.WriteElementString("value", property.ToString());
        }

        private static void WriteObject(XmlWriter writer, Gibbed.Volition.FileFormats.Vint.Object o)
        {
            writer.WriteElementString("name", o.Name);
            writer.WriteElementString("type", o.Type);

            writer.WriteStartElement("baseline");
            foreach (KeyValuePair<string, Gibbed.Volition.FileFormats.Vint.Property> property in o.Baseline)
            {
                writer.WriteStartElement("property");
                WriteProperty(writer, property.Key, property.Value);
                writer.WriteEndElement(); // property
            }
            writer.WriteEndElement(); // baseline

            writer.WriteStartElement("overrides");
            foreach (KeyValuePair<string, Dictionary<string, Gibbed.Volition.FileFormats.Vint.Property>> resolution in o.Overrides)
            {
                writer.WriteStartElement("resolution");
                writer.WriteElementString("name", resolution.Key);

                foreach (KeyValuePair<string, Gibbed.Volition.FileFormats.Vint.Property> property in resolution.Value)
                {
                    writer.WriteStartElement("property");
                    WriteProperty(writer, property.Key, property.Value);
                    writer.WriteEndElement(); // property
                }

                writer.WriteEndElement(); // resolution
            }
            writer.WriteEndElement(); // overrides

            writer.WriteStartElement("children");
            foreach (Gibbed.Volition.FileFormats.Vint.Object child in o.Children)
            {
                writer.WriteStartElement("object");
                WriteObject(writer, child);
                writer.WriteEndElement(); // object
            }
            writer.WriteEndElement(); // children
        }

        private static void WriteDocument(VintFile vint, Stream output)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.OmitXmlDeclaration = true;

            XmlWriter writer = XmlWriter.Create(output, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("root");

            writer.WriteElementString("vint_doc_type", "vint_document");
            writer.WriteElementString("name", Path.ChangeExtension(vint.Name, ".vint_xdoc"));
            writer.WriteElementString("vint_doc_version", "2");
            writer.WriteElementString("anim_time", vint.AnimationTime.ToString());

            writer.WriteStartElement("metadata");
            foreach (KeyValuePair<string, string> metadata in vint.Metadata)
            {
                writer.WriteStartElement("metadata_item");
                writer.WriteElementString("name", metadata.Key);
                writer.WriteElementString("value", metadata.Value);
                writer.WriteEndElement(); // metadata_item
            }
            writer.WriteEndElement(); // metadata

            writer.WriteStartElement("critical_resources");
            writer.WriteStartElement("pegs");
            foreach (string filename in vint.CriticalResources)
            {
                writer.WriteElementString("filename", filename);
            }
            writer.WriteEndElement(); // pegs
            writer.WriteEndElement(); // critical_resources

            writer.WriteStartElement("elements");
            foreach (Gibbed.Volition.FileFormats.Vint.Object element in vint.Elements)
            {
                writer.WriteStartElement("object");
                WriteObject(writer, element);
                writer.WriteEndElement(); // object
            }
            writer.WriteEndElement(); // elements

            writer.WriteStartElement("animations");
            foreach (Gibbed.Volition.FileFormats.Vint.Object animation in vint.Animations)
            {
                writer.WriteStartElement("object");
                WriteObject(writer, animation);
                writer.WriteEndElement(); // object
            }
            writer.WriteEndElement(); // animations

            writer.WriteEndElement(); // root

            writer.WriteEndDocument();

            writer.Flush();
        }

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

            if (extra.Count == 0 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_directory+", GetExecutableName());
                Console.WriteLine("Convert .vint_doc files to .vint_xdoc.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            foreach (string directory in extra)
            {
                foreach (string inputPath in Directory.GetFiles(Path.GetFullPath(directory), "*.vint_doc"))
                {
                    string outputPath = Path.ChangeExtension(inputPath, ".vint_xdoc");
                    
                    if (File.Exists(outputPath) && overwriteFiles == false)
                    {
                        continue;
                    }

                    Console.WriteLine(Path.GetFullPath(inputPath));

                    Stream input = File.OpenRead(inputPath);

                    if (input.ReadValueU32() != 0x3027)
                    {
                        input.Close();
                        continue;
                    }

                    input.Seek(-4, SeekOrigin.Current);

                    VintFile vint = new VintFile();
                    vint.Deserialize(input);

                    Stream output = File.Open(outputPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);

                    WriteDocument(vint, output);

                    output.Close();
                    input.Close();
                }
            }
        }
    }
}
