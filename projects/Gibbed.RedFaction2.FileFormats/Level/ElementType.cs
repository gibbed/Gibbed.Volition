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

namespace Gibbed.RedFaction2.FileFormats.Level
{
    public enum ElementType : uint
    {
        None = 0,
        Unknown200 = 0x200,
        Unknown300 = 0x300,
        Cameras = 0x400,
        AmbientSounds = 0x500,
        Events = 0x600,
        SpawnPoints = 0x700,
        Unknown800 = 0x800,
        UnknownC00 = 0xC00,
        ClimbingRegions = 0xD00,
        Emitters = 0xE00,
        UnknownF00 = 0xF00, // not found in any retail RF2 levels
        Decals = 0x1000,
        PushRegions = 0x1100,
        Unknown2000 = 0x2000,
        Doors = 0x3000,
        Unknown4000 = 0x4000,
        Unknown6000 = 0x6000, // not found in any retail RF2 levels
        Mirrors = 0x7677,
        Glares = 0x7678,
        Unknown7679 = 0x7679, // not found in any retail RF2 levels
        Unknown7680 = 0x7680,
        Unknown7681 = 0x7681,
        Unknown7682 = 0x7682,
        Unknown7777 = 0x7777,
        Unknown7778 = 0x7778,
        SplinePaths = 0x7779,
        Unknown7900 = 0x7900,
        Unknown7901 = 0x7901,
        Unknown10000 = 0x10000,
        Navs = 0x20000,
        Entities = 0x30000,
        Items = 0x40000,
        Clutters = 0x50000,
        Triggers = 0x60000,
        Unknown70000 = 0x70000,
        Unknown80000 = 0x80000, // not found in any retail RF2 levels
        Unknown1000000 = 0x1000000, // string array
        Unknown1000001 = 0x1000001,
        Unknown2000000 = 0x2000000,
        Unknown3000000 = 0x3000000,
    }
}
