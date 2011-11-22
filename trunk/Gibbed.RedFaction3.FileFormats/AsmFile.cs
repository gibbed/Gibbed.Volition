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
        public Endian Endian;
        public ushort Version;
        public List<Asm.ContainerEntry> Containers = new List<Asm.ContainerEntry>();

        public void Deserialize(Stream input)
        {
            var magic = input.ReadValueU32();
            if (magic != 0xBEEFFEED)
            {
                throw new FormatException("not an asm file");
            }

            var endian = magic == 0xBEEFFEED ? Endian.Little : Endian.Big;

            this.Version = input.ReadValueU16(endian);
            if (this.Version != 11)
            {
                throw new FormatException("unsupported asm version " + this.Version.ToString());
            }

            var containerCount = input.ReadValueU16();

            this.Containers.Clear();
            for (ushort i = 0; i < containerCount; i++)
            {
                var container = new Asm.ContainerEntry();
                container.Deserialize(input, this.Version, endian);
                this.Containers.Add(container);
            }

            this.Endian = endian;
        }

        public void Serialize(Stream output)
        {
            var endian = this.Endian;

            output.WriteValueU32(0xBEEFFEED, endian);
            output.WriteValueU16(this.Version, endian);
            output.WriteValueU16((ushort)this.Containers.Count, endian);

            for (ushort i = 0; i < (ushort)this.Containers.Count; i++)
            {
                this.Containers[i].Serialize(output, this.Version, endian);
            }
        }
    }
}
