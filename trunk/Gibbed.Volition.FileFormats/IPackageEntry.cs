using System;
using System.IO;
using Gibbed.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gibbed.Volition.FileFormats
{
    public interface IPackageEntry
    {
        string Name { get; set; }
        uint Offset { get; set; }
        uint UncompressedSize { get; set; }
        uint CompressedSize { get; set; }
    }
}
