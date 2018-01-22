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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Gibbed.IO;
using Gibbed.RedFaction2.FileFormats;
using NDesk.Options;
using Newtonsoft.Json;
using Peg = Gibbed.RedFaction2.FileFormats.Peg;

namespace Gibbed.RedFaction2.ConvertPEG
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
            Disassemble,
            Assemble,
        }

        public static void Main(string[] args)
        {
            var fullAlpha = false;
            var showHelp = false;
            var mode = Mode.Unknown;

            var options = new OptionSet()
            {
                // ReSharper disable AccessToModifiedClosure
                { "full-alpha", "when decoding textures, don't force 1-bit alpha", v => fullAlpha = v != null },
                { "a|assemble", "assemble PEG file", v => mode = v != null ? Mode.Assemble : mode },
                { "d|disassemble", "disassemble PEG file", v => mode = v != null ? Mode.Disassemble : mode },
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
                    mode = Mode.Assemble;
                }
                else if (File.Exists(extras[0]) == true)
                {
                    mode = Mode.Disassemble;
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

            if (mode == Mode.Assemble)
            {
                throw new NotImplementedException();
            }
            else if (mode == Mode.Disassemble)
            {
                var inputPath = Path.GetFullPath(extras[0]);
                var outputBasePath = extras.Count > 1 ? extras[1] : Path.ChangeExtension(inputPath, null);

                using (var input = File.OpenRead(inputPath))
                {
                    var peg = new PegFile();
                    peg.Deserialize(input);

                    if (peg.Textures.Count == 0)
                    {
                        return;
                    }

                    var info = new PegFileInfo()
                    {
                        Endian = peg.Endian,
                        Version = peg.Version,
                        Textures = new List<PegTextureInfo>(),
                    };

                    Directory.CreateDirectory(outputBasePath);

                    for (int i = 0; i < peg.Textures.Count; i++)
                    {
                        var nextDataOffset = i + 1 >= peg.Textures.Count
                                                 ? input.Length
                                                 : peg.Textures[i + 1].DataOffset;

                        var texture = peg.Textures[i];
                        var rawTextureSize = nextDataOffset - texture.DataOffset;
                        var frameSize = ComputeFrameSize(texture);
                        var textureSize = frameSize * texture.FrameCount;
                        if (rawTextureSize < textureSize)
                        {
                            throw new FormatException();
                        }

                        input.Position = texture.DataOffset;

                        var textureInfo = new PegTextureInfo()
                        {
                            Width = texture.Width,
                            Height = texture.Height,
                            Format = texture.Format,
                            FormatArgument = texture.FormatArgument,
                            Flags = texture.Flags,
                            AnimationDelay = texture.AnimationDelay,
                            MipCount = texture.MipCount,
                            UnknownA = texture.UnknownA,
                            UnknownB = texture.UnknownB,
                            Name = texture.Name,
                        };

                        if (texture.FrameCount == 1)
                        {
                            textureInfo.Path = texture.Name + ".png";
                            var outputPath = Path.Combine(outputBasePath, textureInfo.Path);
                            var bytes = input.ReadBytes(textureSize);
                            using (var bitmap = ExportTexture(texture, bytes, fullAlpha == false))
                            {
                                bitmap.Save(outputPath, ImageFormat.Png);
                            }
                        }
                        else
                        {
                            textureInfo.FramePaths = new List<string>();
                            for (int j = 0; j < texture.FrameCount; j++)
                            {
                                var texturePath = texture.Name + "_" + j + ".png";
                                textureInfo.FramePaths.Add(texturePath);
                                var outputPath = Path.Combine(outputBasePath, texturePath);
                                var bytes = input.ReadBytes(frameSize);
                                using (var bitmap = ExportTexture(texture, bytes, fullAlpha == false))
                                {
                                    bitmap.Save(outputPath, ImageFormat.Png);
                                }
                            }
                        }

                        info.Textures.Add(textureInfo);
                    }

                    using (var output = File.Create(Path.Combine(outputBasePath, "@peg.json")))
                    using (var textWriter = new StreamWriter(output, Encoding.UTF8))
                    using (var writer = new JsonTextWriter(textWriter))
                    {
                        writer.Formatting = Formatting.Indented;
                        writer.Indentation = 2;
                        writer.IndentChar = ' ';

                        var serializer = new JsonSerializer();
                        serializer.Serialize(writer, info);
                    }
                }
            }
        }

        private static Bitmap ExportTexture(Peg.Texture texture, byte[] bytes, bool oneBitAlpha)
        {
            switch (texture.Format)
            {
                case Peg.TextureFormat.A1R5G5B5:
                {
                    return ExportTextureA1R5G5B5(texture, bytes);
                }

                case Peg.TextureFormat.Indexed:
                {
                    return ExportTextureIndexed(texture, bytes, oneBitAlpha);
                }

                case Peg.TextureFormat.A8R8G8B8:
                {
                    return ExportTextureA8R8G8B8(texture, bytes, oneBitAlpha);
                }
            }

            throw new NotSupportedException();
        }

        private static Bitmap ExportTextureA1R5G5B5(Peg.Texture texture, byte[] bytes)
        {
            var swappedBytes = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i += 2)
            {
                // rrrrrggg ggbbbbba -> bbbbbggg ggrrrrra
                var r = (byte)(bytes[i + 0] & 0x1F);
                var b = (byte)((bytes[i + 1] & 0x7C) >> 2);
                swappedBytes[i + 0] = bytes[i + 0];
                swappedBytes[i + 0] &= 0xE0;
                swappedBytes[i + 0] |= b;
                swappedBytes[i + 1] = bytes[i + 1];
                swappedBytes[i + 1] &= 0x83;
                swappedBytes[i + 1] |= (byte)(r << 2);
            }

            var bitmap = new Bitmap(texture.Width, texture.Height, PixelFormat.Format16bppArgb1555);
            var area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            var scan = bitmapData.Scan0;
            int dataOffset = 0;
            var pitch = bitmap.Width * 2;
            for (int y = 0; y < bitmap.Height; y++)
            {
                Marshal.Copy(swappedBytes, dataOffset, scan, pitch);
                scan += bitmapData.Stride;
                dataOffset += pitch;
            }
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }

        private static Bitmap ExportTextureA8R8G8B8(Peg.Texture texture, byte[] bytes, bool oneBitAlpha)
        {
            var swappedBytes = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i += 4)
            {
                swappedBytes[i + 0] = bytes[i + 2];
                swappedBytes[i + 1] = bytes[i + 1];
                swappedBytes[i + 2] = bytes[i + 0];
                swappedBytes[i + 3] = oneBitAlpha == false
                                          ? bytes[i + 3]
                                          : (byte)(bytes[i + 3] != 0 ? 0xFF : 0);
            }

            var bitmap = new Bitmap(texture.Width, texture.Height, PixelFormat.Format32bppArgb);
            var area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            var scan = bitmapData.Scan0;
            int dataOffset = 0;
            var pitch = bitmap.Width * 4;
            for (int y = 0; y < bitmap.Height; y++)
            {
                Marshal.Copy(swappedBytes, dataOffset, scan, pitch);
                scan += bitmapData.Stride;
                dataOffset += pitch;
            }
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }

        private static int MungePaletteIndex(int value)
        {
            // zzzabzzz -> zzzbazzz
            // I'd really like to know WTF.
            return (value >> 1) & 0x08 |
                   (value << 1) & 0x10 |
                   (value & 0xE7);
        }

        private static Bitmap ExportTextureIndexed(Peg.Texture texture, byte[] bytes, bool oneBitAlpha)
        {
            var bitmap = new Bitmap(texture.Width, texture.Height, PixelFormat.Format8bppIndexed);

            var palette = bitmap.Palette;
            int dataOffset;
            switch (texture.FormatArgument)
            {
                case 1:
                {
                    for (int i = 0, o = 0; i < 256; i++, o += 2)
                    {
                        var r = (bytes[o + 0] & 0x1F) << 3;
                        var g = (((bytes[o + 0] & 0xE0) >> 5) | ((bytes[o + 1] & 0x03) << 3)) << 3;
                        var b = (bytes[o + 1] & 0x7C) << 1;
                        var a = (bytes[o + 1] & 0x80) != 0 ? 0xFF : 0x00;
                        palette.Entries[MungePaletteIndex(i)] = Color.FromArgb(a, r, g, b);
                    }
                    dataOffset = 512;
                    break;
                }

                case 2:
                {
                    for (int i = 0, o = 0; i < 256; i++, o += 4)
                    {
                        palette.Entries[MungePaletteIndex(i)] = Color.FromArgb(
                            oneBitAlpha == false
                                ? bytes[o + 3]
                                : bytes[o + 3] != 0 ? 0xFF : 0,
                            bytes[o + 0],
                            bytes[o + 1],
                            bytes[o + 2]);
                    }
                    dataOffset = 1024;
                    break;
                }

                default:
                {
                    throw new NotSupportedException();
                }
            }
            bitmap.Palette = palette;

            var area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            var scan = bitmapData.Scan0;
            var pitch = texture.Width;
            for (int y = 0; y < bitmap.Height; y++)
            {
                Marshal.Copy(bytes, dataOffset, scan, pitch);
                scan += bitmapData.Stride;
                dataOffset += pitch;
            }
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }

        private static int ComputeFrameSize(Peg.Texture texture)
        {
            int blockSize = 0;
            var mipWidth = (int)texture.Width;
            var mipHeight = (int)texture.Height;
            for (int i = 0; i < texture.MipCount; i++)
            {
                blockSize += mipWidth * mipHeight;
                mipWidth /= 2;
                mipHeight /= 2;
            }

            int size;
            switch (texture.Format)
            {
                case (Peg.TextureFormat)1:
                case (Peg.TextureFormat)2:
                case (Peg.TextureFormat)13:
                case (Peg.TextureFormat)14:
                {
                    throw new NotSupportedException();
                }

                case Peg.TextureFormat.A8R8G8B8:
                {
                    size = 4 * blockSize;
                    break;
                }

                case Peg.TextureFormat.A1R5G5B5:
                case (Peg.TextureFormat)12:
                {
                    size = 2 * blockSize;
                    break;
                }

                case Peg.TextureFormat.Indexed:
                {
                    int paletteSize;
                    switch (texture.FormatArgument)
                    {
                        case 1:
                        {
                            paletteSize = 512;
                            break;
                        }

                        case 2:
                        {
                            paletteSize = 1024;
                            break;
                        }

                        default:
                        {
                            throw new NotSupportedException();
                        }
                    }
                    size = paletteSize + blockSize;
                    break;
                }

                case (Peg.TextureFormat)5:
                {
                    int paletteSize;
                    switch (texture.FormatArgument)
                    {
                        case 1:
                        {
                            paletteSize = 32;
                            break;
                        }

                        case 2:
                        {
                            paletteSize = 64;
                            break;
                        }

                        default:
                        {
                            throw new NotSupportedException();
                        }
                    }
                    size = paletteSize + blockSize / 2;
                    break;
                }

                case (Peg.TextureFormat)6:
                case (Peg.TextureFormat)9:
                case (Peg.TextureFormat)10:
                {
                    throw new NotSupportedException();
                }

                case (Peg.TextureFormat)11:
                {
                    int paletteSize;
                    switch (texture.FormatArgument)
                    {
                        case 1:
                        {
                            paletteSize = 512;
                            break;
                        }

                        case 2:
                        {
                            paletteSize = 1024;
                            break;
                        }

                        default:
                        {
                            throw new NotSupportedException();
                        }
                    }
                    size = paletteSize + blockSize;
                    throw new NotSupportedException();
                    break;
                }

                default:
                {
                    throw new NotSupportedException();
                }
            }

            return size;
        }
    }
}
