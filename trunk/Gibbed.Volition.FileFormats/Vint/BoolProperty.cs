using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats.Vint
{
    public class BoolProperty : Property
    {
        public bool Value;

        public override string Tag
        {
            get { return "bool"; }
        }

        public override void Deserialize(Stream stream, VintFile vint)
        {
            this.Value = stream.ReadValueBoolean();
        }

        public override string ToString()
        {
            return this.Value.ToString().ToLowerInvariant();
        }
    }
}
