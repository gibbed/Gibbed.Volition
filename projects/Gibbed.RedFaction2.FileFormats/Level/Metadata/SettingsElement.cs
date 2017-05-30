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
        private string _Unknown1;
        private Color _Unknown2;
        private bool _Unknown3;
        private Color _Unknown4;
        private float _Unknown5;
        private float _Unknown6;
        private Color _Unknown7;
        private byte _Unknown8;

        public string Unknown1
        {
            get { return this._Unknown1; }
            set { this._Unknown1 = value; }
        }

        public Color Unknown2
        {
            get { return this._Unknown2; }
            set { this._Unknown2 = value; }
        }

        public bool Unknown3
        {
            get { return this._Unknown3; }
            set { this._Unknown3 = value; }
        }

        public Color Unknown4
        {
            get { return this._Unknown4; }
            set { this._Unknown4 = value; }
        }

        public float Unknown5
        {
            get { return this._Unknown5; }
            set { this._Unknown5 = value; }
        }

        public float Unknown6
        {
            get { return this._Unknown6; }
            set { this._Unknown6 = value; }
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
            this._Unknown1 = input.ReadStringU16(40, Encoding.ASCII, endian);
            input.Seek(4, SeekOrigin.Current);
            this._Unknown2 = Color.Read(input);
            this._Unknown3 = input.ReadValueB8();
            this._Unknown4 = Color.Read(input);
            this._Unknown5 = input.ReadValueF32(endian);
            this._Unknown6 = input.ReadValueF32(endian);

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
            output.WriteStringU16(this._Unknown1, 40, Encoding.ASCII, endian);
            output.WriteValueU32(0, endian);
            Color.Write(output, this._Unknown2);
            output.WriteValueB8(this._Unknown3);
            Color.Write(output, this._Unknown4);
            output.WriteValueF32(this._Unknown5, endian);
            output.WriteValueF32(this._Unknown6, endian);

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
            this._Unknown1 = item.Unknown1;
            this._Unknown2 = item.Unknown2;
            this._Unknown3 = item.Unknown3;
            this._Unknown4 = item.Unknown4;
            this._Unknown5 = item.Unknown5;
            this._Unknown6 = item.Unknown6;
            this._Unknown7 = item.Unknown7;
            this._Unknown8 = item.Unknown8;
        }

        public void ExportJson(JsonWriter writer)
        {
            Item item;
            item.Unknown1 = this._Unknown1;
            item.Unknown2 = this._Unknown2;
            item.Unknown3 = this._Unknown3;
            item.Unknown4 = this._Unknown4;
            item.Unknown5 = this._Unknown5;
            item.Unknown6 = this._Unknown6;
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
