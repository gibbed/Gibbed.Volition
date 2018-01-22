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
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.IO;
using Newtonsoft.Json;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GlareElement : ObjectElement
    {
        #region Fields
        private Color _Unknown6;
        private bool _Unknown7;
        private bool _Unknown8;
        private bool _Unknown9;
        private bool _Unknown10;
        private uint _Unknown11;
        private bool _Unknown12;
        private string _Unknown13;
        private float _Unknown14;
        private float _Unknown15;
        private float _Unknown16;
        private float _Unknown17;
        private float _Unknown18;
        private string _Unknown19;
        private float _Unknown20;
        private float _Unknown21;
        private float _Unknown22;
        private bool _Unknown23;
        private readonly List<int> _Links; 
        #endregion

        public GlareElement()
        {
            this._Links = new List<int>();
        }

        protected override ushort ClassNameMaximumLength
        {
            get { return 128; }
        }

        protected override ushort ScriptNameMaximumLength
        {
            get { return 128; }
        }

        #region Properties
        [JsonProperty("__u6")]
        public Color Unknown6
        {
            get { return this._Unknown6; }
            set { this._Unknown6 = value; }
        }

        [JsonProperty("__u7")]
        public bool Unknown7
        {
            get { return this._Unknown7; }
            set { this._Unknown7 = value; }
        }

        [JsonProperty("__u8")]
        public bool Unknown8
        {
            get { return this._Unknown8; }
            set { this._Unknown8 = value; }
        }

        [JsonProperty("__u9")]
        public bool Unknown9
        {
            get { return this._Unknown9; }
            set { this._Unknown9 = value; }
        }

        [JsonProperty("__u10")]
        public bool Unknown10
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
        public bool Unknown12
        {
            get { return this._Unknown12; }
            set { this._Unknown12 = value; }
        }

        [JsonProperty("__u13")]
        public string Unknown13
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
        public float Unknown18
        {
            get { return this._Unknown18; }
            set { this._Unknown18 = value; }
        }

        [JsonProperty("__u19")]
        public string Unknown19
        {
            get { return this._Unknown19; }
            set { this._Unknown19 = value; }
        }

        [JsonProperty("__u20")]
        public float Unknown20
        {
            get { return this._Unknown20; }
            set { this._Unknown20 = value; }
        }

        [JsonProperty("__u21")]
        public float Unknown21
        {
            get { return this._Unknown21; }
            set { this._Unknown21 = value; }
        }

        [JsonProperty("__u22")]
        public float Unknown22
        {
            get { return this._Unknown22; }
            set { this._Unknown22 = value; }
        }

        [JsonProperty("__u23")]
        public bool Unknown23
        {
            get { return this._Unknown23; }
            set { this._Unknown23 = value; }
        }

        [JsonProperty("links")]
        public List<int> Links
        {
            get { return this._Links; }
        }
        #endregion

        public override void Read(Stream input, uint version, Endian endian)
        {
            base.Read(input, version, endian);

            this._Unknown6 = Color.Read(input);
            this._Unknown7 = version >= 259 && input.ReadValueB8() == true;
            this._Unknown8 = version >= 288 && input.ReadValueB8() == true;
            this._Unknown9 = version >= 289 && input.ReadValueB8() == true;
            this._Unknown10 = version >= 292 && input.ReadValueB8() == true;

            if (version < 232)
            {
                this._Unknown11 = input.ReadValueU32(endian);
            }

            this._Unknown12 = version >= 232 && input.ReadValueB8() == true;

            this._Unknown13 = input.ReadStringU16(23, Encoding.ASCII, endian);
            if (string.IsNullOrEmpty(this._Unknown13) == false)
            {
                this._Unknown14 = input.ReadValueF32(endian);
                this._Unknown15 = input.ReadValueF32(endian);
                this._Unknown16 = input.ReadValueF32(endian);
                this._Unknown17 = input.ReadValueF32(endian);
                this._Unknown18 = input.ReadValueF32(endian);
            }

            this._Unknown19 = input.ReadStringU16(23, Encoding.ASCII, endian);
            if (string.IsNullOrEmpty(this._Unknown19) == false)
            {
                this._Unknown20 = input.ReadValueF32(endian);
                this._Unknown21 = input.ReadValueF32(endian);
                this._Unknown22 = version >= 273 ? input.ReadValueF32(endian) : 1.0f;
            }

            this._Unknown23 = version >= 276 && input.ReadValueB8() == true;
            
            var linkCount = input.ReadValueU32(endian);
            this._Links.Clear();
            for (uint i = 0; i < linkCount; i++)
            {
                this._Links.Add(input.ReadValueS32(endian));
            }
        }

        public override void Write(Stream output, uint version, Endian endian)
        {
            base.Write(output, version, endian);
            throw new NotImplementedException();
        }

        public class ArrayElement : SerializableArrayElement<GlareElement>
        {
        }
    }
}
