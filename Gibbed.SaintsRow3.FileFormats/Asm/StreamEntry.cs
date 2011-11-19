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

namespace Gibbed.SaintsRow3.FileFormats.Asm
{
    public class StreamEntry
    {
        public string Name { get; set; }
        public byte Unknown1 { get; set; }
        public ushort Flags { get; set; }
        public uint DataOffset { get; set; }
        public uint Unknown6 { get; set; }
        public uint DataCompressedSize { get; set; }
        public List<FileSize> Sizes { get; set; }
        public List<FileEntry> Files { get; set; }

        public StreamEntry()
        {
            this.Sizes = new List<FileSize>();
            this.Files = new List<FileEntry>();
        }

        public void Deserialize(Stream input, ushort version, Endian endian)
        {
            this.Name = input.ReadStringU16(0x40, Encoding.ASCII, endian);
            this.Unknown1 = input.ReadValueU8();
            this.Flags = input.ReadValueU16();
            var fileCount = input.ReadValueU16();
            this.DataOffset = input.ReadValueU32();

            if (version >= 10)
            {
                var unknown = input.ReadStringU16(0x40, Encoding.ASCII, endian);
            }

            var unk7count = input.ReadValueU32();
            var unk7 = input.ReadBytes(unk7count);

            this.Unknown6 = version >= 10 ? 0 : input.ReadValueU32(endian);
            this.DataCompressedSize = input.ReadValueU32(endian);

            this.Sizes.Clear();
            if ((this.Flags & 0x80) != 0 || version >= 11)
            {
                for (ushort i = 0; i < fileCount; i++)
                {
                    var size = new FileSize();
                    size.HeaderFileSize = version >= 9 ? input.ReadValueS32(endian) : -1;
                    size.DataFileSize = input.ReadValueS32(endian);
                    this.Sizes.Add(size);
                }
            }

            this.Files.Clear();
            for (short i = 0; i < fileCount; i++)
            {
                var file = new FileEntry();
                file.Deserialize(input, endian);
                this.Files.Add(file);
            }
        }

        public void Serialize(Stream output, ushort version, Endian endian)
        {
            output.WriteValueU16((ushort)this.Name.Length);
            output.WriteString(this.Name.Substring(0, (ushort)this.Name.Length), Encoding.ASCII);
            output.WriteValueU8(this.Unknown1);
            output.WriteValueU16(this.Flags);
            output.WriteValueS16((short)this.Files.Count);
            output.WriteValueU32(this.DataOffset);
            output.WriteValueS32(this.Sizes.Count);
            output.WriteValueU32(this.Unknown6);

            for (int i = 0; i < this.Sizes.Count; i++)
            {
                if (version >= 9)
                {
                    output.WriteValueS32(this.Sizes[i].DataFileSize, endian);
                }

                output.WriteValueS32(this.Sizes[i].DataFileSize, endian);
            }

            for (short i = 0; i < (short)this.Files.Count; i++)
            {
                this.Files[i].Serialize(output, endian);
            }
        }
    }
}
