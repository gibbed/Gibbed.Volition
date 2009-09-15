using System;

namespace Gibbed.Volition.FileFormats.Packages
{
    [Flags]
    internal enum PackageFlags : uint
    {
        None = 0,
        Compressed = 1,
        Solid = 2,
    }
}
