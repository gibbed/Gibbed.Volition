﻿/* Copyright (c) 2011 Rick (rick 'at' gibbed 'dot' us)
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

namespace Gibbed.Volition.FileFormats.Peg
{
    [Flags]
    public enum TextureFlags : ushort
    {
        None = 0, // 0
        Unknown0 = 1 << 0, // 1
        Unknown1 = 1 << 1, // 2
        Unknown2 = 1 << 2, // 4
        CubeTexture = 1 << 3, // 8
        Unknown4 = 1 << 4, // 16
        Unknown5 = 1 << 5, // 32
        Unknown6 = 1 << 6, // 64
        Unknown7 = 1 << 7, // 128
        Unknown8 = 1 << 8, // 256
        Unknown9 = 1 << 9, // 512
        Unknown10 = 1 << 10, // 1024
        Unknown11 = 1 << 11, // 2048
        Unknown12 = 1 << 12, // 4096
        Unknown13 = 1 << 13, // 8192
        Unknown14 = 1 << 14, // 16384
        Unknown15 = 1 << 15, // 32768
    }
}
