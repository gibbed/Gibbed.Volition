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

// VPP version 4
// Used by: Saints Row 2

namespace Gibbed.Volition.FileFormats.Packages
{
    internal class PackageFile4 : IPackageFile
    {
        public List<PackageEntry> Entries { get; set; }
        public bool IsSolid { get; set; }
        public long SolidOffset { get; set; }
        public int SolidUncompressedSize { get; set; }
        public int SolidCompressedSize { get; set; }

        public PackageFile4()
        {
            this.Entries = new List<PackageEntry>();
        }

        public void Deserialize(Stream input, bool littleEndian)
        {
            var buffer = new byte[384];
            if (input.ReadAligned(buffer, 0, 384, 2048) != 384)
            {
                throw new FormatException("failed to read header version 4");
            }

            var header = buffer.ToStructure<Structures.PackageHeader4>();

            if (littleEndian == false)
            {
                header = header.Swap();
            }

            if (header.Magic != 0x51890ACE)
            {
                throw new FormatException("not a package file");
            }

            if (header.Version != 4)
            {
                throw new FormatException("unsupported package version");
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

            // Extensions
            var extensionsData = input.ReadToMemoryStream(header.ExtensionsSize.Align(2048));
            if (extensionsData.Length != header.ExtensionsSize.Align(2048))
            {
                throw new FormatException("failed to read extensions index");
            }

            long baseOffset = input.Position;

            this.Entries.Clear();
            for (int i = 0; i < header.IndexCount; i++)
            {
                var entry = new PackageEntry();

                int offset = i * 28; // Each index entry is 28 bytes long

                var index = indexBuffer.ToStructure<Structures.PackageIndex4>(offset);

                if (littleEndian == false)
                {
                    index = index.Swap();
                }

                if (index.Unknown08 != 0 || index.Unknown1C != 0)
                {
                    throw new FormatException("unexpected index entry value");
                }

                if ((header.Flags & PackageFlags.Compressed) != PackageFlags.Compressed && index.CompressedSize != -1)
                {
                    throw new FormatException("index entry with a compressed size when not compressed");
                }

                namesData.Seek(index.NameOffset, SeekOrigin.Begin);
                entry.Name = namesData.ReadStringZ(Encoding.ASCII);
                extensionsData.Seek(index.ExtensionOffset, SeekOrigin.Begin);
                entry.Name += "." + extensionsData.ReadStringZ(Encoding.ASCII);

                entry.Offset = index.Offset;
                entry.CompressedSize = index.CompressedSize;
                entry.UncompressedSize = index.UncompressedSize;

                // package is compressed with zlib, offsets are not correct, fix 'em
                if ((header.Flags & PackageFlags.Compressed) == PackageFlags.Compressed)
                {
                    entry.Offset = baseOffset;
                    baseOffset += entry.CompressedSize.Align(2048);
                    entry.CompressionType = Packages.PackageCompressionType.Zlib;
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

            /*
            MemoryStream memory;

            PackageHeader4 header = new PackageHeader4();
            header.Magic = 0x51890ACE;
            header.Version = 4; // this.Version
            header.CompressedDataSize = 0xFFFFFFFF;
            header.Flags = 2; // I think this flag means 'preload' or something of that sort. Patch has it set at least.

            List<string> names = new List<string>();
            List<string> extensions = new List<string>();
            Dictionary<string, int> nameOffsets = new Dictionary<string, int>();
            Dictionary<string, int> extensionOffsets = new Dictionary<string, int>();

            foreach (PackageEntry entry in this.Entries)
            {
                if (names.Contains(entry.Name) == false)
                {
                    names.Add(entry.Name);
                }

                if (extensions.Contains(entry.Extension) == false)
                {
                    extensions.Add(entry.Extension);
                }
            }

            names.Sort();
            extensions.Sort();

            memory = new MemoryStream();
            foreach (string name in names)
            {
                nameOffsets[name] = (int)memory.Position;
                memory.WriteASCIIZ(name);
            }
            header.NamesSize = (int)memory.Length;
            byte[] namesBuffer = memory.GetBuffer();

            memory = new MemoryStream();
            foreach (string extension in extensions)
            {
                extensionOffsets[extension] = (int)memory.Position;
                memory.WriteASCIIZ(extension);
            }
            header.ExtensionsSize = (int)memory.Length;
            byte[] extensionsBuffer = memory.GetBuffer();

            int totalSize = 0;
            memory = new MemoryStream();
            foreach (PackageEntry entry in this.Entries)
            {
                memory.WriteS32(nameOffsets[entry.Name]);
                memory.WriteS32(extensionOffsets[entry.Extension]);
                memory.WriteU32(entry.Unknown08);
                memory.WriteU32((uint)entry.Offset);
                memory.WriteS32(entry.UncompressedSize);
                memory.WriteS32(entry.CompressedSize);
                memory.WriteU32(entry.Unknown1C);
                totalSize += (int)entry.UncompressedSize.Align(16);
            }
            header.IndexSize = (int)memory.Length;
            byte[] indexBuffer = memory.GetBuffer();

            header.IndexCount = this.Entries.Count;

            header.UncompressedDataSize = totalSize;

            totalSize += 2048; // header
            totalSize += header.IndexSize.Align(2048); // index
            totalSize += header.NamesSize.Align(2048); // names
            totalSize += header.ExtensionsSize.Align(2048); // extensions

            header.PackageSize = totalSize;

            int headerSize = Marshal.SizeOf(typeof(PackageHeader4));
            byte[] headerBuffer = new byte[headerSize];
            GCHandle headerHandle = GCHandle.Alloc(headerBuffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(header, headerHandle.AddrOfPinnedObject(), false);
            headerHandle.Free();

            output.WriteAligned(headerBuffer, 0, headerBuffer.Length, 2048);
            output.WriteAligned(indexBuffer, 0, header.IndexSize, 2048);
            output.WriteAligned(namesBuffer, 0, header.NamesSize, 2048);
            output.WriteAligned(extensionsBuffer, 0, header.ExtensionsSize, 2048);
            */
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
