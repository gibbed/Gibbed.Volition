using System.Runtime.InteropServices;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats.Packages.Structures
{
    [StructLayout(LayoutKind.Sequential, Size = 28)]
    internal struct PackageIndex4
    {
        public int NameOffset;
        public int ExtensionOffset;
        public uint Unknown08;
        public int Offset;
        public int UncompressedSize;
        public int CompressedSize;
        public uint Unknown1C;

        public PackageIndex4 Swap()
        {
            PackageIndex4 swapped = new PackageIndex4();
            swapped.NameOffset = this.NameOffset.Swap();
            swapped.ExtensionOffset = this.ExtensionOffset.Swap();
            swapped.Unknown08 = this.Unknown08.Swap();
            swapped.Offset = this.Offset.Swap();
            swapped.UncompressedSize = this.UncompressedSize.Swap();
            swapped.CompressedSize = this.CompressedSize.Swap();
            swapped.Unknown1C = this.Unknown1C.Swap();
            return swapped;
        }
    }
}
