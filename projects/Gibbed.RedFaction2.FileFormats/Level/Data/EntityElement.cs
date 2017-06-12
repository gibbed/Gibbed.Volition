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
    public class EntityElement : ISerializableElement
    {
        public class ArrayElement : SerializableArrayElement<EntityElement>
        {
        }

        public void Read(Stream input, uint version, Endian endian)
        {
            var unknown0 = input.ReadValueU32(endian);
            var unknown1 = input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
            var unknown2 = Vector3.Read(input, endian);
            var unknown3 = Transform.Read(input, endian);
            var unknown4 = input.ReadStringU16(32, Encoding.ASCII, endian);
            var unknown5 = input.ReadValueB8();
            var unknown6 = input.ReadValueU32(endian);
            var unknown7 = input.ReadValueU32(endian);
            var unknown8 = version >= 274 ? input.ReadValueU32(endian) : 0;
            var unknown9 = input.ReadValueU32(endian);
            var unknown10 = input.ReadStringU16(32, Encoding.ASCII, endian);
            var unknown11 = input.ReadStringU16(32, Encoding.ASCII, endian);

            bool unknown63 = false;

            if (version >= 248)
            {
                var unknown12 = input.ReadValueU32(endian);
                var unknown13 = version >= 263 ? input.ReadValueU32(endian) : 0;
                unknown63 = (unknown12 & (1u << 17)) != 0;
            }
            else
            {
                var unknown14 = input.ReadValueB8();
                var unknown15 = input.ReadValueB8();
                var unknown16 = input.ReadValueB8();
                var unknown17 = input.ReadValueB8();
                var unknown18 = input.ReadValueB8();
                var unknown19 = input.ReadValueB8();
            }

            var unknown20 = input.ReadValueU32(endian);
            var unknown21 = input.ReadValueU32(endian);

            if (version < 248)
            {
                var unknown22 = input.ReadValueB8();
                var unknown23 = input.ReadValueB8();
                var unknown24 = input.ReadValueB8();
            }

            var unknown25 = input.ReadValueF32(endian);
            var unknown26 = input.ReadValueF32(endian);
            var unknown27 = version >= 242 ? input.ReadValueF32(endian) : 100.0f;
            var unknown28 = version >= 242 ? input.ReadValueF32(endian) : 100.0f;
            var unknown29 = input.ReadValueU32(endian);
            var unknown30 = input.ReadStringU16(32, Encoding.ASCII, endian);
            var unknown31 = input.ReadStringU16(32, Encoding.ASCII, endian);
            var unknown32 = input.ReadStringU16(32, Encoding.ASCII, endian);
            var unknown33 = input.ReadStringU16(32, Encoding.ASCII, endian);
            var unknown34 = input.ReadStringU16(32, Encoding.ASCII, endian);
            var unknown35 = input.ReadStringU16(32, Encoding.ASCII, endian);
            var unknown36 = input.ReadStringU16(32, Encoding.ASCII, endian);
            var unknown37 = version >= 254 ? input.ReadStringU16(32, Encoding.ASCII, endian) : "";
            var unknown38 = input.ReadValueU8();
            var unknown39 = input.ReadValueU8();

            if (version < 232)
            {
                var unknown40 = input.ReadValueU32(endian);
                for (uint i = 0; i < unknown40; i++)
                {
                    var unknown41 = input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
                    var unknown42 = input.ReadValueU32(endian);
                }
            }

            var unknown43 = input.ReadValueU32(endian);
            var unknown44 = input.ReadValueU32(endian);
            var unknown45 = input.ReadValueU32(endian);

            if (version < 248)
            {
                var unknown46 = input.ReadValueB8();
                var unknown47 = input.ReadValueB8();
                var unknown48 = input.ReadValueB8();
                var unknown49 = input.ReadValueB8();
                var unknown50 = input.ReadValueB8();
                var unknown51 = input.ReadValueB8();
                var unknown52 = input.ReadValueB8();
                var unknown53 = input.ReadValueB8();
                var unknown54 = input.ReadValueB8();
                var unknown55 = input.ReadValueB8();
                var unknown56 = input.ReadValueB8();
                var unknown57 = input.ReadValueB8();
                var unknown58 = input.ReadValueB8();
                var unknown59 = input.ReadValueB8();
                var unknown60 = version >= 246 ? input.ReadValueB8() : false;
                var unknown61 = input.ReadValueB8();
                var unknown62 = input.ReadValueB8();
                unknown63 = input.ReadValueB8();
            }

            var unknown64 = 15.0f;
            var unknown65 = 0.0f;

            if (unknown63 == true)
            {
                unknown64 = input.ReadValueF32(endian);
                unknown65 = version >= 214 ? input.ReadValueF32(endian) : 0.0f;
            }

            if (version < 254)
            {
                var unknown66 = input.ReadStringU16(128, Encoding.ASCII, endian);
                var unknown67 = input.ReadStringU16(128, Encoding.ASCII, endian);
            }

            var unknown68 = version >= 218 ? input.ReadValueS32(endian) : -1;
            var unknown69 = version >= 218 ? input.ReadValueU8() : 0;
            var unknown70 = version >= 253 ? input.ReadValueF32(endian) : 10.0f;
            var unknown71 = version >= 255 ? input.ReadValueF32(endian) : 1.0f;
            var unknown72 = version >= 281 ? input.ReadValueF32(endian) : 1.0f;

            if (version < 248)
            {
                var unknown73 = version >= 241 ? input.ReadValueB8() : false;
            }

            if (input.Position == 7361)
            {
            }

            if (version >= 239)
            {
                var unknown74 = version >= 267 ? 19 : 18;
                for (uint i = 0; i < unknown74; i++)
                {
                    var unknown75 = input.ReadStringU16(128, Encoding.ASCII, endian);
                    var unknown76 = input.ReadStringU16(23, Encoding.ASCII, endian);
                }
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }
    }
}
