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

using System.IO;
using System.Text;
using Gibbed.IO;
using Newtonsoft.Json;

namespace Gibbed.RedFaction2.FileFormats.Level.Metadata
{
    public class SettingsElement : IElement
    {
        private string _DefaultTexture;
        private int _Hardness;
        private Color _AmbientLightColor;
        private bool _Unknown3;
        private Color _DistanceBasedFogColor;
        private float _DistanceBasedFogNearClipPlane;
        private float _DistanceBasedFogFarClipPlane;
        private Color _Unknown7;
        private byte _Unknown8;

        public string DefaultTexture
        {
            get { return this._DefaultTexture; }
            set { this._DefaultTexture = value; }
        }

        public int Hardness
        {
            get { return this._Hardness; }
            set { this._Hardness = value; }
        }

        public Color AmbientLightColor
        {
            get { return this._AmbientLightColor; }
            set { this._AmbientLightColor = value; }
        }

        public bool Unknown3
        {
            get { return this._Unknown3; }
            set { this._Unknown3 = value; }
        }

        public Color DistanceBasedFogColor
        {
            get { return this._DistanceBasedFogColor; }
            set { this._DistanceBasedFogColor = value; }
        }

        public float DistanceBasedFogNearClipPlane
        {
            get { return this._DistanceBasedFogNearClipPlane; }
            set { this._DistanceBasedFogNearClipPlane = value; }
        }

        public float DistanceBasedFogFarClipPlane
        {
            get { return this._DistanceBasedFogFarClipPlane; }
            set { this._DistanceBasedFogFarClipPlane = value; }
        }

        public Color Unknown7
        {
            get { return this._Unknown7; }
            set { this._Unknown7 = value; }
        }

        public byte Unknown8
        {
            get { return this._Unknown8; }
            set { this._Unknown8 = value; }
        }

        public void Read(Stream input, uint version, Endian endian)
        {
            this._DefaultTexture = input.ReadStringU16(40, Encoding.ASCII, endian);
            this._Hardness = input.ReadValueS32(endian);
            this._AmbientLightColor = Color.Read(input);
            this._Unknown3 = input.ReadValueB8();
            this._DistanceBasedFogColor = Color.Read(input);
            this._DistanceBasedFogNearClipPlane = input.ReadValueF32(endian);
            this._DistanceBasedFogFarClipPlane = input.ReadValueF32(endian);

            if (version >= 247)
            {
                input.Seek(4, SeekOrigin.Current); // color
                input.Seek(4, SeekOrigin.Current); // float
                input.Seek(4, SeekOrigin.Current); // float
            }

            if (version >= 272)
            {
                input.Seek(4, SeekOrigin.Current); // float
            }

            if (version >= 258)
            {
                input.Seek(4, SeekOrigin.Current); // float
            }

            if (version >= 270)
            {
                this._Unknown7 = Color.Read(input);
                this._Unknown8 = (byte)input.ReadValueU32(endian);
            }

            if (version >= 287)
            {
                input.Seek(4, SeekOrigin.Current); // float
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            output.WriteStringU16(this._DefaultTexture, 40, Encoding.ASCII, endian);
            output.WriteValueS32(this._Hardness, endian);
            Color.Write(output, this._AmbientLightColor);
            output.WriteValueB8(this._Unknown3);
            Color.Write(output, this._DistanceBasedFogColor);
            output.WriteValueF32(this._DistanceBasedFogNearClipPlane, endian);
            output.WriteValueF32(this._DistanceBasedFogFarClipPlane, endian);

            if (version >= 247)
            {
                output.WriteValueU32(0, endian); // color
                output.WriteValueU32(0, endian); // float
                output.WriteValueU32(0, endian); // float
            }

            if (version >= 272)
            {
                output.WriteValueU32(0, endian); // float
            }

            if (version >= 258)
            {
                output.WriteValueU32(0, endian); // float
            }

            if (version >= 270)
            {
                Color.Write(output, this._Unknown7);
                output.WriteValueU32((uint)this._Unknown8, endian);
            }

            if (version >= 287)
            {
                output.WriteValueU32(0, endian); // float
            }
        }

        public void ImportJson(JsonReader reader)
        {
            var serializer = new JsonSerializer();
            var item = serializer.Deserialize<Item>(reader);
            this._DefaultTexture = item.Unknown1;
            this._AmbientLightColor = item.Unknown2;
            this._Unknown3 = item.Unknown3;
            this._DistanceBasedFogColor = item.Unknown4;
            this._DistanceBasedFogNearClipPlane = item.Unknown5;
            this._DistanceBasedFogFarClipPlane = item.Unknown6;
            this._Unknown7 = item.Unknown7;
            this._Unknown8 = item.Unknown8;
        }

        public void ExportJson(JsonWriter writer)
        {
            Item item;
            item.Unknown1 = this._DefaultTexture;
            item.Unknown2 = this._AmbientLightColor;
            item.Unknown3 = this._Unknown3;
            item.Unknown4 = this._DistanceBasedFogColor;
            item.Unknown5 = this._DistanceBasedFogNearClipPlane;
            item.Unknown6 = this._DistanceBasedFogFarClipPlane;
            item.Unknown7 = this._Unknown7;
            item.Unknown8 = this._Unknown8;
            var serializer = new JsonSerializer();
            serializer.Serialize(writer, item);
        }

        [JsonObject(MemberSerialization.OptIn)]
        public struct Item
        {
            [JsonProperty("u1")]
            public string Unknown1;

            [JsonProperty("u2")]
            public Color Unknown2;

            [JsonProperty("u3")]
            public bool Unknown3;

            [JsonProperty("u4")]
            public Color Unknown4;

            [JsonProperty("u5")]
            public float Unknown5;

            [JsonProperty("u6")]
            public float Unknown6;

            [JsonProperty("u7")]
            public Color Unknown7;

            [JsonProperty("u8")]
            public byte Unknown8;
        }
    }
}
