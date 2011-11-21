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
using Gibbed.IO;

/* VPP version 4
 * 
 * Used by:
 *   Saints Row 2
 */

namespace Gibbed.Volition.FileFormats
{
    public class PackageFileV4 : IPackageFile, IPackageFile<Package.Entry>
    {
        public Endian Endian { get; set; }
        public Package.HeaderFlags Flags { get; set; }
        public uint TotalSize { get; set; }
        public uint UncompressedSize { get; set; }
        public uint CompressedSize { get; set; }
        public List<Package.Entry> Entries { get; private set; }
        public IEnumerable<IPackageEntry> Directory
        {
            get { return this.Entries; }
        }

        public long DataOffset { get; set; }

        public PackageFileV4()
        {
            this.Flags = Package.HeaderFlags.None;
            this.Entries = new List<Package.Entry>();
        }

        public int EstimateHeaderSize()
        {
            int totalSize = 0;

            totalSize += 2048; // header
            totalSize += (this.Entries.Count * 28).Align(2048);

            int namesSize = 0;
            foreach (var entry in this.Entries)
            {
                namesSize += Path.GetFileNameWithoutExtension(entry.Name).Length + 1;
            }
            totalSize += namesSize.Align(2048);

            int extensionsSize = 0;
            foreach (var entry in this.Entries)
            {
                var extension = Path.GetExtension(entry.Name);
                if (extension != null && extension.StartsWith(".") == true)
                {
                    extension = extension.Substring(1);
                }

                extensionsSize += extension.Length + 1;
            }
            totalSize += extensionsSize.Align(2048);

            return totalSize;
        }

        public void Serialize(Stream output)
        {
            var endian = this.Endian;

            var names = new MemoryStream();
            var extensions = new MemoryStream();
            var directory = new MemoryStream();

            var nameOffsets = new Dictionary<string, uint>();
            var extensionOffsets = new Dictionary<string, uint>();

            foreach (var entry in this.Entries)
            {
                var name = Path.GetFileNameWithoutExtension(entry.Name);
                
                var extension = Path.GetExtension(entry.Name);
                if (extension != null && extension.StartsWith(".") == true)
                {
                    extension = extension.Substring(1);
                }

                uint nameOffset;
                if (nameOffsets.ContainsKey(name) == false)
                {
                    nameOffset = (uint)names.Position;
                    nameOffsets.Add(name, nameOffset);
                    names.WriteStringZ(name, Encoding.ASCII);
                }
                else
                {
                    nameOffset = nameOffsets[name];
                }

                uint extensionOffset;
                if (extensionOffsets.ContainsKey(extension) == false)
                {
                    extensionOffset = (uint)extensions.Position;
                    extensionOffsets.Add(extension, extensionOffset);
                    extensions.WriteStringZ(extension, Encoding.ASCII);
                }
                else
                {
                    extensionOffset = extensionOffsets[extension];
                }

                directory.WriteValueU32(nameOffset, endian);
                directory.WriteValueU32(extensionOffset, endian);
                directory.WriteValueU32(0, endian);
                directory.WriteValueU32(entry.Offset, endian);
                directory.WriteValueU32(entry.UncompressedSize, endian);
                directory.WriteValueU32(entry.CompressedSize, endian);
                directory.WriteValueU32(0, endian);
                names.WriteStringZ(entry.Name, Encoding.ASCII);
            }

            var header = new Package.HeaderV4();
            header.Name = "         Created using      Gibbed's     Volition Tools ";
            header.Path = "           Read the       Foundation     Novels from       Asimov.       I liked them. ";

            header.Flags = this.Flags;

            header.DirectoryCount = (uint)this.Entries.Count;
            header.PackageSize = this.TotalSize;
            header.DirectorySize = (uint)directory.Length;
            header.NamesSize = (uint)names.Length;
            header.UncompressedSize = this.UncompressedSize;
            header.CompressedSize = (this.Flags & Package.HeaderFlags.Compressed) != 0 ?
                this.CompressedSize : 0xFFFFFFFF;

            directory.Seek(0, SeekOrigin.Begin);
            directory.SetLength(directory.Length.Align(2048));

            names.Seek(0, SeekOrigin.Begin);
            names.SetLength(names.Length.Align(2048));

            output.WriteValueU32(0x51890ACE, endian);
            output.WriteValueU32(4, endian);

            header.Serialize(output, endian);
            output.Seek(2048, SeekOrigin.Begin);
            output.WriteFromStream(directory, directory.Length);
            output.WriteFromStream(names, names.Length);
        }

        public void Deserialize(Stream input)
        {
            Endian endian;
            Package.HeaderV4 header;

            using (var data = input.ReadToMemoryStream(2048))
            {
                var magic = data.ReadValueU32(Endian.Little);
                if (magic != 0x51890ACE &&
                    magic.Swap() != 0x51890ACE)
                {
                    throw new FormatException("not a package file");
                }
                endian = magic == 0x51890ACE ? Endian.Little : Endian.Big;

                var version = data.ReadValueU32(endian);
                if (version != 4)
                {
                    throw new FormatException("unexpected package version (expected 4)");
                }

                header = new Package.HeaderV4();
                header.Deserialize(data, endian);
            }

            this.Entries.Clear();
            using (var directory = input.ReadToMemoryStream(header.DirectorySize.Align(2048)))
            {
                using (var names = input.ReadToMemoryStream(header.NamesSize.Align(2048)))
                {
                    using (var extensions = input.ReadToMemoryStream(header.ExtensionsSize.Align(2048)))
                    {
                        for (int i = 0; i < header.DirectoryCount; i++)
                        {
                            var nameOffset = directory.ReadValueU32(endian);
                            var extensionOffset = directory.ReadValueU32(endian);
                            directory.Seek(4, SeekOrigin.Current); // runtime offset
                            var offset = directory.ReadValueU32(endian);
                            var uncompressedSize = directory.ReadValueU32(endian);
                            var compressedSize = directory.ReadValueU32(endian);
                            directory.Seek(4, SeekOrigin.Current); // package pointer

                            names.Seek(nameOffset, SeekOrigin.Begin);
                            extensions.Seek(extensionOffset, SeekOrigin.Begin);

                            var name = names.ReadStringZ(Encoding.ASCII);
                            name += "." + extensions.ReadStringZ(Encoding.ASCII);

                            this.Entries.Add(new Package.Entry()
                            {
                                Name = name,
                                Offset = offset,
                                UncompressedSize = uncompressedSize,
                                CompressedSize = compressedSize,
                            });
                        }
                    }
                }
            }

            this.Endian = endian;
            this.Flags = header.Flags & Package.HeaderFlags.Compressed;
            this.UncompressedSize = header.UncompressedSize;
            this.CompressedSize = header.CompressedSize;
            this.DataOffset = input.Position;
        }
    }
}
