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
    public class StreamEntry
    {
        public string Name { get; set; }
        public byte Unknown1 { get; set; }
        public short Unknown2 { get; set; }
        public int Unknown4 { get; set; }
        public int Unknown6 { get; set; }
        public List<int> FileSizes { get; set; }
        public List<FileEntry> Files { get; set; }

        public StreamEntry()
        {
            this.FileSizes = new List<int>();
            this.Files = new List<FileEntry>();
        }

        public void Deserialize(Stream input)
        {
            this.Name = input.ReadStringU16(0x40, Encoding.ASCII, Endian.Little);
            this.Unknown1 = input.ReadValueU8();
            this.Unknown2 = input.ReadValueS16();
            short fileCount = input.ReadValueS16();
            this.Unknown4 = input.ReadValueS32();
            int unk7count = input.ReadValueS32();
            this.Unknown6 = input.ReadValueS32();

            this.FileSizes.Clear();
            for (int i = 0; i < unk7count; i++)
            {
                this.FileSizes.Add(input.ReadValueS32());
            }

            this.Files.Clear();
            for (short i = 0; i < fileCount; i++)
            {
                var file = new FileEntry();
                file.Deserialize(input);
                this.Files.Add(file);
            }
        }

        public void Serialize(Stream output)
        {
            output.WriteValueU16((ushort)this.Name.Length);
            output.WriteString(this.Name.Substring(0, (ushort)this.Name.Length), Encoding.ASCII);
            output.WriteValueU8(this.Unknown1);
            output.WriteValueS16(this.Unknown2);
            output.WriteValueS16((short)this.Files.Count);
            output.WriteValueS32(this.Unknown4);
            output.WriteValueS32(this.FileSizes.Count);
            output.WriteValueS32(this.Unknown6);

            for (int i = 0; i < this.FileSizes.Count; i++)
            {
                output.WriteValueS32(this.FileSizes[i]);
            }

            for (short i = 0; i < (short)this.Files.Count; i++)
            {
                this.Files[i].Serialize(output);
            }
        }
    }
}
