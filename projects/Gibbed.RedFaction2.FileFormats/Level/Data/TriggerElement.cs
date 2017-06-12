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
    public class TriggerElement : ISerializableElement
    {
        public class ArrayElement : SerializableArrayElement<TriggerElement>
        {
        }

        public void Read(Stream input, uint version, Endian endian)
        {
            var unknown0 = input.ReadValueU32(endian);
            var unknown1 = input.ReadStringU16(128, Encoding.ASCII, endian);
            var unknown2 = input.ReadValueB8();
            var unknown3 = input.ReadValueU32(endian);
            var unknown4 = input.ReadValueF32(endian); // value * 1000.0f
            var unknown5 = input.ReadValueS32(endian);
            uint unknown6 = 0;

            if (version < 245 && input.ReadValueB8() == true)
            {
                unknown6 |= 0x001;
            }

            var unknown7 = input.ReadStringU16(32, Encoding.ASCII, endian);

            if (version < 245 && input.ReadValueB8() == true)
            {
                unknown6 |= 0x002;
            }

            var unknown8 = input.ReadValueU8();

            if (version < 245)
            {
                if (input.ReadValueB8() == true)
                {
                    unknown6 |= 0x004;
                }

                if (input.ReadValueB8() == true)
                {
                    unknown6 |= 0x008;
                }

                if (input.ReadValueB8() == true)
                {
                    unknown6 |= 0x100;
                }

                if (input.ReadValueB8() == true)
                {
                    unknown6 |= 0x080;
                }
            }

            if (unknown3 == 0)
            {
                var unknown9 = Vector3.Read(input, endian);
                var unknown10 = input.ReadValueF32(endian);
            }
            else if (unknown3 == 1)
            {
                var unknown9 = Vector3.Read(input, endian);
                var unknown11 = Transform.Read(input, endian);
                var unknown12 = input.ReadValueF32(endian);
                var unknown13 = input.ReadValueF32(endian);
                var unknown14 = input.ReadValueF32(endian);

                if (version < 245 && input.ReadValueB8() == true)
                {
                    unknown6 |= 0x020;
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            var unknown15 = input.ReadValueS32(endian);
            var unknown16 = input.ReadValueS32(endian);
            var unknown17 = input.ReadValueS32(endian);

            if (version < 245 && input.ReadValueB8() == true)
            {
                unknown6 |= 0x010;
            }

            var unknown18 = input.ReadValueF32(endian);
            var unknown19 = input.ReadValueF32(endian);

            if (version < 245 && input.ReadValueB8() == true)
            {
                unknown6 |= 0x400;
            }

            var unknown20 = input.ReadValueS32(endian);

            if (version < 245)
            {
                var unknown21 = input.ReadValueU8();

                if (version <= 218)
                {
                    unknown21 |= 0xE0;
                }

                unknown6 |= ((byte)(unknown21 & 0xE0) | 2 * (unknown21 & 7u)) << 11;
            }

            if (version >= 245)
            {
                unknown6 = input.ReadValueU32(endian);
            }

            var linkCount = input.ReadValueU32(endian);
            if (linkCount >= 16)
            {
                throw new FormatException();
            }
            for (uint i = 0; i < linkCount; i++)
            {
                var unknown21 = input.ReadValueU32(endian);
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }
    }
}
