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
using System.Text;
using Gibbed.IO;
using Newtonsoft.Json;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    public class Unknown007900Element : IElement
    {
        public void Read(Stream input, uint version, Endian endian)
        {
            var dataVersion = input.ReadValueU32();
            if (dataVersion != 5)
            {
                // "Error:  Level is too new!  Make sure that you have the latest version of the game."
                throw new FormatException();
            }

            var unknown0 = input.ReadValueU32(endian);
            var unknown1 = input.ReadValueU32(endian);

            var unknown2 = new string[unknown0];
            for (uint i = 0; i < unknown0; i++)
            {
                unknown2[i] = input.ReadStringZ(Encoding.ASCII);
            }

            var unknown3 = input.ReadValueU32(endian);
            for (uint i = 0; i < unknown3; i++)
            {
                var unknown4 = input.ReadValueF32(endian);
                var unknown5 = input.ReadValueF32(endian);
            }

            for (uint i = 0; i < unknown1; i++)
            {
                var unknown6 = input.ReadValueU32(endian);
            }

            for (uint i = 0; i < unknown1; i++)
            {
                var unknown7 = input.ReadValueU32(endian);
                for (uint j = 0; j < unknown7; j++)
                {
                    var unknown8 = input.ReadValueU32(endian);
                    var unknown9 = input.ReadValueU32(endian);
                    var unknown10 = input.ReadValueU32(endian);
                    var unknown11 = input.ReadValueU32(endian);
                    var unknown12 = input.ReadValueU32(endian);

                    for (uint k = 0; k < unknown8; k++)
                    {
                        var unknown13 = Vector3.Read(input, endian);
                    }

                    var unknown14 = Vector3.Read(input, endian);
                    var unknown15 = Vector3.Read(input, endian);

                    for (uint k = 0; k < unknown9; k++)
                    {
                        var unknown16 = input.ReadValueU32(endian);
                        var unknown17 = input.ReadValueU32(endian);
                        var unknown18 = input.ReadValueU32(endian);
                        var unknown19 = input.ReadValueU32(endian);
                        var unknown20 = input.ReadValueU32(endian);
                        var unknown21 = input.ReadValueU32(endian);
                        var unknown22 = input.ReadValueU32(endian);
                        var unknown23 = input.ReadValueU32(endian);
                        var unknown24 = input.ReadValueU32(endian);
                        var unknown25 = input.ReadValueU32(endian);
                        var unknown26 = input.ReadValueU32(endian);
                        var unknown27 = input.ReadValueU32(endian);
                        var unknown28 = input.ReadValueF32(endian);
                        var unknown29 = input.ReadValueF32(endian);
                        var unknown30 = input.ReadValueF32(endian);
                        var unknown31 = input.ReadValueF32(endian);
                        var unknown32 = input.ReadValueF32(endian);
                        var unknown33 = input.ReadValueF32(endian);
                        var unknown34 = input.ReadValueU32(endian);
                        var unknown35 = Vector3.Read(input, endian);
                    }
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
