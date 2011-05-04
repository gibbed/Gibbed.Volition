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
    [StructLayout(LayoutKind.Sequential, Size = 24)]
    internal struct PackageIndex6
    {
        public int NameOffset;
        public uint Unknown04;
        public int Offset;
        public int UncompressedSize;
        public int CompressedSize;
        public uint Unknown18;

        public PackageIndex6 Swap()
        {
            var swapped = new PackageIndex6();
            swapped.NameOffset = this.NameOffset.Swap();
            swapped.Unknown04 = this.Unknown04.Swap();
            swapped.Offset = this.Offset.Swap();
            swapped.UncompressedSize = this.UncompressedSize.Swap();
            swapped.CompressedSize = this.CompressedSize.Swap();
            swapped.Unknown18 = this.Unknown18.Swap();
            return swapped;
        }
    }
}
