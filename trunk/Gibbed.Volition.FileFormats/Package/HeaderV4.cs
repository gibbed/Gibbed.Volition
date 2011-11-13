using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gibbed.IO;
using System.IO;

namespace Gibbed.Volition.FileFormats.Package
{
    internal class HeaderV4
    {
        public string Name;
        public string Path;
        public HeaderFlags Flags;
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
            output.WriteValueEnum<HeaderFlags>(this.Flags, endian);
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
            this.Flags = input.ReadValueEnum<HeaderFlags>(endian);
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
