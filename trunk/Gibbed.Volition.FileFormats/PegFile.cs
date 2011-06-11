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
using System.Linq;
using System.Text;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats
{
    public class PegFile
    {
        public void Deserialize(Stream input)
        {
            var magic = input.ReadValueU32(true);
            if (magic != 0x564B4547 && // VKEG
                magic != 0x47454B56)
            {
                throw new FormatException("not a peg file");
            }

            var littleEndian = magic == 0x564B4547;

            var majorVersion = input.ReadValueU16(littleEndian);
            if (majorVersion != 10)
            {
                throw new FormatException("unsupported peg version");
            }

            var minorVersion = input.ReadValueU16(littleEndian);

            var headerSize = input.ReadValueU32(littleEndian);
            if (input.Position + headerSize > input.Length)
            {
                throw new EndOfStreamException();
            }

            var dataSize = input.ReadValueU32(littleEndian);

            throw new NotImplementedException();
        }
    }
}
