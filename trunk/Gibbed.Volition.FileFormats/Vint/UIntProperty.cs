using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats.Vint
{
    public class UIntProperty : Property
    {
        public UInt32 Value;

        public override string Tag
        {
            get { return "uint"; }
        }

        public override void Deserialize(Stream stream, VintFile vint)
        {
            this.Value = stream.ReadValueU32();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
