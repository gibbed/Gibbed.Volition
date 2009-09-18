using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats
{
    public class AsmSubEntry
    {
        public string Unk0;
        public byte Unk1;
        public byte Unk2;
        public byte Unk3;
        public byte Unk4;
        public int Unk5;
        public int Unk6;

        public void Deserialize(Stream input)
        {
            this.Unk0 = input.ReadStringASCII(input.ReadValueU16());
            this.Unk1 = input.ReadValueU8();
            this.Unk2 = input.ReadValueU8();
            this.Unk3 = input.ReadValueU8();
            this.Unk4 = input.ReadValueU8();
            this.Unk5 = input.ReadValueS32();
            this.Unk6 = input.ReadValueS32();
        }

        public void Serialize(Stream output)
        {
            output.WriteValueU16((ushort)this.Unk0.Length);
            output.WriteStringASCII(this.Unk0.Substring(0, (ushort)this.Unk0.Length));
            output.WriteValueU8(this.Unk1);
            output.WriteValueU8(this.Unk2);
            output.WriteValueU8(this.Unk3);
            output.WriteValueU8(this.Unk4);
            output.WriteValueS32(this.Unk5);
            output.WriteValueS32(this.Unk6);
        }
    }

    public class AsmEntry
    {
        public string Unk0;
        public byte Unk1;
        public short Unk2;
        public int Unk4;
        public int Unk6;
        public List<int> Unk7 = new List<int>();
        public List<AsmSubEntry> SubEntries = new List<AsmSubEntry>();

        public void Deserialize(Stream input)
        {
            this.Unk0 = input.ReadStringASCII(input.ReadValueU16());
            this.Unk1 = input.ReadValueU8();
            this.Unk2 = input.ReadValueS16();
            short subEntryCount = input.ReadValueS16();
            this.Unk4 = input.ReadValueS32();
            int unk7count = input.ReadValueS32();
            this.Unk6 = input.ReadValueS32();
            
            this.Unk7.Clear();
            for (int i = 0; i < unk7count; i++)
            {
                this.Unk7.Add(input.ReadValueS32());
            }

            this.SubEntries.Clear();
            for (short i = 0; i < subEntryCount; i++)
            {
                AsmSubEntry subEntry = new AsmSubEntry();
                subEntry.Deserialize(input);
                this.SubEntries.Add(subEntry);
            }
        }

        public void Serialize(Stream output)
        {
            output.WriteValueU16((ushort)this.Unk0.Length);
            output.WriteStringASCII(this.Unk0.Substring(0, (ushort)this.Unk0.Length));
            output.WriteValueU8(this.Unk1);
            output.WriteValueS16(this.Unk2);
            output.WriteValueS16((short)this.SubEntries.Count);
            output.WriteValueS32(this.Unk4);
            output.WriteValueS32(this.Unk7.Count);
            output.WriteValueS32(this.Unk6);

            for (int i = 0; i < this.Unk7.Count; i++)
            {
                output.WriteValueS32(this.Unk7[i]);
            }

            for (short i = 0; i < (short)this.SubEntries.Count; i++)
            {
                this.SubEntries[i].Serialize(output);
            }
        }
    }

    public class AsmFile
    {
        public short Version;
        public List<AsmEntry> Entries = new List<AsmEntry>();

        public void Deserialize(Stream input)
        {
            if (input.ReadValueU32() != 0xBEEFFEED)
            {
                throw new FormatException("not an asm file");
            }

            this.Version = input.ReadValueS16();
            if (this.Version != 5)
            {
                throw new FormatException("unsupported asm version " + this.Version.ToString());
            }

            short count = input.ReadValueS16();

            this.Entries.Clear();
            for (short i = 0; i < count; i++)
            {
                AsmEntry entry = new AsmEntry();
                entry.Deserialize(input);
                this.Entries.Add(entry);
            }
        }

        public void Serialize(Stream output)
        {
            output.WriteValueU32(0xBEEFFEED);
            output.WriteValueS16(this.Version);
            
            output.WriteValueS16((short)this.Entries.Count);
            for (short i = 0; i < (short)this.Entries.Count; i++)
            {
                this.Entries[i].Serialize(output);
            }
        }
    }
}
