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

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    public class Unknown002000Element : ISerializableElement
    {
        public class ArrayElement : SerializableArrayElement<Unknown002000Element>
        {
        }

        public void Read(Stream input, uint version, Endian endian)
        {
            var unknown0 = input.ReadValueU32(endian);
            var unknown1 = Vector3.Read(input, endian);
            var unknown2 = Transform.Read(input, endian);
            var unknown3 = new ThingElement();
            unknown3.Read(input, version, endian);
            var unknown4 = input.ReadValueU32(endian);
            var unknown5 = input.ReadValueS32(endian);

            if (version < 227)
            {
                if (version >= 224)
                {
                    input.Seek(4, SeekOrigin.Current); // uint
                }

                if (version >= 226)
                {
                    input.Seek(4, SeekOrigin.Current); // uint
                }
            }

            var unknown6 = input.ReadValueU32(endian);

            if (version >= 235 && version <= 251)
            {
                input.Seek(4, SeekOrigin.Current); // uint
            }

            if ((unknown4 & 8) != 0)
            {
                input.Seek(4 * 4, SeekOrigin.Current); // uint, uint, uint, uint
                input.Seek(4 * 4, SeekOrigin.Current); // float, float, float, float
                input.Seek(4, SeekOrigin.Current); // color
                input.Seek(4, SeekOrigin.Current); // float
                input.Seek(1, SeekOrigin.Current); // bool
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }
    }
}
