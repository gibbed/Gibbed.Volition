using System;

namespace Gibbed.Volition.FileFormats.Packages
{
    [Flags]
    internal enum PackageFlags : uint
    {
        Compressed = 1,
        Solid = 2,
    }
}
