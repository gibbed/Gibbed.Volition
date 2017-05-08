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
using System.Text;
using System.IO;
using Gibbed.IO;

namespace Gibbed.RedFaction2.FileFormats.Peg
{
    public struct Texture
    {
        public ushort Width;
        public ushort Height;
        public TextureFormat Format;
        public byte FormatArgument;
        public byte Flags;
        public byte FrameCount;
        public byte AnimationDelay;
        public byte MipCount;
        public byte UnknownA;
        public byte UnknownB;
        public string Name;
        public uint DataOffset;

        public static Texture Read(Stream input, Endian endian)
        {
            Texture instance;
            instance.Width = input.ReadValueU16(endian);
            instance.Height = input.ReadValueU16(endian);
            var format = input.ReadValueU8();
            instance.FormatArgument = input.ReadValueU8();
            instance.Flags = input.ReadValueU8();
            instance.FrameCount = input.ReadValueU8();
            instance.AnimationDelay = input.ReadValueU8();
            instance.MipCount = input.ReadValueU8();
            instance.UnknownA = input.ReadValueU8();
            instance.UnknownB = input.ReadValueU8();
            instance.Name = input.ReadString(48, true, Encoding.ASCII);
            instance.DataOffset = input.ReadValueU32(endian);
            if (Enum.IsDefined(typeof(TextureFormat), format) == false)
            {
                throw new FormatException("unknown texture format");
            }
            instance.Format = (TextureFormat)format;
            return instance;
        }

        public static void Write(Stream output, Texture instance, Endian endian)
        {
            throw new NotImplementedException();
        }

        public void Write(Stream output, Endian endian)
        {
            Write(output, this, endian);
        }
    }
}
