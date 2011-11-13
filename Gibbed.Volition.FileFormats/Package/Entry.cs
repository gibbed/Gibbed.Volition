using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gibbed.IO;
using System.IO;

namespace Gibbed.Volition.FileFormats.Package
{
    public class Entry : IPackageEntry
    {
        public string Name { get; set; }
        public uint Offset { get; set; }
        public uint UncompressedSize { get; set; }
        public uint CompressedSize { get; set; }
    }
}
