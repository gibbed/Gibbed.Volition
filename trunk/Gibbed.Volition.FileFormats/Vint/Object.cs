using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats.Vint
{
    public class Object
    {
        private Property GetProperty(Stream stream, byte propertyType)
        {
            Property value;
            switch (propertyType)
            {
                case 1:
                {
                    value = new IntProperty();
                    break;
                }

                case 2:
                {
                    value = new UIntProperty();
                    break;
                }

                case 3:
                {
                    value = new FloatProperty();
                    break;
                }

                case 4:
                {
                    value = new StringProperty();
                    break;
                }

                case 5:
                {
                    value = new BoolProperty();
                    break;
                }

                case 6:
                {
                    value = new ColorProperty();
                    break;
                }

                case 7:
                {
                    value = new Vector2FProperty();
                    break;
                }

                // 8 = callback
                // 9 = bitmap
                // 10 = font
                // 11 = sound
                // 12 = enum
                // 13 = variable

                default:
                {
                    throw new FormatException("unknown property type");
                }
            }

            return value;
        }

        public string Name;
        public string Type;

        public Dictionary<string, Property> Baseline = new Dictionary<string, Property>();
        public Dictionary<string, Dictionary<string, Property>> Overrides = new Dictionary<string, Dictionary<string, Property>>();
        public List<Object> Children = new List<Object>();

        public override string ToString()
        {
            if (this.Name == null || this.Name.Length == 0)
            {
                return base.ToString();
            }

            return this.Name;
        }

        public void Deserialize(Stream stream, VintFile vint)
        {
            this.Name = vint.Strings[stream.ReadValueS32()];
            this.Type = vint.Strings[stream.ReadValueS32()];
            UInt16 childCount = stream.ReadValueU16();
            
            this.Overrides.Clear();

            byte unk4 = stream.ReadValueU8();
            UInt32 unk5 = stream.ReadValueU32();

            byte overrideCount = stream.ReadValueU8();
            if (overrideCount > 0)
            {
                string[] overrideNames = new string[overrideCount];

                for (byte i = 0; i < overrideCount; i++)
                {
                    overrideNames[i] = vint.Strings[stream.ReadValueS32()];
                    /*UInt32 overrideOffset = */stream.ReadValueU32();
                }

                for (byte i = 0; i < overrideCount; i++)
                {
                    string overrideName = overrideNames[i];

                    if (this.Overrides.ContainsKey(overrideName))
                    {
                        throw new Exception("duplicate override name");
                    }

                    this.Overrides[overrideName] = new Dictionary<string, Property>();
                    while (true)
                    {
                        byte propertyType = stream.ReadValueU8();
                        if (propertyType == 0)
                        {
                            break;
                        }

                        UInt32 hash = stream.ReadValueU32();
                        string propertyName = PropertyNames.Lookup(hash);
                        if (this.Overrides[overrideName].ContainsKey(propertyName))
                        {
                            throw new Exception("duplicate override property name");
                        }
                        this.Overrides[overrideName][propertyName] = this.GetProperty(stream, propertyType);
                        this.Overrides[overrideName][propertyName].Deserialize(stream, vint);
                    }
                }
            }

            this.Baseline.Clear();
            while (true)
            {
                byte propertyType = stream.ReadValueU8();
                if (propertyType == 0)
                {
                    break;
                }

                UInt32 hash = stream.ReadValueU32();
                string propertyName = PropertyNames.Lookup(hash);
                if (this.Baseline.ContainsKey(propertyName))
                {
                    throw new Exception("duplicate baseline property name");
                }
                this.Baseline[propertyName] = this.GetProperty(stream, propertyType);
                this.Baseline[propertyName].Deserialize(stream, vint);
            }

            for (int i = 0; i < childCount; i++)
            {
                Object child = new Object();
                child.Deserialize(stream, vint);
                this.Children.Add(child);
            }
        }
    }
}
