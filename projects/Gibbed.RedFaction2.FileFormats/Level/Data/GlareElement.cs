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
    public class GlareElement : ISerializableElement
    {
        public class ArrayElement : SerializableArrayElement<GlareElement>
        {
        }

        public void Read(Stream input, uint version, Endian endian)
        {
            var unknown0 = input.ReadValueU32(endian);
            var unknown1 = input.ReadStringU16(128, Encoding.ASCII, endian);
            var unknown2 = Vector3.Read(input, endian);
            var unknown3 = Transform.Read(input, endian);
            var unknown4 = input.ReadStringU16(128, Encoding.ASCII, endian);
            var unknown5 = input.ReadValueB8();
            var unknown6 = Color.Read(input);
            var unknown7 = version >= 259 ? input.ReadValueB8() : false;
            var unknown8 = version >= 288 ? input.ReadValueB8() : false;
            var unknown9 = version >= 289 ? input.ReadValueB8() : false;
            var unknown10 = version >= 292 ? input.ReadValueB8() : false;

            if (version < 232)
            {
                input.Seek(4, SeekOrigin.Current); // uint
            }

            var unknown11 = version >= 232 ? input.ReadValueB8() : false;
            
            var unknown12 = input.ReadStringU16(23, Encoding.ASCII, endian);
            if (string.IsNullOrEmpty(unknown12) == false)
            {
                var unknown13 = input.ReadValueF32(endian);
                var unknown14 = input.ReadValueF32(endian);
                var unknown15 = input.ReadValueF32(endian);
                var unknown16 = input.ReadValueF32(endian);
                var unknown17 = input.ReadValueF32(endian);
            }

            var unknown18 = input.ReadStringU16(23, Encoding.ASCII, endian);
            if (string.IsNullOrEmpty(unknown18) == false)
            {
                var unknown19 = input.ReadValueF32(endian);
                var unknown20 = input.ReadValueF32(endian);
                var unknown21 = version >= 273 ? input.ReadValueF32(endian) : 1.0f;
            }

            var unknown22 = version >= 276 ? input.ReadValueB8() : false;
            
            var unknown23 = input.ReadValueU32(endian);
            for (uint i = 0; i < unknown23; i++)
            {
                var unknown24 = input.ReadValueU32(endian);
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }
    }
}
