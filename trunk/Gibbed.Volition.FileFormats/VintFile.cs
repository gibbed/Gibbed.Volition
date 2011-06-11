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
using System.Collections.Generic;
using System.IO;
using System.Text;
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

            var count = stream.ReadValueU32();
            var bufferSize = stream.ReadValueU32();

            var indexBuffer = new byte[count * 4];

            stream.Read(indexBuffer, 0, indexBuffer.Length);
            var stringData = stream.ReadToMemoryStream(bufferSize);

            stream.Seek(position, SeekOrigin.Begin);

            this.Strings.Clear();
            for (UInt32 i = 0; i < count; i++)
            {
                var stringOffset = BitConverter.ToUInt32(indexBuffer, (int)(i * 4));
                stringData.Seek(stringOffset, SeekOrigin.Begin);
                this.Strings.Add(stringData.ReadStringZ(Encoding.ASCII));
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
            var metadataCount = stream.ReadValueU32();
            var criticalResourceCount = stream.ReadValueU32();
            this.ReadStrings(stream, stream.ReadValueU32());
            var elementCount = stream.ReadValueU16();
            var animationCount = stream.ReadValueU16();

            // not absolutely sure this is the name index... so...
            if (nameIndex != 0)
            {
                throw new FormatException("name index is not 0");
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
                element.Deserialize(stream, this);
                this.Elements.Add(element);
            }

            for (int i = 0; i < animationCount; i++)
            {
                Vint.Object animation = new Vint.Object();
                animation.Deserialize(stream, this);
                this.Animations.Add(animation);
            }
        }
    }
}
