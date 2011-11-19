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

namespace Gibbed.RedFaction3.FileFormats
{
    public class AsmFile
    {
        public short Version;
        public List<Asm.StreamEntry> Streams = new List<Asm.StreamEntry>();

        public void Deserialize(Stream input)
        {
            if (input.ReadValueU32() != 0xBEEFFEED)
            {
                throw new FormatException("not an asm file");
            }

            this.Version = input.ReadValueS16();
            if (this.Version != 5)
            {
                throw new FormatException("unsupported asm version " + this.Version.ToString());
            }

            short count = input.ReadValueS16();

            this.Streams.Clear();
            for (short i = 0; i < count; i++)
            {
                var stream = new Asm.StreamEntry();
                stream.Deserialize(input);
                this.Streams.Add(stream);
            }
        }

        public void Serialize(Stream output)
        {
            output.WriteValueU32(0xBEEFFEED);
            output.WriteValueS16(this.Version);

            output.WriteValueS16((short)this.Streams.Count);
            for (short i = 0; i < (short)this.Streams.Count; i++)
            {
                this.Streams[i].Serialize(output);
            }
        }
    }
}
