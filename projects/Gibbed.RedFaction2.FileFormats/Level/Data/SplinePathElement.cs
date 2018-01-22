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

using System.Collections.Generic;
using System.IO;
using Gibbed.IO;
using Newtonsoft.Json;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    public class SplinePathElement : ObjectElement
    {
        #region Fields
        private bool _Unknown6;
        private int _Unknown7;
        private readonly List<Node> _Unknown8;
        private int _Unknown9;
        #endregion

        public SplinePathElement()
        {
            this._Unknown8 = new List<Node>();
        }

        protected override ushort ClassNameMaximumLength
        {
            get { return ushort.MaxValue; }
        }

        protected override ushort ScriptNameMaximumLength
        {
            get { return ushort.MaxValue; }
        }

        #region Properties
        [JsonProperty("__u6")]
        public bool Unknown6
        {
            get { return this._Unknown6; }
            set { this._Unknown6 = value; }
        }

        [JsonProperty("__u7")]
        public int Unknown7
        {
            get { return this._Unknown7; }
            set { this._Unknown7 = value; }
        }

        [JsonProperty("__u8")]
        public List<Node> Unknown8
        {
            get { return this._Unknown8; }
        }

        [JsonProperty("__u9")]
        public int Unknown9
        {
            get { return this._Unknown9; }
            set { this._Unknown9 = value; }
        }
        #endregion

        public override void Read(Stream input, uint version, Endian endian)
        {
            base.Read(input, version, endian);

            this._Unknown6 = version >= 256 && input.ReadValueB8() == true;
            this._Unknown7 = version >= 257 ? input.ReadValueS32(endian) : -1;
            
            var unknown8Count = input.ReadValueU32(endian);
            this._Unknown8.Clear();
            for (uint i = 0; i < unknown8Count; i++)
            {
                var node = new Node();
                node.Read(input, version, endian);
                this._Unknown8.Add(node);
            }

            this._Unknown9 = version >= 238 ? input.ReadValueS32(endian) : -1;
        }

        public override void Write(Stream output, uint version, Endian endian)
        {
            base.Write(output, version, endian);

            if (version >= 256)
            {
                output.WriteValueB8(this._Unknown6);
            }

            if (version >= 257)
            {
                output.WriteValueS32(this._Unknown7, endian);
            }

            output.WriteValueS32(this._Unknown8.Count);
            foreach (var node in this._Unknown8)
            {
                node.Write(output, version, endian);
            }

            if (version >= 238)
            {
                output.WriteValueS32(this._Unknown9, endian);
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class Node : ISerializableElement
        {
            #region Fields
            private int _Unknown1;
            private Vector3 _Unknown2;
            private Vector3 _Unknown3;
            private Vector3 _Unknown4;
            private uint _Unknown5;
            private Vector4 _Unknown6;
            private float _Unknown7;
            private float _Unknown8;
            private float _Unknown9;
            private float _Unknown10;
            private float _Unknown11;
            #endregion

            #region Properties
            [JsonProperty("__u1")]
            public int Unknown1
            {
                get { return this._Unknown1; }
                set { this._Unknown1 = value; }
            }

            [JsonProperty("__u2")]
            public Vector3 Unknown2
            {
                get { return this._Unknown2; }
                set { this._Unknown2 = value; }
            }

            [JsonProperty("__u3")]
            public Vector3 Unknown3
            {
                get { return this._Unknown3; }
                set { this._Unknown3 = value; }
            }

            [JsonProperty("__u4")]
            public Vector3 Unknown4
            {
                get { return this._Unknown4; }
                set { this._Unknown4 = value; }
            }

            [JsonProperty("__u5")]
            public uint Unknown5
            {
                get { return this._Unknown5; }
                set { this._Unknown5 = value; }
            }

            [JsonProperty("__u6")]
            public Vector4 Unknown6
            {
                get { return this._Unknown6; }
                set { this._Unknown6 = value; }
            }

            [JsonProperty("__u7")]
            public float Unknown7
            {
                get { return this._Unknown7; }
                set { this._Unknown7 = value; }
            }

            [JsonProperty("__u8")]
            public float Unknown8
            {
                get { return this._Unknown8; }
                set { this._Unknown8 = value; }
            }

            [JsonProperty("__u9")]
            public float Unknown9
            {
                get { return this._Unknown9; }
                set { this._Unknown9 = value; }
            }

            [JsonProperty("__u10")]
            public float Unknown10
            {
                get { return this._Unknown10; }
                set { this._Unknown10 = value; }
            }

            [JsonProperty("__u11")]
            public float Unknown11
            {
                get { return this._Unknown11; }
                set { this._Unknown11 = value; }
            }
            #endregion

            public void Read(Stream input, uint version, Endian endian)
            {
                this._Unknown1 = input.ReadValueS32(endian);
                this._Unknown2 = Vector3.Read(input, endian);
                this._Unknown3 = Vector3.Read(input, endian);
                this._Unknown4 = Vector3.Read(input, endian);
                this._Unknown5 = input.ReadValueU32(endian);
                this._Unknown6 = Vector4.Read(input, endian);
                this._Unknown7 = input.ReadValueF32(endian); // value * 127.0f
                this._Unknown8 = input.ReadValueF32(endian); // value * 127.0f
                this._Unknown9 = input.ReadValueF32(endian); // value * 127.0f
                this._Unknown10 = input.ReadValueF32(endian); // value * 2.54f
                this._Unknown11 = input.ReadValueF32(endian); // value * 2.54f
            }

            public void Write(Stream output, uint version, Endian endian)
            {
                output.WriteValueS32(this._Unknown1, endian);
                Vector3.Write(output, this._Unknown2, endian);
                Vector3.Write(output, this._Unknown3, endian);
                Vector3.Write(output, this._Unknown4, endian);
                output.WriteValueU32(this._Unknown5, endian);
                Vector4.Write(output, this._Unknown6, endian);
                output.WriteValueF32(this._Unknown7, endian);
                output.WriteValueF32(this._Unknown8, endian);
                output.WriteValueF32(this._Unknown9, endian);
                output.WriteValueF32(this._Unknown10, endian);
                output.WriteValueF32(this._Unknown11, endian);
            }
        }

        public class ArrayElement : SerializableArrayElement<SplinePathElement>
        {
        }
    }
}
