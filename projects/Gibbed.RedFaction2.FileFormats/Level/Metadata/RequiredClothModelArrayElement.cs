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

namespace Gibbed.RedFaction2.FileFormats.Level.Metadata
{
    public class RequiredClothModelArrayElement : IElement
    {
        private readonly List<Item> _Items;

        public RequiredClothModelArrayElement()
        {
            this._Items = new List<Item>();
        }

        public List<Item> Items
        {
            get { return this._Items; }
        }

        public void Read(Stream input, uint version, Endian endian)
        {
            var count = input.ReadValueU32(endian);
            var items = new List<Item>();
            for (uint i = 0; i < count; i++)
            {
                var unknown1 = input.ReadStringU16(64, Encoding.ASCII, endian);
                var unknown2 = version >= 128 ? input.ReadStringU16(128, Encoding.ASCII, endian) : null;
                items.Add(new Item()
                {
                    Unknown1 = unknown1,
                    Unknown2 = unknown2,
                });
            }
            input.Seek(4 * count, SeekOrigin.Current);
            this._Items.Clear();
            this._Items.AddRange(items);
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            output.WriteValueS32(this._Items.Count, endian);
            foreach (var item in this._Items)
            {
                output.WriteStringU16(item.Unknown1, 64, Encoding.ASCII, endian);
                if (version >= 128)
                {
                    output.WriteStringU16(item.Unknown2, 128, Encoding.ASCII, endian);
                }
            }
            for (int i = 0; i < this._Items.Count; i++)
            {
                output.WriteValueU32(0, endian);
            }
        }

        public void ImportJson(JsonReader reader)
        {
            var serializer = new JsonSerializer();
            var items = serializer.Deserialize<List<Item>>(reader);
            this._Items.Clear();
            this._Items.AddRange(items);
        }

        public void ExportJson(JsonWriter writer)
        {
            var serializer = new JsonSerializer();
            serializer.Serialize(writer, this._Items);
        }

        [JsonObject(MemberSerialization.OptIn)]
        public struct Item
        {
            [JsonProperty("u1")]
            public string Unknown1;

            [JsonProperty("u2")]
            public string Unknown2;
        }
    }
}
