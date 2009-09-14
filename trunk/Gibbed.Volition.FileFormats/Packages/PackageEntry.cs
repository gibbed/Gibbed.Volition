namespace Gibbed.Volition.FileFormats.Packages
{
    internal class PackageEntry
    {
        public string Name;
        public long Offset;
        public int UncompressedSize;
        public int CompressedSize;
        public PackageCompressionType CompressionType;
    }
}
