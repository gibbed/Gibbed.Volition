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

namespace Gibbed.Volition.FileFormats.Interface
{
    public enum PropertyType : byte
    {
        Invalid = 0,
        Int = 1,
        UInt = 2,
        Float = 3,
        String = 4,
        Bool = 5,
        Color = 6,
        Vector2F = 7,
        Callback = 8,
        Bitmap = 9,
        Font = 10,
        Sound = 11,
        Enum = 12,
        Variable = 13,
    }
}
