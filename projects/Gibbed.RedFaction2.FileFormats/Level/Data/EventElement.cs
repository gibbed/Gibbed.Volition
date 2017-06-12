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
    public class EventElement : ISerializableElement
    {
        public class ArrayElement : SerializableArrayElement<EventElement>
        {
        }

        public void Read(Stream input, uint version, Endian endian)
        {
            var uid = input.ReadValueU32(endian);
            var unknown1 = input.ReadStringU16(32, Encoding.ASCII, endian);
            var unknown2 = Vector3.Read(input, endian);
            var unknown3 = input.ReadStringU16(128, Encoding.ASCII, endian);
            var unknown4 = input.ReadValueB8();
            var unknown5 = input.ReadValueF32(endian);
            var unknown6 = version >= 210 ? input.ReadValueS32(endian) : -1;
            var unknown7 = version >= 268 ? input.ReadValueU32(endian) : 0;
            var unknown8 = input.ReadValueB8();
            var unknown9 = input.ReadValueB8();
            var unknown10 = input.ReadValueU32(endian);
            var unknown11 = input.ReadValueU32(endian);
            var unknown12 = version >= 282 ? input.ReadValueS32(endian) : -1;
            var unknown13 = version >= 283 ? input.ReadValueS32(endian) : -1;
            var unknown14 = input.ReadValueF32(endian);
            var unknown15 = input.ReadValueF32(endian);
            var unknown16 = version >= 237 ? input.ReadValueF32(endian) : 0.0f;
            var unknown17 = input.ReadStringU16(64, Encoding.ASCII, endian);
            var unknown18 = input.ReadStringU16(64, Encoding.ASCII, endian);
            var unknown19 = version >= 237 ? input.ReadStringU16(64, Encoding.ASCII, endian) : "";

            var linkCount = input.ReadValueU32(endian);
            if (linkCount > 8)
            {
                throw new FormatException("too many links from event");
            }

            for (uint j = 0; j < linkCount; j++)
            {
                var unknown20 = input.ReadValueU32(endian);
            }

            if (unknown1 == "Teleport" || unknown1 == "Alarm" || unknown1 == "Play_Explosion")
            {
                var unknown21 = Transform.Read(input, endian);
            }

            var unknown22 = Color.Read(input);
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }
    }
}
