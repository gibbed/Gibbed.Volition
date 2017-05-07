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
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.IO;

namespace Gibbed.Volition.FileFormats.Interface
{
    public class StringTable
    {
        private List<string> Strings
            = new List<string>();

        public string ReadString(int index)
        {
            if (index < 0 || index >= this.Strings.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return this.Strings[index];
        }

        public string ReadString(Stream input, Endian endian)
        {
            var index = input.ReadValueS32(endian);
            if (index < 0 || index >= this.Strings.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return this.Strings[index];
        }

        public int WriteIndex(string value)
        {
            if (this.Strings.Contains(value) == false)
            {
                this.Strings.Add(value);
            }

            return this.Strings.IndexOf(value);
        }

        public void WriteIndex(Stream output, Endian endian, string value)
        {
            if (this.Strings.Contains(value) == false)
            {
                this.Strings.Add(value);
            }

            output.WriteValueS32(this.Strings.IndexOf(value), endian);
        }

        public void Serialize(Stream output, Endian endian)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, Endian endian)
        {
            var count = input.ReadValueU32(endian);
            var size = input.ReadValueU32(endian);

            var offsets = new uint[count];
            for (uint i = 0; i < count; i++)
            {
                offsets[i] = input.ReadValueU32(endian);
            }

            var encoding = Encoding.GetEncoding(1252);

            this.Strings.Clear();
            using (var data = input.ReadToMemoryStream(size))
            {
                for (uint i = 0; i < count; i++)
                {
                    data.Seek(offsets[i], SeekOrigin.Begin);
                    this.Strings.Add(data.ReadStringZ(encoding));
                }
            }
        }
    }
}
