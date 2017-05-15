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

using System.IO;
using System.Text;
using Gibbed.IO;

namespace Gibbed.Volition.FileFormats
{
    public static class StreamHelpers
    {
        public static string ReadStringU16(this Stream input, ushort maximumLength, Encoding encoding, Endian endian)
        {
            var length = input.ReadValueU16(endian);

            if (length >= maximumLength)
            {
                length = maximumLength;
                length--;
            }

            return input.ReadString(length, true, encoding);
        }

        public static void WriteStringU16(this Stream output, string value, ushort maximumLength, Encoding encoding, Endian endian)
        {
            var length = value == null ? 0 : value.Length;

            if (length >= maximumLength)
            {
                length = maximumLength;
                maximumLength--;
            }

            output.WriteValueU16((ushort)length, endian);

            if (length > 0)
            {
                if (length == value.Length)
                {
                    output.WriteString(value, encoding);                    
                }
                else
                {
                    output.WriteString(value.Substring(0, length), encoding);
                }
            }
        }
    }
}
