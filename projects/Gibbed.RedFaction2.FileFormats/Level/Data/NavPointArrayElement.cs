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
using System.Collections.Generic;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    public class NavPointArrayElement : IElement
    {
        #region Fields
        private readonly List<Item> _Items;
        #endregion

        public NavPointArrayElement()
        {
            this._Items = new List<Item>();
        }

        #region Properties
        public List<Item> Items
        {
            get { return this._Items; }
        }
        #endregion

        public void Read(Stream input, uint version, Endian endian)
        {
            var count = input.ReadValueU32(endian);

            var items = new Item[count];

            for (int i = 0; i < count; i++)
            {
                Item item;
                item.Element = new NavPointElement();
                item.Unknown11 = new List<uint>();
                item.Element.Read(input, version, endian);
                items[i] = item;
            }

            for (int i = 0; i < count; i++)
            {
                var item = items[i];
                var unknown11Count = input.ReadValueU8();
                for (uint j = 0; j < unknown11Count; j++)
                {
                    item.Unknown11.Add(input.ReadValueU32(endian));
                }
            }

            this._Items.Clear();
            this._Items.AddRange(items);
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
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
            [JsonProperty("element")]
            public NavPointElement Element;

            [JsonProperty("__u11")]
            public List<uint> Unknown11;
        }
    }
}
