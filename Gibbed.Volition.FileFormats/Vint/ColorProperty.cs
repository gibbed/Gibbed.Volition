using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats.Vint
{
    public class ColorProperty : Property
    {
        public float R;
        public float G;
        public float B;

        public override string Tag
        {
            get { return "color"; }
        }

        public override void Deserialize(Stream stream, VintFile vint)
        {
            this.R = stream.ReadValueF32();
            this.G = stream.ReadValueF32();
            this.B = stream.ReadValueF32();
        }

        public override string ToString()
        {
            return
                this.R.ToString() + "," +
                this.G.ToString() + "," +
                this.B.ToString();
        }
    }
}
