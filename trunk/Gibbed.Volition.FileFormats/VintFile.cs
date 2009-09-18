using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats
{
    public class VintFile
    {
        public UInt16 DocumentType;
        public float AnimationTime;
        public string Name;

        public List<string> CriticalResources = new List<string>();
        public Dictionary<string, string> Metadata = new Dictionary<string, string>();

        public List<string> Strings = new List<string>();

        public List<Vint.Object> Elements = new List<Vint.Object>();
        public List<Vint.Object> Animations = new List<Vint.Object>();

        private void ReadStrings(Stream stream, UInt32 offset)
        {
            long position = stream.Position;
            stream.Seek(offset, SeekOrigin.Begin);

            UInt32 count = stream.ReadValueU32();
            UInt32 bufferSize = stream.ReadValueU32();

            byte[] indexBuffer = new byte[count * 4];
            byte[] stringBuffer = new byte[bufferSize];

            stream.Read(indexBuffer, 0, indexBuffer.Length);
            stream.Read(stringBuffer, 0, stringBuffer.Length);

            stream.Seek(position, SeekOrigin.Begin);

            this.Strings.Clear();
            for (UInt32 i = 0; i < count; i++)
            {
                UInt32 stringOffset = BitConverter.ToUInt32(indexBuffer, (int)(i * 4));
                this.Strings.Add(stringBuffer.ToStringASCIIZ(stringOffset));
            }
        }

        public void Deserialize(Stream stream)
        {
            if (stream.ReadValueU32() != 0x3027)
            {
                throw new FormatException("not a volition interface document file");
            }

            int nameIndex = stream.ReadValueS32();

            this.DocumentType = stream.ReadValueU16();
            if (this.DocumentType != 1)
            {
                throw new FormatException("unsupported volition interface document type");
            }

            this.AnimationTime = stream.ReadValueF32();
            UInt32 metadataCount = stream.ReadValueU32();
            UInt32 criticalResourceCount = stream.ReadValueU32();
            this.ReadStrings(stream, stream.ReadValueU32());
            UInt16 elementCount = stream.ReadValueU16();
            UInt16 animationCount = stream.ReadValueU16();

            // not absolutely sure this is the name index... so...
            if (nameIndex != 0)
            {
                throw new Exception();
            }

            this.Name = this.Strings[nameIndex];

            for (int i = 0; i < criticalResourceCount; i++)
            {
                byte unk1 = stream.ReadValueU8();
                int criticalResourceIndex = stream.ReadValueS32();
                if (unk1 == 0)
                {
                    this.CriticalResources.Add(this.Strings[criticalResourceIndex]);
                }
            }

            this.Metadata.Clear();
            for (int i = 0; i < metadataCount; i++)
            {
                int key = stream.ReadValueS32();
                int value = stream.ReadValueS32();
                this.Metadata[this.Strings[key]] = this.Strings[value];
            }

            for (int i = 0; i < elementCount; i++)
            {
                Vint.Object element = new Vint.Object();
                element.Read(stream, this);
                this.Elements.Add(element);
            }

            for (int i = 0; i < animationCount; i++)
            {
                Vint.Object animation = new Vint.Object();
                animation.Read(stream, this);
                this.Animations.Add(animation);
            }
        }
    }
}
