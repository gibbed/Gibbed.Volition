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

using System.Runtime.InteropServices;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats.Packages.Structures
{
    [StructLayout(LayoutKind.Explicit, Size = 384)]
    internal struct PackageHeader6
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

        [FieldOffset(0x16C)]
        public int Unknown16C;

        public PackageHeader6 Swap()
        {
            var swapped = new PackageHeader6();
            swapped.Magic = this.Magic.Swap();
            swapped.Version = this.Version.Swap();
            swapped.Flags = (PackageFlags)(((uint)(this.Flags)).Swap());
            swapped.IndexCount = this.IndexCount.Swap();
            swapped.PackageSize = this.PackageSize.Swap();
            swapped.IndexSize = this.IndexSize.Swap();
            swapped.NamesSize = this.NamesSize.Swap();
            swapped.UncompressedDataSize = this.UncompressedDataSize.Swap();
            swapped.CompressedDataSize = this.CompressedDataSize.Swap();
            swapped.Unknown16C = this.Unknown16C.Swap();
            return swapped;
        }
    }
}
