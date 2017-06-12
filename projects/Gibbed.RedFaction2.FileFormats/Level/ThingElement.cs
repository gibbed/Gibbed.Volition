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

namespace Gibbed.RedFaction2.FileFormats.Level
{
    public class ThingElement : ISerializableElement
    {
        // TODO(gibbed): fields
        private string _Unknown1;

        public void Read(Stream input, uint version, Endian endian)
        {
            if (version == 200)
            {
                input.Seek(4, SeekOrigin.Current); // uint
            }

            this._Unknown1 = input.ReadStringU16(63, Encoding.ASCII, endian);
            input.Seek(4, SeekOrigin.Current); // uint

            var count = input.ReadValueS32(endian);
            var unknown2 = new string[count];
            for (uint i = 0; i < count; i++)
            {
                unknown2[i] = input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
            }

            if (version == 200)
            {
                var count2 = input.ReadValueU32(endian);
                /*
                for (uint i = 0; i < count2; i++)
                {
                    input.ReadValueU32(endian);
                    input.ReadValueF32(endian);
                    input.ReadValueF32(endian);
                }
                */
                input.Seek((4 + 4 + 4) * count2, SeekOrigin.Current);
            }

            if (version >= 201 && version <= 211)
            {
                input.Seek(4, SeekOrigin.Current); // uint
            }

            var count3 = input.ReadValueU32(endian);
            for (uint i = 0; i < count3; i++)
            {
                var unknownx94 = version >= 86 ? input.ReadValueS32(endian) : -1;

                if (version >= 202)
                {
                    if (version >= 216)
                    {
                        if (version > 226)
                        {
                            if (version > 223)
                            {
                                var unknownx00 = Vector3.Read(input, endian);
                                var unknownx0C = Vector3.Read(input, endian);
                                var unknownx18 = input.ReadValueU32(endian);
                                var unknownx1C = input.ReadValueF32(endian);
                                var unknown3 = version >= 278
                                                   ? input.ReadStringU16(32, Encoding.ASCII, endian)
                                                   : "None";
                                var unknownx24 = version >= 278 ? input.ReadValueF32(endian) : 1.0f;
                                var unknownx28 = version >= 278 ? input.ReadValueF32(endian) : 0.0f;
                                var unknownx2C = version >= 278 ? input.ReadValueF32(endian) : 0.0f;
                                var unknownx30 = Color.Read(input);
                                var unknownx34 = input.ReadValueF32(endian);
                                var unknownx38 = (ushort)input.ReadValueU32(endian);

                                if (version < 284)
                                {
                                    /*
                                    input.ReadValueU32(endian);
                                    input.ReadValueU32(endian);
                                    input.ReadValueF32(endian);
                                    input.ReadValueU32(endian);
                                    */
                                    input.Seek(4 + 4 + 4 + 4, SeekOrigin.Current);
                                }

                                var unknownx3C = input.ReadValueF32(endian);
                                var unknownx40 = input.ReadValueF32(endian);
                                var unknownx44 = version >= 284 ? input.ReadValueF32(endian) : 0.0f;
                                var unknownx48 = version >= 284 ? input.ReadValueF32(endian) : 0.0f;

                                if (version < 284)
                                {
                                    /*
                                    Color.Read(input);
                                    input.ReadValueU32(endian);
                                    */
                                    input.Seek(4 + 4, SeekOrigin.Current);
                                }

                                if (version < 284 && (unknownx18 & (1 << 13)) != 0)
                                {
                                    input.ReadStringU16(1024, Encoding.ASCII, endian);
                                }
                            }
                            else
                            {
                                var unknownx00 = Vector3.Read(input, endian);
                                var unknownx0C = Vector3.Read(input, endian);
                                var unknownx18 = input.ReadValueU32(endian);
                                var unknownx1C = input.ReadValueF32(endian);
                                var unknownx30 = Color.Read(input);
                                var unknownx34 = input.ReadValueF32(endian);
                                var unknownx38 = (ushort)input.ReadValueU32(endian);

                                {
                                    /*
                                    input.ReadValueU32(endian);
                                    input.ReadValueU32(endian);
                                    input.ReadValueF32(endian);
                                    input.ReadValueU32(endian);
                                    */
                                    input.Seek(4 + 4 + 4 + 4, SeekOrigin.Current);
                                }

                                var unknownx3C = input.ReadValueF32(endian);
                                var unknownx40 = input.ReadValueF32(endian);

                                {
                                    /*
                                    Color.Read(input);
                                    input.ReadValueU32(endian);
                                    */
                                    input.Seek(4 + 4, SeekOrigin.Current);
                                }

                                if (version < 284 && (unknownx18 & (1 << 13)) != 0)
                                {
                                    input.ReadStringU16(1024, Encoding.ASCII, endian);
                                }
                            }
                        }
                        else
                        {
                            var unknownx00 = Vector3.Read(input, endian);
                            var unknownx0C = Vector3.Read(input, endian);
                            var unknownx18 = input.ReadValueU32(endian);
                            var unknownx1C = input.ReadValueF32(endian);
                            var unknownx30 = Color.Read(input);
                            var unknownx34 = input.ReadValueF32(endian);
                            var unknownx38 = (ushort)input.ReadValueU32(endian);

                            {
                                /*
                                input.ReadValueU32(endian);
                                input.ReadValueU32(endian);
                                input.ReadValueF32(endian);
                                input.ReadValueU32(endian);
                                */
                                input.Seek(4 + 4 + 4 + 4, SeekOrigin.Current);
                            }

                            var unknownx3C = input.ReadValueF32(endian);
                            var unknownx40 = input.ReadValueF32(endian);

                            {
                                /*
                                Color.Read(input);
                                input.ReadValueU32(endian);
                                */
                                input.Seek(4 + 4, SeekOrigin.Current);
                            }

                            if (version >= 224)
                            {
                                input.Seek(4, SeekOrigin.Current); // uint
                            }

                            if (version == 226)
                            {
                                input.Seek(4, SeekOrigin.Current); // uint
                            }

                            if (version < 284 && (unknownx18 & (1 << 13)) != 0)
                            {
                                input.ReadStringU16(1024, Encoding.ASCII, endian);
                            }
                        }
                    }
                    else
                    {
                        var unknownx00 = Vector3.Read(input, endian);
                        var unknownx0C = Vector3.Read(input, endian);
                        var unknownx18 = input.ReadValueU32(endian);
                        var unknownx1C = input.ReadValueF32(endian);
                        input.Seek(4, SeekOrigin.Current); // float
                        var unknownx30 = Color.Read(input);
                        var unknownx34 = input.ReadValueF32(endian);
                        var unknownx38 = (ushort)input.ReadValueU32(endian);

                        {
                            /*
                            input.ReadValueU32(endian);
                            input.ReadValueU32(endian);
                            input.ReadValueU32(endian);
                            input.ReadValueF32(endian);
                            input.ReadValueU32(endian);
                            */
                            input.Seek(4 + 4 + 4 + 4 + 4, SeekOrigin.Current);
                        }

                        var unknownx3C = input.ReadValueF32(endian);
                        var unknownx40 = input.ReadValueF32(endian);

                        {
                            /*
                            Color.Read(input);
                            input.ReadValueU32(endian);
                            */
                            input.Seek(4 + 4, SeekOrigin.Current);
                        }

                        if ((unknownx18 & (1 << 3)) != 0)
                        {
                            input.ReadStringU16(1024, Encoding.ASCII, endian);
                        }
                    }
                }
                else
                {
                    var unknownx00 = Vector3.Read(input, endian);
                    var unknownx0C = Vector3.Read(input, endian);

                    uint unknownx18 = 0;

                    if (version >= 75 && input.ReadValueB8() == true)
                    {
                        unknownx18 |= 0x40000000;
                    }

                    if (version >= 101 && input.ReadValueB8() == true)
                    {
                        unknownx18 |= 0x2;
                    }

                    if (version >= 104 && input.ReadValueB8() == true)
                    {
                        unknownx18 |= 0x4;
                    }

                    if (version >= 104 && input.ReadValueB8() == true)
                    {
                        unknownx18 |= 0x8;
                    }

                    if (version >= 83)
                    {
                        input.Seek(1, SeekOrigin.Current); // bool
                    }

                    if (version >= 120 && input.ReadValueB8() == true)
                    {
                        unknownx18 |= 0x20;
                    }

                    var j = version >= 113 && input.ReadValueB8() == true;

                    if (version >= 117 && input.ReadValueB8() == true)
                    {
                        unknownx18 |= 0x40;
                    }

                    var unknownx1C = version >= 122 ? input.ReadValueF32(endian) : -1.0f;

#if RF1_HACK
                    if (version >= 180)
                    {
                        input.ReadStringU16(32, Encoding.ASCII, endian);
                    }
#endif

                    if (false)
                    {
                        input.Seek(4, SeekOrigin.Current); // float
                        var unknownx30 = Color.Read(input);
                        input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
                        input.Seek(4, SeekOrigin.Current); // float
                        var unknownx38 = version >= 89 ? (ushort)input.ReadValueU32(endian) : 1;

                        if (version >= 109)
                        {
                            input.Seek(4, SeekOrigin.Current);
                        }
                        //version >= 109 ? input.ReadValueU32(endian) : 60;

                        if (version < 116 || input.ReadValueB8() == true)
                        {
                            unknownx18 |= 0x10;
                        }

                            if (version >= 116)
                            {
                                input.Seek(4 + 4 + 4 + 4, SeekOrigin.Current);
                            }
                            /*
                            version >= 116 ? input.ReadValueU32(endian) : 64;
                            version >= 116 ? input.ReadValueU32(endian) : 64;
                            version >= 116 ? input.ReadValueF32(endian) : 180.0f;
                            version >= 116 ? input.ReadValueU32(endian) : 0;
                            */

                        var unknownx3C = version >= 129 ? input.ReadValueF32(endian) : 0.0f;
                        var unknownx40 = version >= 129 ? input.ReadValueF32(endian) : 0.0f;
                    }

                    if ((unknownx18 & 0x20) != 0)
                    {
                        if (version >= 120)
                        {
                            input.Seek(4, SeekOrigin.Current);
                        }
                        //version >= 120 && (unknown18 & 0x20) != 0 ? Color.Read(input) : new Color(0x64, 0x64, 0x64, 0xFF);
                    }
                }

                if (version >= 231 && version < 252)
                {
                    var count4 = input.ReadValueU32(endian);
                    input.Seek(4 * count4, SeekOrigin.Current); // uints
                }
            }

            var count5 = version >= 113 ? input.ReadValueU32(endian) : 0;
            for (uint i = 0; i < count5; i++)
            {
                var unknown3 = input.ReadValueU32(endian);
                var count6 = input.ReadValueU32(endian);
                for (uint j = 0; j < count6; j++)
                {
                    var unknown4 = input.ReadValueU32(endian);
                }
            }

            var count7 = version >= 216 ? input.ReadValueU32(endian) : 0;
            for (uint i = 0; i < count7; i++)
            {
                var unknown5 = input.ReadValueU32(endian);
                var unknown6 = input.ReadValueU32(endian);
            }

            var count8 = input.ReadValueU32(endian);
            for (uint i = 0; i < count8; i++)
            {
                var unknown7 = input.ReadValueU32(endian);
                var unknown8 = input.ReadValueU32(endian);
                var unknown9 = Vector3.Read(input, endian);
                var unknown10 = Vector3.Read(input, endian);
            }

            var count9 = input.ReadValueU32(endian);
            for (uint i = 0; i < count9; i++)
            {
                var unknown11 = Vector3.Read(input, endian);
            }

            var count10 = input.ReadValueU32(endian);
            for (uint i = 0; i < count10; i++)
            {
                var unknown12 = version >= 167 ? Vector3.Read(input, endian) : new Vector3();
                var unknown13 = version >= 167 ? input.ReadValueF32(endian) : 0.0f;
                var unknown14 = input.ReadValueU32(endian);

                var unknown15 = version < 212 ? input.ReadValueS32(endian) : -1;

                if (version >= 226)
                {
                    input.Seek(4, SeekOrigin.Current);
                }
                // version >= 266 ? input.ReadValueS32(endian) : -1

                var unknown16 = version >= 46 ? input.ReadValueS32(endian) : -1; // 114

                if (version >= 66 && version < 212)
                {
                    input.Seek(4 + 4, SeekOrigin.Current);
                }
                //version >= 66 && version < 212 ? input.ReadValueS32(endian) : -1
                //version >= 66 && version < 212 ? input.ReadValueS32(endian) : -1

                var unknown17 = version >= 63 ? input.ReadValueU32(endian) : 0; // 113

                uint unknown18; // 111
                if (version >= 77)
                {
                    unknown18 = input.ReadValueU32(endian);
                    if (version < 234 & (unknown18 & 0x10) != 0)
                    {
                        unknown18 &= 0xFFFFFFEF;
                        unknown18 |= 0x8000;
                    }
                }
                else
                {
                    unknown18 = 0u;
                    if (version >= 75 && input.ReadValueB8() == true)
                    {
                        unknown18 |= 1;
                    }
                }

                if (version >= 79)
                {
                    input.Seek(4, SeekOrigin.Current);
                }
                //version >= 29 ? input.ReadValueU32(endian) : 0

                unknown18 &= 0xFF0FFFFF;

                if (version >= 295 && (unknown18 & 0x8000) != 0)
                {
                    input.Seek(4, SeekOrigin.Current); // float
                    var unknown19 = input.ReadValueF32(endian); // 73
                    if (unknown19.Equals(1.0f) == true)
                    {
                        unknown18 |= 0x100000;
                    }
                    else if (unknown19.Equals(1.35f) == true)
                    {
                        unknown18 |= 0x200000;
                    }
                    else if (unknown19.Equals(1.5f) == true)
                    {
                        unknown18 |= 0x400000;
                    }
                    else
                    {
                        unknown18 |= 0x800000;
                    }
                }

                if (version >= 217 && version < 234 || version >= 250)
                {
                    input.Seek(3, SeekOrigin.Current); // 3 bytes
                    var unknown19 = input.ReadValueF32(endian); // 75

                    {
                        unknown18 |= 0x4000000;
                    }
                }

                if (version >= 230 && version < 252)
                {
                    input.Seek(6, SeekOrigin.Current); // 6 bytes
                }

                var unknown20 = input.ReadValueU32(endian); // 81
                var count11 = input.ReadValueU32(endian);
                for (uint j = 0; j < count11; j++)
                {
                    var unknown21 = input.ReadValueU32(endian);
                    var unknown22 = input.ReadValueF32(endian);
                    var unknown23 = input.ReadValueF32(endian);

                    var unknown24 = version >= 212 ? input.ReadValueU8() : 0x80;
                    var unknown25 = version >= 212 ? input.ReadValueU8() : 0x80;
                    var unknown26 = version >= 212 ? input.ReadValueU8() : 0x80;
                    var unknown27 = version >= 212 ? input.ReadValueU8() : 0xFF;

                    if (version < 212 && unknown15 > -1)
                    {
                        input.Seek(8, SeekOrigin.Current); // two floats
                    }
                }
            }

            if (version < 212)
            {
                var count12 = input.ReadValueU32(endian);
                for (uint i = 0; i < count12; i++)
                {
                    // string array
                    throw new NotImplementedException();
                }
            }

            if (version < 295 && (version > 200 && version < 234 || version >= 251))
            {
                var count13 = version >= 94 ? input.ReadValueU32(endian) : 0;
                for (uint i = 0; i < count13; i++)
                {
                    input.Seek(4 + 4 + 4, SeekOrigin.Current); // uint, float, float
                }
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }
    }
}
