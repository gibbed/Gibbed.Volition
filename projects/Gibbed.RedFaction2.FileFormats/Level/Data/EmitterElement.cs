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
    public class EmitterElement : ISerializableElement
    {
        public class ArrayElement : SerializableArrayElement<EmitterElement>
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
            var unknown7 = input.ReadValueF32(endian);
            var unknown8 = input.ReadValueF32(endian);
            var unknown9 = input.ReadValueF32(endian);
            var unknown10 = input.ReadValueF32(endian);
            var unknown11 = input.ReadValueU32(endian);
            var unknown12 = input.ReadValueF32(endian);
            var unknown13 = input.ReadValueF32(endian);
            var unknown14 = input.ReadValueF32(endian);
            var unknown15 = input.ReadValueF32(endian);
            var unknown16 = Color.Read(input);
            var unknown17 = input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
            var unknown18 = input.ReadValueU32(endian);
            var unknown19 = input.ReadValueB8();
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }
    }
}
