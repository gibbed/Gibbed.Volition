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

using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.IO;
using Gibbed.Volition.FileFormats;

namespace Gibbed.RedFaction3.FileFormats.Asm
{
    public class ContainerEntry
    {
        public string Name { get; set; }
        public byte Type { get; set; }
        public ushort Flags { get; set; }
        public uint DataOffset { get; set; }
        public uint CompressedSize { get; set; }

        public List<int> Sizes { get; set; }
        public List<PrimitiveEntry> Primitives { get; set; }

        public ContainerEntry()
        {
            this.Sizes = new List<int>();
            this.Primitives = new List<PrimitiveEntry>();
        }

        public void Serialize(Stream output, ushort version, Endian endian)
        {
            output.WriteStringU16(this.Name, 0x40, Encoding.ASCII, endian);
            output.WriteValueU8(this.Type);
            output.WriteValueU16(this.Flags, endian);
            output.WriteValueU16((ushort)this.Primitives.Count, endian);
            output.WriteValueU32(this.DataOffset, endian);
            output.WriteValueS32(this.Sizes.Count, endian);
            output.WriteValueU32(this.CompressedSize, endian);

            foreach (var size in this.Sizes)
            {
                output.WriteValueS32(size, endian);
            }

            for (short i = 0; i < (short)this.Primitives.Count; i++)
            {
                this.Primitives[i].Serialize(output, endian);
            }
        }

        public void Deserialize(Stream input, ushort version, Endian endian)
        {
            this.Name = input.ReadStringU16(0x40, Encoding.ASCII, endian);
            this.Type = input.ReadValueU8();
            this.Flags = input.ReadValueU16(endian);
            var primitiveCount = input.ReadValueU16(endian);
            this.DataOffset = input.ReadValueU32(endian);
            var sizeCount = input.ReadValueU32(endian);
            this.CompressedSize = input.ReadValueU32(endian);

            for (ushort i = 0; i < sizeCount; i++)
            {
                this.Sizes.Add(input.ReadValueS32(endian));
            }

            this.Primitives.Clear();
            for (ushort i = 0; i < primitiveCount; i++)
            {
                var primitive = new PrimitiveEntry();
                primitive.Deserialize(input, endian);
                this.Primitives.Add(primitive);
            }
        }
    }
}
