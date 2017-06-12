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

using System;
using System.IO;
using Gibbed.IO;
using Newtonsoft.Json;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    public class NavPointArrayElement : IElement
    {
        public void Read(Stream input, uint version, Endian endian)
        {
            var count = input.ReadValueU32(endian);

            for (int i = 0; i < count; i++)
            {
                var instance = new NavPointElement();
                instance.Read(input, version, endian);
            }

            for (int i = 0; i < count; i++)
            {
                var unknown11 = input.ReadValueU8();
                for (uint j = 0; j < unknown11; j++)
                {
                    var unknown12 = input.ReadValueU32(endian);
                }
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }

        public void ImportJson(JsonReader reader)
        {
            throw new NotImplementedException();
        }

        public void ExportJson(JsonWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
