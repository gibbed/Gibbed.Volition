/* Copyright (c) 2011 Rick (rick 'at' gibbed 'dot' us)
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
using Gibbed.IO;

namespace Gibbed.Volition.FileFormats.Interface
{
    public class Object
    {
        public string Name;
        public string Type;

        public PropertyList Baseline = null;
        public Dictionary<string, PropertyList> Overrides
            = new Dictionary<string, PropertyList>();

        public List<Object> Children
            = new List<Object>();

        internal static IProperty CreateProperty(PropertyType type)
        {
            switch (type)
            {
                case PropertyType.Int: return new IntProperty();
                case PropertyType.Float: return new FloatProperty();
                case PropertyType.Bool: return new BoolProperty();
                case PropertyType.String: return new StringProperty();
                case PropertyType.Color: return new ColorProperty();
                case PropertyType.Vector2F: return new Vector2FProperty();
                default: throw new ArgumentException();
            }
        }

        public void Serialize(Stream output, Endian endian, StringTable strings)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, Endian endian, StringTable strings)
        {
            this.Name = strings.ReadString(input, endian);
            this.Type = strings.ReadString(input, endian);
            
            var childCount = input.ReadValueU16(endian);

            var unknown08 = input.ReadValueU8();
            var baselineOffset = input.ReadValueU32(endian);
            var overrideCount = input.ReadValueU8();

            if (overrideCount > 0)
            {
                var infos = new List<KeyValuePair<string, uint>>();
                for (byte i = 0; i < overrideCount; i++)
                {
                    var name = strings.ReadString(input, endian);
                    var offset = input.ReadValueU32(endian);

                    infos.Add(new KeyValuePair<string, uint>(
                        name, offset));
                }

                foreach (var info in infos)
                {
                    if (input.Position != info.Value)
                    {
                        throw new FormatException();
                    }

                    if (this.Overrides.ContainsKey(info.Key) == true)
                    {
                        throw new FormatException();
                    }

                    var OVERRIDE = new PropertyList();
                    OVERRIDE.Deserialize(input, endian, strings);
                    this.Overrides.Add(info.Key, OVERRIDE);
                }
            }

            if (baselineOffset != input.Position)
            {
                throw new FormatException();
            }

            this.Baseline = new PropertyList();
            this.Baseline.Deserialize(input, endian, strings);

            for (ushort i = 0; i < childCount; i++)
            {
                var element = new Object();
                element.Deserialize(input, endian, strings);
                this.Children.Add(element);
            }
        }
    }
}
