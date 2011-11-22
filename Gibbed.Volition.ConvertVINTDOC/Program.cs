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
using System.Linq;
using System.Xml;
using Gibbed.IO;
using Gibbed.Volition.FileFormats;
using NDesk.Options;
using Interface = Gibbed.Volition.FileFormats.Interface;

namespace Gibbed.Volition.ConvertVINTDOC
{
    internal class Program
    {
        private static Dictionary<uint, string> PropertyNames
            = new Dictionary<uint, string>();

        static Program()
        {
            var propertyNames = new string[]
			{
                // document
                "document_name",

				// element
				"render_mode",
				"visible",
				"mask",
				"offset",
				"anchor",
				"tint",
				"alpha",
				"depth",
                "mouse_depth",
				"screen_size",
				"screen_nw",
				"screen_se",
				"rotation",
				"scale",
				"auto_offset",
				"unscaled_size",
                "background",

				// group
				"screen_size",
				"screen_se",
				"screen_nw",
				"offset",

				// animation
				"start_time",
				"is_paused",
				"target_handle",

				// tween
				"target_handle",
				"target_name",
				"target_property",
				"state",
				"start_time",
				"duration",
				"start_value",
				"end_value",
				"loop_mode",
				"algorithm",
				"max_loops",
				"start_event",
				"end_event",
				"per_frame_event",
				"start_value_type",
				"end_value_type",

				// bitmap
				"custom_source_coords",
				"image",
				"image_crc",
				"source_nw",
				"source_se",

				// gradient
				"gradient_nw",
				"gradient_ne",
				"gradient_sw",
				"gradient_se",
				"alpha_nw",
				"alpha_ne",
				"alpha_sw",
				"alpha_se",

				// text
				"font",
				"text_tag",
				"text_tag_crc",
				"text_scale",
				"word_wrap",
				"wrap_width",
				"vert_align",
				"horz_align",
				"force_case",
				"leading",
                "kerning",
				"insert_values",
				"screen_size",
				"line_frame_enable",
				"line_frame_w",
				"line_frame_m",
				"line_frame_e",
                "shadow_enabled",
                "shadow_offset",
                "shadow_tint",
                "shadow_alpha",
                "background",

				// point
				"screen_size",

				// clip
				"clip_size",
				"clip_enabled",

				// bitmap_circle
				"image",
				"screen_size",
				"source_nw",
				"source_se",
				"start_angle",
				"end_angle",
				"num_wedges",

				// sr2_map
				"zoom",
				"map_mode",

				// video
				"vid_id_handle",
                "vid_name",
                "is_paused",
                "is_stopped",
                "is_looped",
                "frame",
                "end_event",
                "frame_event",
                "frame_event_num",
			};

            foreach (var propertyName in propertyNames)
            {
                var hash = propertyName.HashVolitionCRC();
                if (PropertyNames.ContainsKey(hash) == true &&
                    PropertyNames[hash] == propertyName)
                {
                    continue;
                }

                PropertyNames.Add(propertyName.HashVolitionCRC(), propertyName);
            }
        }

        private static void WriteProperty(XmlWriter writer, KeyValuePair<uint, Interface.IProperty> kv)
        {
            if (PropertyNames.ContainsKey(kv.Key) == false)
            {
                writer.WriteElementString("hash", kv.Key.ToString("X8"));
            }
            else
            {
                writer.WriteElementString("name", PropertyNames[kv.Key]);
            }

            writer.WriteElementString("type", kv.Value.Tag);
            writer.WriteElementString("value", kv.Value.ToString());
        }

        private static void WriteObject(XmlWriter writer, Interface.Object o)
        {
            writer.WriteElementString("name", o.Name);
            writer.WriteElementString("type", o.Type);

            writer.WriteStartElement("baseline");
            foreach (var kv in o.Baseline.Properties)
            {
                writer.WriteStartElement("property");
                WriteProperty(writer, kv);
                writer.WriteEndElement(); // property
            }
            writer.WriteEndElement(); // baseline

            writer.WriteStartElement("overrides");
            foreach (var resolution in o.Overrides)
            {
                writer.WriteStartElement("resolution");
                writer.WriteElementString("name", resolution.Key);

                foreach (var kv in resolution.Value.Properties)
                {
                    writer.WriteStartElement("property");
                    WriteProperty(writer, kv);
                    writer.WriteEndElement(); // property
                }

                writer.WriteEndElement(); // resolution
            }
            writer.WriteEndElement(); // overrides

            writer.WriteStartElement("children");
            foreach (Interface.Object child in o.Children)
            {
                writer.WriteStartElement("object");
                WriteObject(writer, child);
                writer.WriteEndElement(); // object
            }
            writer.WriteEndElement(); // children
        }

        private static void WriteDocument(InterfaceFile vint, Stream output)
        {
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.OmitXmlDeclaration = true;
            settings.CheckCharacters = false;

            var writer = XmlWriter.Create(output, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("root");

            writer.WriteElementString("vint_doc_type", "vint_document");
            writer.WriteElementString("name", Path.ChangeExtension(vint.Name, ".vint_xdoc"));
            writer.WriteElementString("vint_doc_version", "2");
            writer.WriteElementString("anim_time", vint.AnimationTime.ToString());

            writer.WriteStartElement("metadata");
            foreach (var metadata in vint.Metadata)
            {
                writer.WriteStartElement("metadata_item");
                writer.WriteElementString("name", metadata.Name);
                writer.WriteElementString("value", metadata.Value);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("critical_resources");
            {
                writer.WriteStartElement("pegs");
                {
                    writer.WriteStartElement("autoload");
                    {
                        foreach (var resource in vint.CriticalResources
                            .Where(r =>
                                r.Type == Interface.CriticalResourceType.Peg &&
                                r.Autoload == true))
                        {
                            writer.WriteElementString("filename", resource.Name);
                        }
                    }
                    writer.WriteEndElement();

                    foreach (var resource in vint.CriticalResources
                        .Where(r =>
                            r.Type == Interface.CriticalResourceType.Peg &&
                            r.Autoload == false))
                    {
                        writer.WriteElementString("filename", resource.Name);
                    }
                }
                writer.WriteEndElement();

                writer.WriteStartElement("documents");
                {
                    foreach (var resource in vint.CriticalResources
                        .Where(r =>
                            r.Type == Interface.CriticalResourceType.Document))
                    {
                        writer.WriteElementString("filename", resource.Name);
                    }
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("elements");
            foreach (Interface.Object element in vint.Elements)
            {
                writer.WriteStartElement("object");
                WriteObject(writer, element);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("animations");
            foreach (Interface.Object animation in vint.Animations)
            {
                writer.WriteStartElement("object");
                WriteObject(writer, animation);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndElement();

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

            var options = new OptionSet()
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

            if (extras.Count == 0 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_directory+", GetExecutableName());
                Console.WriteLine("Convert .vint_doc files to .vint_xdoc.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            foreach (string directory in extras)
            {
                foreach (string inputPath in Directory.GetFiles(Path.GetFullPath(directory), "*.vint_doc"))
                {
                    var outputPath = Path.ChangeExtension(inputPath, ".vint_xdoc");
                    
                    if (overwriteFiles == false &&
                        File.Exists(outputPath) == true)
                    {
                        continue;
                    }

                    Console.WriteLine(Path.GetFullPath(inputPath));

                    using (var input = File.OpenRead(inputPath))
                    {
                        if (input.ReadValueU32() != 0x3027)
                        {
                            input.Close();
                            continue;
                        }

                        input.Seek(-4, SeekOrigin.Current);

                        var vint = new InterfaceFile();
                        vint.Deserialize(input);

                        using (var output = File.Open(outputPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            WriteDocument(vint, output);
                        }
                    }
                }
            }
        }
    }
}
