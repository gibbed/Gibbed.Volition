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
using System.Globalization;
using System.IO;
using System.Xml;
using Gibbed.IO;
using Gibbed.Volition.FileFormats;
using NDesk.Options;
using Peg = Gibbed.Volition.FileFormats.Peg;

namespace Gibbed.Volition.ConvertPEG
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
            var overwriteFiles = false;
            var mode = Mode.Unknown;

            OptionSet options = new OptionSet()
            {
                { "o|overwrite", "overwrite files if they already exist", v => overwriteFiles = v != null },
                { "a|assemble", "assemble peg file", v => mode = v != null ? Mode.Assemble : mode },
                { "d|disassemble", "disassemble peg file", v => mode = v != null ? Mode.Disassemble : mode },
                { "h|help", "show this message and exit", v => showHelp = v != null },
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

            if (mode == Mode.Unknown && extra.Count > 0)
            {
                // detect mode
                if (Directory.Exists(extra[0]) == true)
                {
                    mode = Mode.Assemble;
                }
                else if (File.Exists(extra[0]) == true)
                {
                    mode = Mode.Disassemble;
                }
            }

            if (extra.Count < 1 ||
                extra.Count > 2 ||
                showHelp == true ||
                mode == Mode.Unknown)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ [-i|-o] input [output]", GetExecutableName());
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            var inputPath = extra[0];

            if (mode == Mode.Assemble)
            {
                throw new NotImplementedException();
            }
            else if (mode == Mode.Disassemble)
            {
                inputPath = Path.GetFullPath(inputPath);

                if (IsHeaderPath(inputPath) == false)
                {
                    Console.WriteLine("Please specify a path to a peg header file (ie, *.peg_pc).");
                    return;
                }

                using (var input = File.OpenRead(inputPath))
                {
                    var magic = input.ReadValueU32(Endian.Little);
                    if (magic != 0x564B4547 &&
                        magic != 0x47454B56)
                    {
                        throw new FormatException("not a peg file");
                    }

                    var version = input.ReadValueU16(magic == 0x564B4547 ? Endian.Little : Endian.Big);
                    if (version != 10 && version != 13)
                    {
                        throw new FormatException("unsupported peg version");
                    }

                    input.Seek(0, SeekOrigin.Begin);

                    if (version == 10)
                    {
                        var peg = new PegFileV10();
                        peg.Deserialize(input);

                        var dataPath = GetDataPath(inputPath);
                        if (dataPath == null)
                        {
                            Console.WriteLine("Could not find data file for '{0}'.", inputPath);
                            return;
                        }

                        using (var data = File.OpenRead(dataPath))
                        {
                            var outputPath = Path.ChangeExtension(inputPath, null);
                            Directory.CreateDirectory(outputPath);

                            var xmlPath = Path.Combine(outputPath, "@peg.xml");
                            var settings = new XmlWriterSettings()
                            {
                                Indent = true,
                            };

                            using (var xml = XmlWriter.Create(xmlPath, settings))
                            {
                                xml.WriteStartDocument();
                                xml.WriteStartElement("peg");
                                xml.WriteAttributeString("version", peg.Version.ToString(CultureInfo.InvariantCulture));
                                xml.WriteAttributeString("platform", peg.Platform.ToString());
                                xml.WriteAttributeString("little_endian", peg.Endian.ToString());

                                if (peg.Textures != null &&
                                    peg.Textures.Count > 0)
                                {
                                    xml.WriteStartElement("textures");
                                    foreach (var texture in peg.Textures)
                                    {
                                        xml.WriteStartElement("texture");
                                        xml.WriteAttributeString("name", texture.Name);

                                        if (texture.Frames != null &&
                                            texture.Frames.Count > 0)
                                        {
                                            var counter = 0;
                                            var countLength =
                                                texture.Frames.Count.ToString(CultureInfo.InvariantCulture).Length;

                                            var baseName = Path.GetFileNameWithoutExtension(texture.Name);
                                            if (string.IsNullOrEmpty(baseName) == true)
                                            {
                                                throw new InvalidOperationException();
                                            }
                                            var basePath = Path.Combine(outputPath, baseName);

                                            xml.WriteStartElement("frames");

                                            foreach (var frame in texture.Frames)
                                            {
                                                xml.WriteStartElement("frame");
                                                xml.WriteAttributeString("width",
                                                                         frame.Width.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("height",
                                                                         frame.Height.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("format", frame.Format.ToString());
                                                xml.WriteAttributeString("flags", frame.Flags.ToString());
                                                xml.WriteAttributeString("levels",
                                                                         frame.Levels.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("animation_delay",
                                                                         frame.Delay.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("u0A",
                                                                         frame.Unknown0A.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("u0C",
                                                                         frame.Unknown0C.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("u18",
                                                                         frame.Unknown18.ToString(
                                                                             CultureInfo.InvariantCulture));

                                                data.Seek(frame.DataOffset, SeekOrigin.Begin);
                                                var buffer = new byte[frame.DataSize];
                                                data.Read(buffer, 0, buffer.Length);

                                                string framePath = basePath;
                                                string dataType;

                                                if (texture.Frames.Count != 1)
                                                {
                                                    framePath += "_" +
                                                                 counter.ToString(CultureInfo.InvariantCulture)
                                                                        .PadLeft(countLength, '0');
                                                }

                                                string actualPath;
                                                if (SaveFrameV10(peg,
                                                                 frame,
                                                                 buffer,
                                                                 framePath,
                                                                 out actualPath,
                                                                 out dataType) == false)
                                                {
                                                    actualPath = Path.ChangeExtension(framePath, ".raw");
                                                    dataType = "raw";

                                                    using (var output = File.Create(actualPath))
                                                    {
                                                        output.Write(buffer, 0, buffer.Length);
                                                    }
                                                }

                                                xml.WriteStartElement("source");
                                                xml.WriteAttributeString("type", dataType);
                                                xml.WriteValue(actualPath.Substring(outputPath.Length + 1));
                                                xml.WriteEndElement();

                                                xml.WriteEndElement();

                                                counter++;
                                            }

                                            xml.WriteEndElement();
                                        }

                                        xml.WriteEndElement();
                                    }
                                    xml.WriteEndElement();
                                }

                                xml.WriteEndElement();
                                xml.WriteEndDocument();
                            }
                        }
                    }
                    else if (version == 13)
                    {
                        var peg = new PegFileV13();
                        peg.Deserialize(input);

                        var dataPath = GetDataPath(inputPath);
                        if (dataPath == null)
                        {
                            Console.WriteLine("Could not find data file for '{0}'.", inputPath);
                            return;
                        }

                        using (var data = File.OpenRead(dataPath))
                        {
                            var outputPath = Path.ChangeExtension(inputPath, null);
                            Directory.CreateDirectory(outputPath);

                            var xmlPath = Path.Combine(outputPath, "@peg.xml");
                            var settings = new XmlWriterSettings()
                            {
                                Indent = true,
                            };

                            using (var xml = XmlWriter.Create(xmlPath, settings))
                            {
                                xml.WriteStartDocument();
                                xml.WriteStartElement("peg");
                                xml.WriteAttributeString("version", peg.Version.ToString(CultureInfo.InvariantCulture));
                                xml.WriteAttributeString("platform", peg.Platform.ToString());
                                xml.WriteAttributeString("little_endian", peg.Endian.ToString());

                                if (peg.Textures != null &&
                                    peg.Textures.Count > 0)
                                {
                                    xml.WriteStartElement("textures");
                                    foreach (var texture in peg.Textures)
                                    {
                                        xml.WriteStartElement("texture");
                                        xml.WriteAttributeString("name", texture.Name);

                                        if (texture.Frames != null &&
                                            texture.Frames.Count > 0)
                                        {
                                            var counter = 0;
                                            var countLength =
                                                texture.Frames.Count.ToString(CultureInfo.InvariantCulture).Length;

                                            var baseName = Path.GetFileNameWithoutExtension(texture.Name);
                                            if (string.IsNullOrEmpty(baseName) == true)
                                            {
                                                throw new InvalidOperationException();
                                            }
                                            var basePath = Path.Combine(outputPath, baseName);

                                            xml.WriteStartElement("frames");

                                            foreach (var frame in texture.Frames)
                                            {
                                                xml.WriteStartElement("frame");
                                                xml.WriteAttributeString("u04",
                                                                         frame.Unknown04.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("width",
                                                                         frame.Width.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("height",
                                                                         frame.Height.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("format", frame.Format.ToString());
                                                xml.WriteAttributeString("flags", frame.Flags.ToString());
                                                xml.WriteAttributeString("levels",
                                                                         frame.Levels.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("animation_delay",
                                                                         frame.Delay.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("u0E",
                                                                         frame.Unknown0E.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("u10",
                                                                         frame.Unknown10.ToString(
                                                                             CultureInfo.InvariantCulture));
                                                xml.WriteAttributeString("u12",
                                                                         frame.Unknown12.ToString(
                                                                             CultureInfo.InvariantCulture));

                                                data.Seek(frame.DataOffset, SeekOrigin.Begin);
                                                var buffer = new byte[frame.DataSize];
                                                data.Read(buffer, 0, buffer.Length);

                                                string framePath = basePath;
                                                string dataType;

                                                if (texture.Frames.Count != 1)
                                                {
                                                    framePath += "_" +
                                                                 counter.ToString(CultureInfo.InvariantCulture)
                                                                        .PadLeft(countLength, '0');
                                                }

                                                string actualPath;
                                                if (SaveFrameV16(peg,
                                                                 frame,
                                                                 buffer,
                                                                 framePath,
                                                                 out actualPath,
                                                                 out dataType) == false)
                                                {
                                                    actualPath = Path.ChangeExtension(framePath, ".raw");
                                                    dataType = "raw";

                                                    using (var output = File.Create(actualPath))
                                                    {
                                                        output.Write(buffer, 0, buffer.Length);
                                                    }
                                                }

                                                xml.WriteStartElement("source");
                                                xml.WriteAttributeString("type", dataType);
                                                xml.WriteValue(actualPath.Substring(outputPath.Length + 1));
                                                xml.WriteEndElement();

                                                xml.WriteEndElement();

                                                counter++;
                                            }

                                            xml.WriteEndElement();
                                        }

                                        xml.WriteEndElement();
                                    }
                                    xml.WriteEndElement();
                                }

                                xml.WriteEndElement();
                                xml.WriteEndDocument();
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

        private static bool IsHeaderPath(string path)
        {
            var extension = Path.GetExtension(path);
            if (extension == null)
            {
                return false;
            }

            return extension.StartsWith(".peg_") ||
                   extension.StartsWith(".cpeg_") ||
                   extension.StartsWith(".cvbm_");
        }

        private static string GetDataPath(string headerPath)
        {
            string prefix = null;
            string suffix = null;

            var extension = Path.GetExtension(headerPath);
            if (string.IsNullOrEmpty(extension) == true)
            {
                throw new InvalidOperationException();
            }

            if (extension.StartsWith(".peg_") == true)
            {
                prefix = ".g_peg_";
                suffix = extension.Substring(5);
            }
            else if (extension.StartsWith(".cpeg_") == true)
            {
                prefix = ".gpeg_";
                suffix = extension.Substring(6);
            }
            else if (extension.StartsWith(".cvbm_") == true)
            {
                prefix = ".gvbm_";
                suffix = extension.Substring(6);
            }

            var dataPath = Path.ChangeExtension(headerPath, prefix + suffix);
            if (File.Exists(dataPath) == true)
            {
                return dataPath;
            }

            return null;
        }

        private static bool SaveFrameV10(
            PegFileV10 peg,
            Peg.FrameV10 frame,
            byte[] buffer,
            string basePath,
            out string finalPath,
            out string type)
        {
            finalPath = Path.ChangeExtension(basePath, ".png");
            type = "png";

            switch (frame.Format)
            {
                case Peg.PixelFormat.A8R8G8B8:
                {
                    var bitmap = ImageHelper.ExportA8R8G8B8(frame.Width, frame.Height, buffer);
                    bitmap.Save(finalPath, System.Drawing.Imaging.ImageFormat.Png);
                    return true;
                }

                case Peg.PixelFormat.DXT1:
                case Peg.PixelFormat.DXT3:
                case Peg.PixelFormat.DXT5:
                {
                    var bitmap = ImageHelper.ExportDXT(frame.Format, frame.Width, frame.Height, buffer);
                    bitmap.Save(finalPath, System.Drawing.Imaging.ImageFormat.Png);
                    return true;
                }
            }

            return false;
        }

        private static bool SaveFrameV16(
            PegFileV13 peg,
            Peg.FrameV13 frame,
            byte[] buffer,
            string basePath,
            out string finalPath,
            out string type)
        {
            finalPath = Path.ChangeExtension(basePath, ".png");
            type = "png";

            switch (frame.Format)
            {
                case Peg.PixelFormat.A8R8G8B8:
                {
                    var bitmap = ImageHelper.ExportA8R8G8B8(frame.Width, frame.Height, buffer);
                    bitmap.Save(finalPath, System.Drawing.Imaging.ImageFormat.Png);
                    return true;
                }

                case Peg.PixelFormat.DXT1:
                case Peg.PixelFormat.DXT3:
                case Peg.PixelFormat.DXT5:
                {
                    var bitmap = ImageHelper.ExportDXT(frame.Format, frame.Width, frame.Height, buffer);
                    bitmap.Save(finalPath, System.Drawing.Imaging.ImageFormat.Png);
                    return true;
                }
            }

            return false;
        }
    }
}
