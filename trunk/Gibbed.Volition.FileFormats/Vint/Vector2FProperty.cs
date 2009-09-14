using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gibbed.Helpers;

namespace Gibbed.Volition.FileFormats.Vint
{
    public class Vector2FProperty : Property
    {
        public float X;
        public float Y;

        public override string Tag
        {
            get { return "vector2f"; }
        }

        public override void Read(Stream stream, VintFile vint)
        {
            this.X = stream.ReadValueF32();
            this.Y = stream.ReadValueF32();
        }

        public override string ToString()
        {
            return
                this.X.ToString() + "," +
                this.Y.ToString();
        }
    }
}
