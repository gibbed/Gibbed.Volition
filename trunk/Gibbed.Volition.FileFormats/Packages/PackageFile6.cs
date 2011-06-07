/* Copyright (c) 2011 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.Helpers;

// VPP version 6
// Used by: Red Faction Armageddon

namespace Gibbed.Volition.FileFormats.Packages
{
    internal class PackageFile6 : IPackageFile
    {
        public List<PackageEntry> Entries { get; set; }
        public bool IsSolid { get; set; }
        public long SolidOffset { get; set; }
        public int SolidUncompressedSize { get; set; }
        public int SolidCompressedSize { get; set; }

        public PackageFile6()
        {
            this.Entries = new List<PackageEntry>();
        }

        public void Deserialize(Stream input, bool littleEndian)
        {
            var buffer = new byte[384];
            if (input.ReadAligned(buffer, 0, 384, 2048) != 384)
            {
                throw new FormatException("failed to read header");
            }

            var header = buffer.ToStructure<Structures.PackageHeader6>();
            if (littleEndian == false)
            {
                header = header.Swap();
            }

            // File Index
            var indexBuffer = new byte[header.IndexSize];
            if (input.ReadAligned(indexBuffer, 0, header.IndexSize, 2048) != header.IndexSize)
            {
                throw new FormatException("failed to read file index");
            }

            // Names
            var namesData = input.ReadToMemoryStream(header.NamesSize.Align(2048));
            if (namesData.Length != header.NamesSize.Align(2048))
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
                var entry = new PackageEntry();

                int offset = i * 24; // Each index entry is 24 bytes long

                var index = indexBuffer
                    .ToStructure<Structures.PackageIndex6>(offset);

                if (littleEndian == false)
                {
                    index = index.Swap();
                }

                if (index.Unknown04 != 0 || index.Unknown18 != 0)
                {
                    throw new FormatException("unexpected index entry value");
                }

                if ((header.Flags & PackageFlags.Compressed) != PackageFlags.Compressed &&
                    index.CompressedSize != -1)
                {
                    throw new FormatException("index entry with a compressed size when not compressed");
                }

                namesData.Seek(index.NameOffset, SeekOrigin.Begin);
                entry.Name = namesData.ReadStringZ(Encoding.ASCII);

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

        public void Serialize(Stream output, bool littleEndian, Packages.PackageCompressionType compressionType)
        {
            throw new NotImplementedException();
        }

        public int EstimateHeaderSize()
        {
            throw new NotImplementedException();
        }

        public int UncompressedDataSize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int CompressedDataSize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int PackageSize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
