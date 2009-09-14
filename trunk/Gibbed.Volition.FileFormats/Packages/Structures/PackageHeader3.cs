using System.Runtime.InteropServices;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats.Packages.Structures
{
    [StructLayout(LayoutKind.Explicit, Size = 384)]
    internal struct PackageHeader3
    {
        [FieldOffset(0)]
        public uint Magic;

        [FieldOffset(4)]
        public uint Version;

        [FieldOffset(0x14C)]
        public PackageFlags Flags;

        [FieldOffset(0x154)]
        public int IndexCount;

        [FieldOffset(0x158)]
        public int PackageSize;

        [FieldOffset(0x15C)]
        public int IndexSize;

        [FieldOffset(0x160)]
        public int NamesSize;

        [FieldOffset(0x164)]
        public int UncompressedDataSize;

        [FieldOffset(0x168)]
        public int CompressedDataSize;

        public PackageHeader3 Swap()
        {
            PackageHeader3 swapped = new PackageHeader3();
            swapped.Magic = this.Magic.Swap();
            swapped.Version = this.Version.Swap();
            swapped.Flags = (PackageFlags)(((uint)(this.Flags)).Swap());
            swapped.IndexCount = this.IndexCount.Swap();
            swapped.PackageSize = this.PackageSize.Swap();
            swapped.IndexSize = this.IndexSize.Swap();
            swapped.NamesSize = this.NamesSize.Swap();
            swapped.UncompressedDataSize = this.UncompressedDataSize.Swap();
            swapped.CompressedDataSize = this.CompressedDataSize.Swap();
            return swapped;
        }
    }
}
