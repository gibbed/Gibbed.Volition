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
    public enum MetadataType : uint
    {
        None = 0,
        Data = 0x100,
        Settings = 0x900,
        Unknown1300 = 0x1300, // string array, not found in any retail RF2 levels
        RequiredSpawnTextures = 0x7000,
        RequiredClothModels = 0x7001,
        RequiredAnimations = 0x7002,
        RequiredModels = 0x7003,
        RequiredEffects = 0x7004,
        RequiredTextures = 0x7005,
    }
}
