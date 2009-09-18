using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats.Vint
{
    public class StringProperty : Property
    {
        public string Value;

        public override string Tag
        {
            get { return "string"; }
        }

        public override void Deserialize(Stream stream, VintFile vint)
        {
            this.Value = vint.Strings[stream.ReadValueS32()];
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
