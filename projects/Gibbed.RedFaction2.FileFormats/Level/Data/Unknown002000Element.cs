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
    public class Unknown002000Element : ISerializableElement
    {
        #region Fields
        private int _Uid;
        private Vector3 _Position;
        private Transform _Transform;
        private ThingElement _Unknown3;
        private uint _Unknown4;
        private int _Unknown5;
        private uint _Unknown6;
        private uint _Unknown7;
        private uint _Unknown8;
        private uint _Unknown9;
        private uint _Unknown10;
        private uint _Unknown11;
        private uint _Unknown12;
        private uint _Unknown13;
        private float _Unknown14;
        private float _Unknown15;
        private float _Unknown16;
        private float _Unknown17;
        private Color _Unknown18;
        private float _Unknown19;
        private bool _Unknown20;
        #endregion

        #region Properties
        [JsonProperty("uid")]
        public int Uid
        {
            get { return this._Uid; }
            set { this._Uid = value; }
        }

        [JsonProperty("pos")]
        public Vector3 Position
        {
            get { return this._Position; }
            set { this._Position = value; }
        }

        [JsonProperty("transform")]
        public Transform Transform
        {
            get { return this._Transform; }
            set { this._Transform = value; }
        }

        [JsonProperty("__u3")]
        public ThingElement Unknown3
        {
            get { return this._Unknown3; }
            set { this._Unknown3 = value; }
        }

        [JsonProperty("__u4")]
        public uint Unknown4
        {
            get { return this._Unknown4; }
            set { this._Unknown4 = value; }
        }

        [JsonProperty("__u5")]
        public int Unknown5
        {
            get { return this._Unknown5; }
            set { this._Unknown5 = value; }
        }

        [JsonProperty("__u6")]
        public uint Unknown6
        {
            get { return this._Unknown6; }
            set { this._Unknown6 = value; }
        }

        [JsonProperty("__u7")]
        public uint Unknown7
        {
            get { return this._Unknown7; }
            set { this._Unknown7 = value; }
        }

        [JsonProperty("__u8")]
        public uint Unknown8
        {
            get { return this._Unknown8; }
            set { this._Unknown8 = value; }
        }

        [JsonProperty("__u9")]
        public uint Unknown9
        {
            get { return this._Unknown9; }
            set { this._Unknown9 = value; }
        }

        [JsonProperty("__u10")]
        public uint Unknown10
        {
            get { return this._Unknown10; }
            set { this._Unknown10 = value; }
        }

        [JsonProperty("__u11")]
        public uint Unknown11
        {
            get { return this._Unknown11; }
            set { this._Unknown11 = value; }
        }

        [JsonProperty("__u12")]
        public uint Unknown12
        {
            get { return this._Unknown12; }
            set { this._Unknown12 = value; }
        }

        [JsonProperty("__u13")]
        public uint Unknown13
        {
            get { return this._Unknown13; }
            set { this._Unknown13 = value; }
        }

        [JsonProperty("__u14")]
        public float Unknown14
        {
            get { return this._Unknown14; }
            set { this._Unknown14 = value; }
        }

        [JsonProperty("__u15")]
        public float Unknown15
        {
            get { return this._Unknown15; }
            set { this._Unknown15 = value; }
        }

        [JsonProperty("__u16")]
        public float Unknown16
        {
            get { return this._Unknown16; }
            set { this._Unknown16 = value; }
        }

        [JsonProperty("__u17")]
        public float Unknown17
        {
            get { return this._Unknown17; }
            set { this._Unknown17 = value; }
        }

        [JsonProperty("__u18")]
        public Color Unknown18
        {
            get { return this._Unknown18; }
            set { this._Unknown18 = value; }
        }

        [JsonProperty("__u19")]
        public float Unknown19
        {
            get { return this._Unknown19; }
            set { this._Unknown19 = value; }
        }

        [JsonProperty("__u20")]
        public bool Unknown20
        {
            get { return this._Unknown20; }
            set { this._Unknown20 = value; }
        }
        #endregion

        public void Read(Stream input, uint version, Endian endian)
        {
            this._Uid = input.ReadValueS32(endian);
            this._Position = Vector3.Read(input, endian);
            this._Transform = Transform.Read(input, endian);
            this._Unknown3 = new ThingElement();
            this._Unknown3.Read(input, version, endian);
            this._Unknown4 = input.ReadValueU32(endian);
            this._Unknown5 = input.ReadValueS32(endian);

            if (version < 227)
            {
                if (version >= 224)
                {
                    this._Unknown6 = input.ReadValueU32(endian);
                }

                if (version >= 226)
                {
                    this._Unknown7 = input.ReadValueU32(endian);
                }
            }

            this._Unknown8 = input.ReadValueU32(endian);

            if (version >= 235 && version <= 251)
            {
                this._Unknown9 = input.ReadValueU32(endian);
            }

            if ((this._Unknown4 & 8) != 0)
            {
                this._Unknown10 = input.ReadValueU32(endian);
                this._Unknown11 = input.ReadValueU32(endian);
                this._Unknown12 = input.ReadValueU32(endian);
                this._Unknown13 = input.ReadValueU32(endian);
                this._Unknown14 = input.ReadValueF32(endian);
                this._Unknown15 = input.ReadValueF32(endian);
                this._Unknown16 = input.ReadValueF32(endian);
                this._Unknown17 = input.ReadValueF32(endian);
                this._Unknown18 = Color.Read(input);
                this._Unknown19 = input.ReadValueF32(endian);
                this._Unknown20 = input.ReadValueB8();
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }

        public class ArrayElement : SerializableArrayElement<Unknown002000Element>
        {
        }
    }
}
