using System.Runtime.InteropServices;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats.Packages.Structures
{
    [StructLayout(LayoutKind.Sequential, Size = 28)]
    internal struct PackageIndex3
    {
        public int NameOffset;
        public uint Unknown04;
        public int Offset;
        public uint Unknown0C;
        public int UncompressedSize;
        public int CompressedSize;
        public uint Unknown1C;

        public PackageIndex3 Swap()
        {
            PackageIndex3 swapped = new PackageIndex3();
            swapped.NameOffset = this.NameOffset.Swap();
            swapped.Unknown04 = this.Unknown04.Swap();
            swapped.Offset = this.Offset.Swap();
            swapped.Unknown0C = this.Unknown0C.Swap();
            swapped.UncompressedSize = this.UncompressedSize.Swap();
            swapped.CompressedSize = this.CompressedSize.Swap();
            swapped.Unknown1C = this.Unknown1C.Swap();
            return swapped;
        }
    }
}
