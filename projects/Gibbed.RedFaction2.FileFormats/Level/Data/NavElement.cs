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
    public class NavElement : ISerializableElement
    {
        private static void SetFlag(ref uint flags, bool check, int flag)
        {
            if (check == false)
            {
                flags &= ~(1u << flag);
            }
            else
            {
                flags |= 1u << flag;
            }
        }

        public void Read(Stream input, uint version, Endian endian)
        {
            var unknown0 = input.ReadValueU32(endian);
            var unknown1 = input.ReadValueB8();
            var unknown2 = input.ReadValueF32(endian);
            var unknown3 = Vector3.Read(input, endian);
            var unknown4 = input.ReadValueF32(endian);

            uint unknown5 = 0;

            if (version < 213)
            {
                SetFlag(ref unknown5, input.ReadValueB32(endian), 4);
                SetFlag(ref unknown5, input.ReadValueB8(), 3);
            }
            else
            {
                unknown5 = input.ReadValueU32(endian);

                if (version < 249)
                {
                    unknown5 |= 0xE000;
                }
            }

            if ((unknown5 & 8) != 0)
            {
                var unknown6 = Transform.Read(input, endian);
                var unknown7 = version >= 214 ? input.ReadValueF32(endian) : 90.0f;
                // cos(value * 0.5 * 0.017453292)
            }

            if (version < 213)
            {
                SetFlag(ref unknown5, input.ReadValueB8(), 5);
                SetFlag(ref unknown5, input.ReadValueB8(), 6);
            }

            var unknown8 = input.ReadValueF32(endian);
            var unknown9 = version >= 277 ? input.ReadValueS32(endian) : -1;

            if (version >= 225 && version <= 232)
            {
                input.Seek(4, SeekOrigin.Current); // uint
            }

            var linkCount = input.ReadValueU32(endian);
            if (linkCount > 4)
            {
                // "too many links (max %d) from nav uid: %d"
                throw new FormatException();
            }

            for (uint j = 0; j < linkCount; j++)
            {
                var unknown10 = input.ReadValueU32(endian);
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }
    }
}
