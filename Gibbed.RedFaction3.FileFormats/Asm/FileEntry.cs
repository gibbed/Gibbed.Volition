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

using System.IO;
using System.Text;
using Gibbed.IO;
using Gibbed.Volition.FileFormats;

namespace Gibbed.RedFaction3.FileFormats.Asm
{
    public class FileEntry
    {
        public string Name { get; set; }
        public byte Unk1 { get; set; }
        public byte Unk2 { get; set; }
        public byte Unk3 { get; set; }
        public byte Unk4 { get; set; }
        public int HeaderFileSize { get; set; }
        public int DataFileSize { get; set; }

        public void Deserialize(Stream input)
        {
            this.Name = input.ReadStringU16(0x40, Encoding.ASCII, Endian.Little);
            this.Unk1 = input.ReadValueU8();
            this.Unk2 = input.ReadValueU8();
            this.Unk3 = input.ReadValueU8();
            this.Unk4 = input.ReadValueU8();
            this.HeaderFileSize = input.ReadValueS32();
            this.DataFileSize = input.ReadValueS32();
        }

        public void Serialize(Stream output)
        {
            output.WriteValueU16((ushort)this.Name.Length);
            output.WriteString(this.Name.Substring(0, (ushort)this.Name.Length), Encoding.ASCII);
            output.WriteValueU8(this.Unk1);
            output.WriteValueU8(this.Unk2);
            output.WriteValueU8(this.Unk3);
            output.WriteValueU8(this.Unk4);
            output.WriteValueS32(this.HeaderFileSize);
            output.WriteValueS32(this.DataFileSize);
        }
    }
}
