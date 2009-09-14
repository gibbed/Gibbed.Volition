using System.Collections.Generic;
using System.IO;

namespace Gibbed.Volition.FileFormats
{
    internal interface IPackageFile
    {
        List<Packages.PackageEntry> Entries { get; }
        void Deserialize(Stream input, bool littleEndian);
        void Serialize(Stream output, bool littleEndian);
        bool IsSolid { get; }
        long SolidOffset { get; }
        int SolidUncompressedSize { get; }
        int SolidCompressedSize { get; }
    }
}
