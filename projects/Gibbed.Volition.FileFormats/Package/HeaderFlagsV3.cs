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

namespace Gibbed.Volition.FileFormats.Package
{
    [Flags]
    public enum HeaderFlagsV3 : uint
    {
        None = 0u,
        Compressed = 1u << 0,
        Condensed = 1u << 1,
        Unknown2 = 1u << 2,
        Unknown3 = 1u << 3,
        Unknown4 = 1u << 4,
        Unknown5 = 1u << 5,
        Unknown6 = 1u << 6,
        Unknown7 = 1u << 7,
        Unknown8 = 1u << 8,
        Unknown9 = 1u << 9,
        Unknown10 = 1u << 10,
        Unknown11 = 1u << 11,
        Unknown12 = 1u << 12,
        Unknown13 = 1u << 13,
        Unknown14 = 1u << 14,
        Unknown15 = 1u << 15,
        Unknown16 = 1u << 16,
        Unknown17 = 1u << 17,
        Unknown18 = 1u << 18,
        Unknown19 = 1u << 19,
        Unknown20 = 1u << 20,
        Unknown21 = 1u << 21,
        Unknown22 = 1u << 22,
        Unknown23 = 1u << 23,
        Unknown24 = 1u << 24,
        Unknown25 = 1u << 25,
        Unknown26 = 1u << 26,
        Unknown27 = 1u << 27,
        Unknown28 = 1u << 28,
        Unknown29 = 1u << 29,
        Unknown30 = 1u << 30,
        Unknown31 = 1u << 31,
    }
}
