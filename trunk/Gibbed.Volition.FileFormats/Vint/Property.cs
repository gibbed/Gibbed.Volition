using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Gibbed.Volition.FileFormats.Vint
{
    public abstract class Property
    {
        public abstract string Tag { get; }
        public abstract void Deserialize(Stream stream, VintFile vint);
    }
}
