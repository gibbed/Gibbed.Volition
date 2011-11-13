using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gibbed.Volition.FileFormats.Package
{
    [Flags]
    public enum HeaderFlags : uint
    {
        None = 0,
        Compressed = 1 << 0,
        Condensed = 1 << 1,
        Unknown11 = 1 << 11,
        Unknown12 = 1 << 12,
        Unknown14 = 1 << 14,
    }
}
