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
using Gibbed.IO;

namespace Gibbed.Volition.FileFormats.Packages.Structures
{
    [StructLayout(
        LayoutKind.Sequential, Size = 384,
        CharSet = CharSet.Ansi,
        Pack = 1)]
    internal struct PackageHeader4
    {
        public uint Magic;                  // 000
        public uint Version;                // 004
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x41)]
        public string String1;              // 008
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)]
        public string String2;              // 049
        public byte Pad1;
        public byte Pad2;
        public byte Pad3;
        public PackageFlags Flags;          // 14C
        public uint Unknown150;             // 150
        public int IndexCount;              // 154
        public int PackageSize;             // 158
        public int IndexSize;               // 15C
        public int NamesSize;               // 160
        public int ExtensionsSize;          // 164
        public int UncompressedDataSize;    // 168
        public int CompressedDataSize;      // 16C
        public uint IndexPointer;           // 170
        public uint NamesPointer;           // 174
        public uint ExtensionsPointer;      // 178
        public uint DataPointer;            // 17C

        public PackageHeader4 Swap()
        {
            var swapped = new PackageHeader4();
            swapped.Magic = this.Magic.Swap();
            swapped.Version = this.Version.Swap();
            swapped.Flags = (PackageFlags)(((uint)(this.Flags)).Swap());
            swapped.IndexCount = this.IndexCount.Swap();
            swapped.PackageSize = this.PackageSize.Swap();
            swapped.IndexSize = this.IndexSize.Swap();
            swapped.NamesSize = this.NamesSize.Swap();
            swapped.ExtensionsSize = this.ExtensionsSize.Swap();
            swapped.UncompressedDataSize = this.UncompressedDataSize.Swap();
            swapped.CompressedDataSize = this.CompressedDataSize.Swap();
            return swapped;
        }
    }
}
