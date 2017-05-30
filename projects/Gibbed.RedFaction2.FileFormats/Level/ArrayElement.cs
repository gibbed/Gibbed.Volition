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

namespace Gibbed.RedFaction2.FileFormats.Level
{
    public abstract class ArrayElement<T> : IElement
    {
        private readonly List<T> _Items;

        public ArrayElement()
        {
            this._Items = new List<T>();
        }

        public List<T> Items
        {
            get { return this._Items; }
        }

        public virtual void Read(Stream input, uint version, Endian endian)
        {
            var count = input.ReadValueU32(endian);
            var items = new List<T>();
            for (uint i = 0; i < count; i++)
            {
                items.Add(this.ReadItem(input, version, endian));
            }
            this._Items.Clear();
            this._Items.AddRange(items);
        }

        protected abstract T ReadItem(Stream input, uint version, Endian endian);

        public virtual void Write(Stream output, uint version, Endian endian)
        {
            output.WriteValueS32(this._Items.Count, endian);
            foreach (var item in this._Items)
            {
                this.WriteItem(output, item, version, endian);
            }
        }

        protected abstract void WriteItem(Stream output, T instance, uint version, Endian endian);

        public void ImportJson(JsonReader reader)
        {
            var serializer = new JsonSerializer();
            var items = serializer.Deserialize<List<T>>(reader);
            this._Items.Clear();
            this._Items.AddRange(items);
        }

        public void ExportJson(JsonWriter writer)
        {
            var serializer = new JsonSerializer();
            serializer.Serialize(writer, this._Items);
        }
    }
}
