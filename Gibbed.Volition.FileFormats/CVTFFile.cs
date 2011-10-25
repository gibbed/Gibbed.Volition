/* Copyright (c) 2011 Rick (rick 'at' gibbed 'dot' us)
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
using Gibbed.IO;

namespace Gibbed.Volition.FileFormats
{
    public class CVTFFile
    {
        public void Deserialize(Stream input, bool littleEndian)
        {
            var unknown1_count = input.ReadValueU32(littleEndian);
            var unknown1_offset = input.ReadValueU32(littleEndian);
            var unknown2_offset = input.ReadValueU32(littleEndian);
            var unknown2_count = input.ReadValueU32(littleEndian);
            var unknown3_offset = input.ReadValueU32(littleEndian);
            var unknown3_count = input.ReadValueU32(littleEndian);
            var unknown4_offset = input.ReadValueU32(littleEndian);
            var unknown4_count = input.ReadValueU32(littleEndian);
            var unknown5_offset = input.ReadValueU32(littleEndian);
            var unknown5_count = input.ReadValueU32(littleEndian);

            if (unknown1_count > 0)
            {
                if (unknown1_offset == 0xFFFFFFFF ||
                    unknown1_offset > input.Length)
                {
                    throw new FormatException("bad offset");
                }
                input.Seek(unknown1_offset, SeekOrigin.Begin);

                for (uint i = 0; i < unknown1_count; i++)
                {
                    // 16 bytes each
                }
            }

            var unknown4 = new List<string>();
            if (unknown4_count > 0)
            {
                if (unknown4_offset == 0xFFFFFFFF ||
                    unknown4_offset > input.Length)
                {
                    throw new FormatException("bad offset");
                }
                input.Seek(unknown4_offset, SeekOrigin.Begin);

                var offsets = new uint[unknown4_count];
                for (int i = 0; i < offsets.Length; i++)
                {
                    offsets[i] = input.ReadValueU32(littleEndian);
                }

                for (int i = 0; i < offsets.Length; i++)
                {
                    input.Seek(offsets[i], SeekOrigin.Begin);
                    unknown4.Add(input.ReadStringZ());
                }
            }

            var unknown5 = new List<string>();
            if (unknown5_count > 0)
            {
                if (unknown5_offset == 0xFFFFFFFF ||
                    unknown5_offset > input.Length)
                {
                    throw new FormatException("bad offset");
                }
                input.Seek(unknown5_offset, SeekOrigin.Begin);

                var offsets = new uint[unknown5_count];
                for (int i = 0; i < offsets.Length; i++)
                {
                    offsets[i] = input.ReadValueU32(littleEndian);
                }

                for (int i = 0; i < offsets.Length; i++)
                {
                    input.Seek(offsets[i], SeekOrigin.Begin);
                    unknown5.Add(input.ReadStringZ());
                }
            }

            if (unknown3_count > 0)
            {
                if (unknown3_offset == 0xFFFFFFFF ||
                    unknown3_offset > input.Length)
                {
                    throw new FormatException("bad offset");
                }
                input.Seek(unknown3_offset, SeekOrigin.Begin);

                for (uint i = 0; i < unknown3_count; i++)
                {
                    // 44 bytes each
                }
            }
        }
    }
}
