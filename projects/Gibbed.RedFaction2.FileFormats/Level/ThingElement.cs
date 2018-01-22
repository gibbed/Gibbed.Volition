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
using System.Collections.Generic;

namespace Gibbed.RedFaction2.FileFormats.Level
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ThingElement : ISerializableElement
    {
        #region Fields
        private uint _Unknown1;
        private string _Unknown2;
        private uint _Unknown3;
        private readonly List<string> _Unknown4;
        private readonly List<Tuple<int, float, float>> _Unknown5;
        private uint _Unknown6;
        private readonly List<Unknown7> _Unknown7;
        private readonly List<KeyValuePair<uint, List<uint>>> _Unknown8;
        private readonly List<KeyValuePair<uint, uint>>  _Unknown9;
        private readonly List<Tuple<uint, uint, Vector3, Vector3>> _Unknown10;
        private readonly List<Vector3> _Unknown11;
        private readonly List<Unknown12> _Unknown12;
        private readonly List<Tuple<uint, float, float>> _Unknown14;
        #endregion

        public ThingElement()
        {
            this._Unknown4 = new List<string>();
            this._Unknown5 = new List<Tuple<int, float, float>>();
            this._Unknown7 = new List<Unknown7>();
            this._Unknown8 = new List<KeyValuePair<uint, List<uint>>>();
            this._Unknown9 = new List<KeyValuePair<uint, uint>>();
            this._Unknown10 = new List<Tuple<uint, uint, Vector3, Vector3>>();
            this._Unknown11 = new List<Vector3>();
            this._Unknown12 = new List<Unknown12>();
            this._Unknown14 = new List<Tuple<uint, float, float>>();
        }

        #region Properties
        public uint Unknown1
        {
            get { return this._Unknown1; }
            set { this._Unknown1 = value; }
        }

        public string Unknown2
        {
            get { return this._Unknown2; }
            set { this._Unknown2 = value; }
        }

        public uint Unknown3
        {
            get { return this._Unknown3; }
            set { this._Unknown3 = value; }
        }

        public List<string> Unknown4
        {
            get { return this._Unknown4; }
        }

        public List<Tuple<int, float, float>> Unknown5
        {
            get { return this._Unknown5; }
        }

        public uint Unknown6
        {
            get { return this._Unknown6; }
            set { this._Unknown6 = value; }
        }

        public List<Unknown7> Unknown7s
        {
            get { return this._Unknown7; }
        }

        public List<KeyValuePair<uint, List<uint>>> Unknown8
        {
            get { return this._Unknown8; }
        }

        public List<KeyValuePair<uint, uint>> Unknown9
        {
            get { return this._Unknown9; }
        }

        public List<Tuple<uint, uint, Vector3, Vector3>> Unknown10
        {
            get { return this._Unknown10; }
        }

        public List<Vector3> Unknown11
        {
            get { return this._Unknown11; }
        }

        public List<Unknown12> Unknown12s
        {
            get { return this._Unknown12; }
        }

        public List<Tuple<uint, float, float>> Unknown14
        {
            get { return this._Unknown14; }
        }
        #endregion

        public void Read(Stream input, uint version, Endian endian)
        {
            if (version == 200)
            {
                this._Unknown1 = input.ReadValueU32(endian);
            }

            this._Unknown2 = input.ReadStringU16(63, Encoding.ASCII, endian);
            this._Unknown3 = input.ReadValueU32(endian);

            var unknown4Count = input.ReadValueS32(endian);
            this._Unknown4.Clear();
            for (uint i = 0; i < unknown4Count; i++)
            {
                this._Unknown4.Add(input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian));
            }

            this._Unknown5.Clear();
            if (version == 200)
            {
                var unknown5Count = input.ReadValueU32(endian);
                for (uint i = 0; i < unknown5Count; i++)
                {
                    var unknown5Item1 = input.ReadValueS32(endian);
                    var unknown5Item2 = input.ReadValueF32(endian);
                    var unknown5Item3 = input.ReadValueF32(endian);
                    this._Unknown5.Add(new Tuple<int, float, float>(unknown5Item1, unknown5Item2, unknown5Item3));
                }
            }

            if (version >= 201 && version <= 211)
            {
                this._Unknown6 = input.ReadValueU32(endian);
            }

            var unknown7Count = input.ReadValueU32(endian);
            this._Unknown7.Clear();
            for (uint i = 0; i < unknown7Count; i++)
            {
                var unknown7 = new Unknown7();
                unknown7.Read(input, version, endian);
                this._Unknown7.Add(unknown7);
            }

            var unknown8Count = version >= 113 ? input.ReadValueU32(endian) : 0;
            this._Unknown8.Clear();
            for (uint i = 0; i < unknown8Count; i++)
            {
                var unknown8Key = input.ReadValueU32(endian);
                var unknown8ValueCount = input.ReadValueU32(endian);
                var unknown8Value = new List<uint>();
                for (uint j = 0; j < unknown8ValueCount; j++)
                {
                    unknown8Value.Add(input.ReadValueU32(endian));
                }
                this._Unknown8.Add(new KeyValuePair<uint, List<uint>>(unknown8Key, unknown8Value));
            }

            var unknown9Count = version >= 216 ? input.ReadValueU32(endian) : 0;
            this._Unknown9.Clear();
            for (uint i = 0; i < unknown9Count; i++)
            {
                var unknown9Key = input.ReadValueU32(endian);
                var unknown9Value = input.ReadValueU32(endian);
                this._Unknown9.Add(new KeyValuePair<uint, uint>(unknown9Key, unknown9Value));
            }

            var unknown10Count = input.ReadValueU32(endian);
            this._Unknown10.Clear();
            for (uint i = 0; i < unknown10Count; i++)
            {
                var unknown10Item1 = input.ReadValueU32(endian);
                var unknown10Item2 = input.ReadValueU32(endian);
                var unknown10Item3 = Vector3.Read(input, endian);
                var unknown10Item4 = Vector3.Read(input, endian);
                this._Unknown10.Add(new Tuple<uint, uint, Vector3, Vector3>(
                                        unknown10Item1,
                                        unknown10Item2,
                                        unknown10Item3,
                                        unknown10Item4));
            }

            var unknown11Count = input.ReadValueU32(endian);
            this._Unknown11.Clear();
            for (uint i = 0; i < unknown11Count; i++)
            {
                this._Unknown11.Add(Vector3.Read(input, endian));
            }

            var unknown12Count = input.ReadValueU32(endian);
            this._Unknown12.Clear();
            for (uint i = 0; i < unknown12Count; i++)
            {
                var unknown12 = new Unknown12();
                unknown12.Read(input, version, endian);
                this._Unknown12.Add(unknown12);
            }

            if (version < 212)
            {
                var unknown13Count = input.ReadValueU32(endian);
                for (uint i = 0; i < unknown13Count; i++)
                {
                    // string array
                    throw new NotImplementedException();
                }
            }

            this._Unknown14.Clear();
            if (version < 295 && (version > 200 && version < 234 || version >= 251))
            {
                var unknown14Count = version >= 94 ? input.ReadValueU32(endian) : 0;
                for (uint i = 0; i < unknown14Count; i++)
                {
                    var unknown14Item1 = input.ReadValueU32(endian);
                    var unknown14Item2 = input.ReadValueF32(endian);
                    var unknown14Item3 = input.ReadValueF32(endian);
                    this._Unknown14.Add(new Tuple<uint, float, float>(unknown14Item1, unknown14Item2, unknown14Item3));
                }
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }

        public class Unknown7 : ISerializableElement
        {
            public void Read(Stream input, uint version, Endian endian)
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

            public void Write(Stream output, uint version, Endian endian)
            {
                throw new NotImplementedException();
            }
        }

        public class Unknown12 : ISerializableElement
        {
            private Vector3 _Unknown1;
            private float _Unknown2;
            private uint _Unknown3;
            private int _Unknown4;

            public void Read(Stream input, uint version, Endian endian)
            {
                this._Unknown1 = version >= 167 ? Vector3.Read(input, endian) : new Vector3();
                this._Unknown2 = version >= 167 ? input.ReadValueF32(endian) : 0.0f;
                this._Unknown3 = input.ReadValueU32(endian);
                this._Unknown4 = version < 212 ? input.ReadValueS32(endian) : -1;

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

                    if (version < 212 && this._Unknown4 > -1)
                    {
                        input.Seek(8, SeekOrigin.Current); // two floats
                    }
                }
            }

            public void Write(Stream output, uint version, Endian endian)
            {
                throw new NotImplementedException();
            }
        }
    }
}
