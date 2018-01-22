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
using System.Text;
using Gibbed.IO;
using Newtonsoft.Json;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ClutterElement : ObjectElement
    {
        #region Fields
        private List<KeyValuePair<string, uint>> _Unknown1;
        private string _SkinName;
        private uint _Unknown2;
        private readonly List<int> _Links;
        #endregion

        public ClutterElement()
        {
            this._Unknown1 = new List<KeyValuePair<string, uint>>();
            this._Links = new List<int>();
        }

        protected override ushort ClassNameMaximumLength
        {
            get { return 32; }
        }

        protected override ushort ScriptNameMaximumLength
        {
            get { return 128; }
        }

        #region Properties
        [JsonProperty("__u1")]
        public List<KeyValuePair<string, uint>> Unknown1
        {
            get { return this._Unknown1; }
            set { this._Unknown1 = value; }
        }

        [JsonProperty("skin_name")]
        public string SkinName
        {
            get { return this._SkinName; }
            set { this._SkinName = value; }
        }

        [JsonProperty("__u2")]
        public uint Unknown2
        {
            get { return this._Unknown2; }
            set { this._Unknown2 = value; }
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

            this._Unknown1.Clear();
            if (version < 232)
            {
                var unknown1Count = input.ReadValueU32(endian);
                for (uint i = 0; i < unknown1Count; i++)
                {
                    var unknown1Key = input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
                    var unknown1Value = input.ReadValueU32(endian);
                    this._Unknown1.Add(new KeyValuePair<string, uint>(unknown1Key, unknown1Value));
                }
            }

            this._SkinName = input.ReadStringU16(32, Encoding.ASCII, endian);

            if (version >= 221 && version <= 231)
            {
                this._Unknown2 = input.ReadValueU32(endian);
            }

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

            if (version < 232)
            {
                output.WriteValueS32(this._Unknown1.Count, endian);
                foreach (var kv in this._Unknown1)
                {
                    output.WriteStringU16(kv.Key, ushort.MaxValue, Encoding.ASCII, endian);
                    output.WriteValueU32(kv.Value, endian);
                }
            }

            output.WriteStringU16(this._SkinName, 32, Encoding.ASCII, endian);

            if (version >= 221 && version <= 231)
            {
                output.WriteValueU32(this._Unknown2, endian);
            }

            output.WriteValueS32(this._Links.Count, endian);
            foreach (var link in this._Links)
            {
                output.WriteValueS32(link, endian);
            }
        }

        public class ArrayElement : SerializableArrayElement<ClutterElement>
        {
        }
    }
}
