using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats.Vint
{
    public class IntProperty : Property
    {
        public Int32 Value;

        public override string Tag
        {
            get { return "int"; }
        }

        public override void Deserialize(Stream stream, VintFile vint)
        {
            this.Value = stream.ReadValueS32();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
