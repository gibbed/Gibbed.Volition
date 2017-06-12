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
using System.Text;
using Gibbed.IO;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    public class PushRegionElement : ISerializableElement
    {
        public class ArrayElement : SerializableArrayElement<PushRegionElement>
        {
        }

        public void Read(Stream input, uint version, Endian endian)
        {
            var unknown0 = input.ReadValueU32(endian);
            var unknown1 = input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
            var unknown2 = Vector3.Read(input, endian);
            var unknown3 = Transform.Read(input, endian);
            var unknown4 = input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
            var unknown5 = input.ReadValueB8();
            var unknown6 = input.ReadValueU32(endian);

            if (unknown6 == 1)
            {
                var unknown7 = input.ReadValueF32(endian);
            }
            else if (unknown6 == 2)
            {
                var unknown7 = Vector3.Read(input, endian);
            }
            else if (unknown6 == 3)
            {
                var unknown7 = Vector3.Read(input, endian);
            }

            var unknown8 = input.ReadValueF32(endian);
            var unknown9 = input.ReadValueU32(endian);
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }
    }
}
