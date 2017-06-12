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
    public class DoorElement : ISerializableElement
    {
        public class ArrayElement : SerializableArrayElement<DoorElement>
        {
        }

        public void Read(Stream input, uint version, Endian endian)
        {
            var unknown0 = input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
            var unknown1 = input.ReadValueB8();
            var unknown2 = version >= 207 ? input.ReadValueS32(endian) : -1;
            var unknown3 = input.ReadValueB8();

            var unknown4 = input.ReadValueU32(endian);
            for (uint i = 0; i < unknown4; i++)
            {
                var unknown5 = input.ReadValueU32(endian);
                var unknown6 = Vector3.Read(input, endian);
                var unknown7 = Transform.Read(input, endian);
                var unknown8 = input.ReadStringU16(128, Encoding.ASCII, endian);
                var unknown9 = input.ReadValueB8();
                var unknown10 = input.ReadValueF32(endian);
                var unknown11 = input.ReadValueF32(endian);
                var unknown12 = input.ReadValueF32(endian);
                var unknown13 = input.ReadValueF32(endian);
                var unknown14 = input.ReadValueF32(endian);
                var unknown15 = input.ReadValueU32(endian);
                input.Seek(4 + 4, SeekOrigin.Current); // uint, uint
                var unknown16 = input.ReadValueF32(endian); // -(value * 0.017453292f)
            }

            var unknown17 = input.ReadValueU32(endian);
            for (uint i = 0; i < unknown17; i++)
            {
                var unknown18 = input.ReadValueU32(endian);
                var unknown19 = Vector3.Read(input, endian);
                var unknown20 = Transform.Read(input, endian);
            }

            var unknown21 = input.ReadValueB8();
            var unknown22 = input.ReadValueB8();
            var unknown23 = version >= 205 ? input.ReadValueB8() : false;
            var unknown24 = input.ReadValueB8();
            var unknown25 = input.ReadValueB8();
            var unknown26 = input.ReadValueB8();
            var unknown27 = input.ReadValueB8();
            var unknown28 = input.ReadValueU32(endian);
            var unknown29 = input.ReadValueU32(endian);

            for (uint i = 0; i < 4; i++)
            {
                var unknown30 = input.ReadStringU16(64, Encoding.ASCII, endian);
                var unknown31 = input.ReadValueF32(endian);
            }

            var unknown32 = version >= 211 ? input.ReadValueS32(endian) : -1;
            
            var unknown33 = input.ReadValueU32(endian);
            for (uint i = 0; i < unknown33; i++)
            {
                input.Seek(4, SeekOrigin.Current); // uint
            }

            var unknown34 = input.ReadValueU32(endian);
            for (uint i = 0; i < unknown34; i++)
            {
                input.Seek(4, SeekOrigin.Current); // uint
            }

            var unknown35 = input.ReadValueS32(endian);
            if (unknown35 != -1)
            {
                var unknown36 = Vector3.Read(input, endian);
                var unknown37 = Transform.Read(input, endian);
            }

            var unknown38 = version >= 207 ? input.ReadValueU32(endian) : 0;

            for (uint i = 0; i < 8; i++)
            {
                var unknown39 = version >= 207 ? input.ReadValueS32(endian) : -1;
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }
    }
}
