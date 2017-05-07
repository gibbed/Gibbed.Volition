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
using System.IO;
using Gibbed.IO;

namespace Gibbed.Volition.FileFormats.Peg
{
    public class FrameV13
    {
        public uint DataOffset;
        public uint Unknown04;
        public ushort Width;
        public ushort Height;
        public PixelFormat Format;
        public ushort Unknown0E;
        public ushort Unknown10;
        public ushort Unknown12;
        public ushort FrameCount;
        public TextureFlags Flags;
        public ushort Unknown20;
        public byte Delay;
        public byte Levels;
        public uint DataSize;

        public void Serialize(Stream output, Endian endian)
        {
            output.WriteValueU32(this.DataOffset, endian);
            output.WriteValueU32(this.Unknown04, endian);
            output.WriteValueU16(this.Width, endian);
            output.WriteValueU16(this.Height, endian);
            output.WriteValueU16((ushort)this.Format, endian);
            output.WriteValueU16(this.Unknown0E, endian);
            output.WriteValueU16(this.Unknown10, endian);
            output.WriteValueU16(this.Unknown12, endian);
            output.WriteValueU16(this.FrameCount, endian);
            output.WriteValueU16((ushort)this.Flags, endian);

            for (int offset = 0x18; offset <= 0x1C; offset += 0x04)
            {
                output.WriteValueU32(0, endian);
            }
            
            output.WriteValueU16(this.Unknown20);
            output.WriteValueU8(this.Delay);
            output.WriteValueU8(this.Levels);
            output.WriteValueU32(this.DataSize, endian);

            for (int offset = 0x28; offset <= 0x48; offset += 0x04)
            {
                output.WriteValueU32(0, endian);
            }
        }

        public void Deserialize(Stream input, Endian endian)
        {
            this.DataOffset = input.ReadValueU32(endian);

            this.Unknown04 = input.ReadValueU32(endian);
            if (this.Unknown04 != 0)
            {
                throw new FormatException();
            }

            this.Width = input.ReadValueU16(endian);
            this.Height = input.ReadValueU16(endian);

            var format = input.ReadValueU16(endian);
            if (Enum.IsDefined(typeof(PixelFormat), format) == false)
            {
                throw new FormatException("unknown pixel format");
            }
            this.Format = (PixelFormat)format;

            this.Unknown0E = input.ReadValueU16(endian);
            this.Unknown10 = input.ReadValueU16(endian);
            this.Unknown12 = input.ReadValueU16(endian);
            this.FrameCount = input.ReadValueU16(endian);
            this.Flags = (TextureFlags)input.ReadValueU16(endian);

            // name pointer
            // unknown runtime value
            for (int offset = 0x18; offset <= 0x1C; offset += 0x04)
            {
                var runtime = input.ReadValueU32(endian);
                if (runtime != 0)
                {
                    throw new FormatException("non-zero runtime values");
                }
            }

            this.Unknown20 = input.ReadValueU16();
            this.Delay = input.ReadValueU8(); // seems to be related to FrameCount - timing related?
            this.Levels = input.ReadValueU8(); // refer to MSDN on IDirect3DDevice9::CreateTexture
            this.DataSize = input.ReadValueU32(endian);

            if (this.Unknown20 != 0)
            {
                throw new InvalidOperationException();
            }

            for (int offset = 0x28; offset <= 0x44; offset += 0x04)
            {
                var runtime = input.ReadValueU32(endian);
                if (runtime != 0)
                {
                    throw new FormatException("non-zero runtime values");
                }
            }
        }
    }
}
