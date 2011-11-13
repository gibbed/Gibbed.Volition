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
using System.IO;
using Gibbed.IO;

namespace Gibbed.Volition.FileFormats.Peg
{
    public class Frame
    {
        public uint DataOffset;
        public ushort Width;
        public ushort Height;
        public PixelFormat Format;
        public ushort Unknown0A;
        public uint Unknown0C;
        public ushort FrameCount;
        public TextureFlags Flags;
        public ushort Unknown18;
        public byte Delay;
        public byte Levels;
        public uint DataSize;

        public void Deserialize(Stream input, Endian endian)
        {
            this.DataOffset = input.ReadValueU32(endian);
            this.Width = input.ReadValueU16(endian);
            this.Height = input.ReadValueU16(endian);

            var format = input.ReadValueU16(endian);
            if (Enum.IsDefined(typeof(PixelFormat), format) == false)
            {
                throw new FormatException("unknown pixel format");
            }
            this.Format = (PixelFormat)format;

            this.Unknown0A = input.ReadValueU16(endian);
            this.Unknown0C = input.ReadValueU32(endian);
            this.FrameCount = input.ReadValueU16(endian);
            this.Flags = (TextureFlags)input.ReadValueU16(endian);

            input.Seek(4, SeekOrigin.Current);
            //var namePointer = input.ReadValueU32(littleEndian);

            this.Unknown18 = input.ReadValueU16();
            this.Delay = input.ReadValueU8(); // seems to be related to FrameCount - timing related?
            this.Levels = input.ReadValueU8(); // refer to MSDN on IDirect3DDevice9::CreateTexture
            this.DataSize = input.ReadValueU32(endian);

            if (this.Unknown18 != 0)
            {
                //throw new InvalidOperationException();
            }

            var nextFramePointer = input.ReadValueU32(endian);
            var prevFramePointer = input.ReadValueU32(endian);
            var d3dFormat = input.ReadValueU32(endian);
            var d3dTexture = input.ReadValueU32(endian);

            if (nextFramePointer != 0 ||
                prevFramePointer != 0 ||
                d3dFormat != 0 ||
                d3dTexture != 0)
            {
                throw new FormatException("non-zero runtime values");
            }
        }

        public void Serialize(Stream output, Endian endian)
        {
            output.WriteValueU32(this.DataOffset, endian);
            output.WriteValueU16(this.Width, endian);
            output.WriteValueU16(this.Height, endian);
            output.WriteValueU16((ushort)this.Format, endian);
            output.WriteValueU16(this.Unknown0A, endian);
            output.WriteValueU32(this.Unknown0C, endian);
            output.WriteValueU16(this.FrameCount, endian);
            output.WriteValueU16((ushort)this.Flags, endian);
            output.WriteValueU32(0, endian);
            output.WriteValueU16(this.Unknown18);
            output.WriteValueU8(this.Delay);
            output.WriteValueU8(this.Levels);
            output.WriteValueU32(this.DataSize, endian);
            output.WriteValueU32(0, endian);
            output.WriteValueU32(0, endian);
            output.WriteValueU32(0, endian);
            output.WriteValueU32(0, endian);
        }
    }
}
