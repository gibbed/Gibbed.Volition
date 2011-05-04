﻿/* Copyright (c) 2011 Rick (rick 'at' gibbed 'dot' us)
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

        public int UncompressedDataSize { get; set; }
        public int CompressedDataSize { get; set; }
        public int PackageSize { get; set; }

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

            foreach (var entry in this.Entries)
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

            var header = buffer.ToStructure<Structures.PackageHeader3>();

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

            this.PackageSize = header.PackageSize;
            this.CompressedDataSize = header.CompressedDataSize;
            this.UncompressedDataSize = header.UncompressedDataSize;

            this.Entries.Clear();
            for (int i = 0; i < header.IndexCount; i++)
            {
                var entry = new PackageEntry();

                int offset = i * 28; // Each index entry is 28 bytes long

                var index = indexBuffer
                    .ToStructure<Structures.PackageIndex3>(offset);

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

                namesData.Seek(index.NameOffset, SeekOrigin.Begin);
                entry.Name = namesData.ReadStringZ(Encoding.ASCII);

                /*
                Console.WriteLine("{0:X8} {1:X8} {2:X8} {3:X8} {4:X8} {5:X8} {6:X8}  {7}",
                    index.NameOffset,
                    index.Unknown04,
                    index.Offset,
                    index.NameHash,
                    index.UncompressedSize,
                    index.CompressedSize,
                    index.Unknown1C,
                    entry.Name);
                */

                if (index.NameHash != entry.Name.HashVolition())
                {
                    // Console.WriteLine("hash mismatch: {0} != {1} for {2}", index.NameHash, entry.Name.HashVolition(), entry.Name);
                }

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

        public void Serialize(Stream output, bool littleEndian, PackageCompressionType compressionType)
        {
            var namesBuffer = new MemoryStream();
            var indexBuffer = new MemoryStream();

            foreach (var entry in this.Entries)
            {
                var index = new Structures.PackageIndex3();
                index.NameOffset = (int)namesBuffer.Position;
                index.Unknown04 = 0;
                index.Offset = (int)entry.Offset;
                index.NameHash = entry.Name.HashVolition(); // ??
                index.UncompressedSize = entry.UncompressedSize;
                index.CompressedSize = entry.CompressedSize;
                index.Unknown1C = 0;

                if (littleEndian == false)
                {
                    index.Swap();
                }

                indexBuffer.WriteStructure<Structures.PackageIndex3>(index);
                namesBuffer.WriteString(entry.Name, Encoding.ASCII);
            }

            var header = new Structures.PackageHeader3();
            header.Magic = 0x51890ACE;
            header.Version = 3;
            header.String1 = "         Created using      Gibbed's     Volition Tools ";
            header.String2 = "           Read the       Foundation     Novels from       Asimov.       I liked them. ";

            header.Flags = PackageFlags.None;

            if (compressionType == PackageCompressionType.Zlib ||
                compressionType == PackageCompressionType.SolidZlib)
            {
                header.Flags |= PackageFlags.Compressed;
            }

            if (compressionType == PackageCompressionType.SolidZlib)
            {
                header.Flags |= PackageFlags.Solid;
            }

            header.IndexCount = this.Entries.Count;
            header.PackageSize = this.PackageSize;
            header.IndexSize = (int)indexBuffer.Length;
            header.NamesSize = (int)namesBuffer.Length;
            header.UncompressedDataSize = this.UncompressedDataSize;

            if (compressionType == PackageCompressionType.None)
            {
                header.CompressedDataSize = -1;
            }
            else
            {
                header.CompressedDataSize = this.CompressedDataSize;
            }

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