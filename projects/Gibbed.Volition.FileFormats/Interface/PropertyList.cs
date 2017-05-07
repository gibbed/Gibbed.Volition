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
using System.Collections.Generic;
using System.IO;
using Gibbed.IO;

namespace Gibbed.Volition.FileFormats.Interface
{
    public class PropertyList
    {
        public Dictionary<uint, IProperty> Properties
            = new Dictionary<uint, IProperty>();

        public void Serialize(Stream output, Endian endian, StringTable strings)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, Endian endian, StringTable strings)
        {
            while (true)
            {
                var type = input.ReadValueEnum<PropertyType>(endian);
                if (type == PropertyType.Invalid)
                {
                    break;
                }

                var hash = input.ReadValueU32(endian);
                var property = Object.CreateProperty(type);
                property.Deserialize(input, endian, strings);
                this.Properties.Add(hash, property);
            }
        }
    }
}
