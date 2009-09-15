using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Helpers;

// VPP version 3
// Used by: Red Faction Guerrilla, Saints Row

namespace Gibbed.Volition.FileFormats.Packages
{
    internal class PackageFile3 : IPackageFile
    {
        public List<PackageEntry> Entries { get; set; }
        public bool IsSolid { get; set; }
        public long SolidOffset { get; set; }
        public int SolidUncompressedSize { get; set; }
        public int SolidCompressedSize { get; set; }

        public PackageFile3()
        {
            this.Entries = new List<PackageEntry>();
        }

        public int EstimateHeaderSize()
        {
            int totalSize = 0;
            
            totalSize += 2048; // header
            totalSize += (this.Entries.Count * 28).Align(2048);

            int namesSize = 0;
            foreach (PackageEntry entry in this.Entries)
            {
                namesSize += entry.Name.Length + 1;
            }

            totalSize += namesSize.Align(2048);
            return totalSize;
        }

        public int EstimateTotalSize()
        {
            int totalSize = this.EstimateHeaderSize();

            if (this.IsSolid)
            {
                throw new InvalidOperationException();
            }

            foreach (PackageEntry entry in this.Entries)
            {
                if (entry.CompressionType == PackageCompressionType.None)
                {
                    totalSize += entry.UncompressedSize.Align(16);
                }
                else if (entry.CompressionType == PackageCompressionType.Zlib)
                {
                    totalSize += entry.CompressedSize.Align(16);
                }
            }

            return totalSize;
        }

        public void Deserialize(Stream input, bool littleEndian)
        {
            byte[] buffer = new byte[384];
            if (input.ReadAligned(buffer, 0, 384, 2048) != 384)
            {
                throw new FormatException("failed to read header version 3");
            }

            Structures.PackageHeader3 header = buffer.ToStructure<Structures.PackageHeader3>();

            if (littleEndian == false)
            {
                header = header.Swap();
            }

            // File Index
            byte[] indexBuffer;
            indexBuffer = new byte[header.IndexSize];
            if (input.ReadAligned(indexBuffer, 0, header.IndexSize, 2048) != header.IndexSize)
            {
                throw new FormatException("failed to read file index");
            }

            // Names
            byte[] namesBuffer;
            namesBuffer = new byte[header.NamesSize];
            if (input.ReadAligned(namesBuffer, 0, header.NamesSize, 2048) != header.NamesSize)
            {
                throw new FormatException("failed to read name index");
            }

            long baseOffset = input.Position;

            if ((header.Flags & PackageFlags.Solid) == PackageFlags.Solid)
            {
                this.IsSolid = true;
                this.SolidOffset = baseOffset;
                this.SolidUncompressedSize = header.UncompressedDataSize;
                this.SolidCompressedSize = header.CompressedDataSize;
            }

            this.Entries.Clear();
            for (int i = 0; i < header.IndexCount; i++)
            {
                PackageEntry entry = new PackageEntry();

                int offset = i * 28; // Each index entry is 28 bytes long

                Structures.PackageIndex3 index = indexBuffer.ToStructure<Structures.PackageIndex3>(offset);

                if (littleEndian == false)
                {
                    index = index.Swap();
                }

                if (index.Unknown04 != 0 || index.Unknown1C != 0)
                {
                    throw new FormatException("unexpected index entry value");
                }

                if ((header.Flags & PackageFlags.Compressed) != PackageFlags.Compressed && index.CompressedSize != -1)
                {
                    throw new FormatException("index entry with a compressed size when not compressed");
                }

                entry.Name = namesBuffer.ToStringASCIIZ(index.NameOffset);

                entry.Offset = index.Offset;
                entry.CompressedSize = index.CompressedSize;
                entry.UncompressedSize = index.UncompressedSize;

                // package is compressed with zlib, offsets are not correct, fix 'em
                if ((header.Flags & PackageFlags.Compressed) == PackageFlags.Compressed)
                {
                    // solid compression (one zlib block)
                    if ((header.Flags & PackageFlags.Solid) == PackageFlags.Solid)
                    {
                        entry.CompressionType = Packages.PackageCompressionType.SolidZlib;
                    }
                    else
                    {
                        entry.Offset = baseOffset;
                        baseOffset += entry.CompressedSize.Align(2048);
                        entry.CompressionType = Packages.PackageCompressionType.Zlib;
                    }
                }
                else
                {
                    entry.Offset += baseOffset;
                    entry.CompressionType = Packages.PackageCompressionType.None;
                }

                this.Entries.Add(entry);
            }
        }

        public void Serialize(Stream output, bool littleEndian)
        {
            MemoryStream namesBuffer = new MemoryStream();
            MemoryStream indexBuffer = new MemoryStream();

            int totalUncompressedSize = 0;
            int totalCompressedSize = 0;
            int totalPackageSize = 0;

            foreach (PackageEntry entry in this.Entries)
            {
                totalUncompressedSize += entry.UncompressedSize;

                if (entry.CompressedSize != -1)
                {
                    totalCompressedSize += entry.CompressedSize;
                    totalPackageSize += entry.CompressedSize.Align(16);
                }
                else
                {
                    totalPackageSize += entry.UncompressedSize.Align(16);
                }

                Structures.PackageIndex3 index = new Structures.PackageIndex3();
                index.NameOffset = (int)namesBuffer.Position;
                index.Unknown04 = 0;
                index.Offset = (int)entry.Offset;
                index.Unknown0C = 0; // ??
                index.UncompressedSize = entry.UncompressedSize;
                index.CompressedSize = entry.CompressedSize;
                index.Unknown1C = 0;

                if (littleEndian == false)
                {
                    index.Swap();
                }

                indexBuffer.WriteStructure<Structures.PackageIndex3>(index);
                namesBuffer.WriteStringASCIIZ(entry.Name);
            }

            Structures.PackageHeader3 header = new Structures.PackageHeader3();
            header.Magic = 0x51890ACE;
            header.Version = 3;
            header.Flags = PackageFlags.None;
            header.IndexCount = this.Entries.Count;
            header.PackageSize =
                2048 +
                (int)indexBuffer.Length.Align(2048) +
                (int)namesBuffer.Length.Align(2048) +
                totalPackageSize;
            header.IndexSize = (int)indexBuffer.Length;
            header.NamesSize = (int)namesBuffer.Length;

            if (littleEndian == false)
            {
                header.Swap();
            }

            indexBuffer.Seek(0, SeekOrigin.Begin);
            indexBuffer.SetLength(indexBuffer.Length.Align(2048));

            namesBuffer.Seek(0, SeekOrigin.Begin);
            namesBuffer.SetLength(namesBuffer.Length.Align(2048));

            output.WriteStructure<Structures.PackageHeader3>(header, 2048);
            output.WriteFromStream(indexBuffer, indexBuffer.Length);
            output.WriteFromStream(namesBuffer, namesBuffer.Length);
        }
    }
}
