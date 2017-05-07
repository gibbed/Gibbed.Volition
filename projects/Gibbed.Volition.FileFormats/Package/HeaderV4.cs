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

namespace Gibbed.Volition.FileFormats.Package
{
    internal class HeaderV4
    {
        public string Name;
        public string Path;
        public HeaderFlagsV4 Flags;
        public uint Unknown150;
        public uint DirectoryCount;
        public uint PackageSize;
        public uint DirectorySize;
        public uint NamesSize;
        public uint ExtensionsSize;
        public uint UncompressedSize;
        public uint CompressedSize;
        public uint DirectoryPointer;
        public uint NamesPointer;
        public uint ExtensionsPointer;
        public uint DataPointer;

        public void Serialize(Stream output, Endian endian)
        {
            output.WriteString(this.Name, 65, Encoding.ASCII);
            output.WriteString(this.Path, 256, Encoding.ASCII);
            output.Seek(3, SeekOrigin.Current);
            output.WriteValueEnum<HeaderFlagsV4>(this.Flags, endian);
            output.WriteValueU32(this.Unknown150, endian);
            output.WriteValueU32(this.DirectoryCount, endian);
            output.WriteValueU32(this.PackageSize, endian);
            output.WriteValueU32(this.DirectorySize, endian);
            output.WriteValueU32(this.NamesSize, endian);
            output.WriteValueU32(this.ExtensionsSize, endian);
            output.WriteValueU32(this.UncompressedSize, endian);
            output.WriteValueU32(this.CompressedSize, endian);
            output.WriteValueU32(this.DirectoryPointer, endian);
            output.WriteValueU32(this.NamesPointer, endian);
            output.WriteValueU32(this.ExtensionsPointer, endian);
            output.WriteValueU32(this.DataPointer, endian);
        }

        public void Deserialize(Stream input, Endian endian)
        {
            this.Name = input.ReadString(65, true, Encoding.ASCII);
            this.Path = input.ReadString(256, true, Encoding.ASCII);
            input.Seek(3, SeekOrigin.Current);
            this.Flags = input.ReadValueEnum<HeaderFlagsV4>(endian);
            this.Unknown150 = input.ReadValueU32(endian);
            this.DirectoryCount = input.ReadValueU32(endian);
            this.PackageSize = input.ReadValueU32(endian);
            this.DirectorySize = input.ReadValueU32(endian);
            this.NamesSize = input.ReadValueU32(endian);
            this.ExtensionsSize = input.ReadValueU32(endian);
            this.UncompressedSize = input.ReadValueU32(endian);
            this.CompressedSize = input.ReadValueU32(endian);
            this.DirectoryPointer = input.ReadValueU32(endian);
            this.NamesPointer = input.ReadValueU32(endian);
            this.ExtensionsPointer = input.ReadValueU32(endian);
            this.DataPointer = input.ReadValueU32(endian);
        }
    }
}
