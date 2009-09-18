using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats.Vint
{
    public class FloatProperty : Property
    {
        public float Value;

        public override string Tag
        {
            get { return "float"; }
        }

        public override void Deserialize(Stream stream, VintFile vint)
        {
            this.Value = stream.ReadValueF32();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
