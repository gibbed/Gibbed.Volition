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
using System.Linq;
using Gibbed.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Gibbed.Volition.FileFormats
{
    public class Package
    {
        public uint Version;
        public bool LittleEndian;

        private bool IsSolid;

        private Stream Stream;

        #region Entry
        private abstract class Entry
        {
            public int Size;
        }

        private class StreamEntry : Entry
        {
            public long Offset;
            public int CompressedSize;
            public Packages.PackageCompressionType CompressionType;
        }

        private class MemoryEntry : Entry
        {
            public byte[] Data;
        }

        private class CopyFromFileEntry : Entry
        {
            public string Path;
        }

        #endregion

        private Dictionary<string, StreamEntry> OriginalEntries = new Dictionary<string, StreamEntry>();
        private Dictionary<string, Entry> Entries = new Dictionary<string, Entry>();
        public ICollection<string> Keys
        {
            get { return this.Entries.Keys; }
        }

        public Package(Stream stream)
            : this(stream, true)
        {
        }

        public Package(Stream stream, bool readExisting)
        {
            if (stream.CanSeek == false || stream.CanRead == false)
            {
                throw new ArgumentException("stream must have seek / read access", "stream");
            }

            this.Stream = stream;

            if (readExisting == true)
            {
                this.Read(stream);
            }
        }

        private void Read(Stream input)
        {
            UInt32 magic = input.ReadValueU32();

            if (magic != 0x51890ACE && magic != 0xCE0A8951)
            {
                throw new FormatException("not a volition package");
            }

            this.LittleEndian = (magic == 0x51890ACE) ? true : false;
            this.Version = input.ReadValueU32(this.LittleEndian);

            if (this.Version != 3 &&
                this.Version != 4 &&
                this.Version != 6)
            {
                throw new FormatException("unsupported volition package version");
            }

            input.Seek(-8, SeekOrigin.Current);

            IPackageFile packageFile = null;

            if (this.Version == 3)
            {
                packageFile = new Packages.PackageFile3();
            }
            else if (this.Version == 4)
            {
                packageFile = new Packages.PackageFile4();
            }
            else if (this.Version == 6)
            {
                packageFile = new Packages.PackageFile6();
            }
            else
            {
                throw new NotSupportedException();
            }

            packageFile.Deserialize(input, this.LittleEndian);

            this.Entries.Clear();
            this.OriginalEntries.Clear();

            if (packageFile.IsSolid == false)
            {
                foreach (Packages.PackageEntry packageEntry in packageFile.Entries)
                {
                    StreamEntry entry = new StreamEntry();
                    entry.Offset = packageEntry.Offset;
                    entry.Size = packageEntry.UncompressedSize;
                    entry.CompressedSize = packageEntry.CompressedSize;
                    entry.CompressionType = packageEntry.CompressionType;

                    if (this.Entries.ContainsKey(packageEntry.Name) == false)
                    {
                        this.Entries.Add(packageEntry.Name, entry);
                    }
                    else
                    {
                        // Saints Row 1 seems to have bugged duplicate entries that point to
                        // different offsets with the same data.
                        this.Entries.Add(packageEntry.Name + "_DUPLICATE_" + packageEntry.Offset.ToString("X8"), entry);
                    }
                }
            }
            else
            {
                this.IsSolid = true;

                byte[] solid = new byte[packageFile.SolidUncompressedSize];

                input.Seek(packageFile.SolidOffset, SeekOrigin.Begin);

                // Decompress solid data
                var zlib = new InflaterInputStream(input);
                if (zlib.Read(solid, 0, solid.Length) != solid.Length)
                {
                    throw new InvalidOperationException("zlib error");
                }

                foreach (Packages.PackageEntry packageEntry in packageFile.Entries)
                {
                    var entry = new MemoryEntry();
                    entry.Data = new byte[packageEntry.UncompressedSize];
                    Array.Copy(solid, (int)packageEntry.Offset, entry.Data, 0, entry.Data.Length);
                    entry.Size = entry.Data.Length;
                    this.Entries.Add(packageEntry.Name, entry);
                }
            }
        }

        private void Write(Stream output)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntry(string name)
        {
            if (this.IsSolid == true)
            {
                throw new NotSupportedException("cannot write to solid packages");
            }
            else if (this.Stream.CanWrite == false)
            {
                throw new NotSupportedException("stream is not writable");
            }
            else if (this.Entries.ContainsKey(name) == false)
            {
                throw new KeyNotFoundException();
            }

            if (this.Entries[name] is StreamEntry)
            {
                this.OriginalEntries[name] = (StreamEntry)this.Entries[name];
            }

            this.Entries.Remove(name);
        }

        public void RenameEntry(string oldName, string newName)
        {
            if (this.IsSolid == true)
            {
                throw new NotSupportedException("cannot write to solid packages");
            }
            else if (this.Stream.CanWrite == false)
            {
                throw new NotSupportedException("stream is not writable");
            }
            else if (this.Entries.ContainsKey(oldName) == false)
            {
                throw new KeyNotFoundException();
            }
            else if (this.Entries.ContainsKey(newName) == true)
            {
                throw new ArgumentException("package already contains the new name", "newName");
            }

            Entry entry = this.Entries[oldName];

            if (entry is StreamEntry)
            {
                this.OriginalEntries[oldName] = (StreamEntry)entry;
            }

            this.Entries.Remove(oldName);
            this.Entries.Add(newName, entry);
        }

        public byte[] GetEntry(string name)
        {
            if (this.Entries.ContainsKey(name) == false)
            {
                throw new KeyNotFoundException();
            }

            if (this.Entries[name] is StreamEntry)
            {
                var entry = (StreamEntry)this.Entries[name];

                this.Stream.Seek(entry.Offset, SeekOrigin.Begin);

                byte[] data = new byte[entry.Size];

                if (entry.CompressionType == Packages.PackageCompressionType.Zlib)
                {
                    var zlib = new InflaterInputStream(this.Stream);
                    
                    int read = zlib.Read(data, 0, data.Length);
                    if (read < 0 || read != data.Length)
                    {
                        throw new InvalidOperationException("zlib error");
                    }
                }
                else if (entry.CompressionType == Packages.PackageCompressionType.None)
                {
                    this.Stream.Read(data, 0, data.Length);
                }
                else
                {
                    throw new InvalidOperationException();
                }

                return data;
            }
            else if (this.Entries[name] is MemoryEntry)
            {
                return (byte[])(((MemoryEntry)this.Entries[name]).Data.Clone());
            }
            else if (this.Entries[name] is CopyFromFileEntry)
            {
                var entry = (CopyFromFileEntry)this.Entries[name];
                byte[] data = new byte[entry.Size];
                Stream input = File.Open(entry.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                input.Read(data, 0, data.Length);
                input.Close();
                return data;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public Stream GetEntryStream(string name)
        {
            return new MemoryStream(this.GetEntry(name));
        }

        public void SetEntry(string name, byte[] data)
        {
            if (this.IsSolid == true)
            {
                throw new NotSupportedException("cannot write to solid packages");
            }
            else if (this.Stream.CanWrite == false)
            {
                throw new NotSupportedException("stream is not writable");
            }

            if (this.Entries.ContainsKey(name) && this.Entries[name] is StreamEntry)
            {
                this.OriginalEntries[name] = (StreamEntry)this.Entries[name];
            }

            MemoryEntry entry = new MemoryEntry();
            entry.Data = (byte[])(data.Clone());
            entry.Size = entry.Data.Length;

            this.Entries[name] = entry;
        }

        public void SetEntry(string name, Stream stream)
        {
            byte[] data = new byte[stream.Length];

            long oldPosition = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(data, 0, data.Length);
            stream.Seek(oldPosition, SeekOrigin.Begin);

            this.SetEntry(name, data);
        }

        public void SetEntry(string name, string path)
        {
            if (this.IsSolid == true)
            {
                throw new NotSupportedException("cannot write to solid packages");
            }
            else if (this.Stream.CanWrite == false)
            {
                throw new NotSupportedException("stream is not writable");
            }

            if (this.Entries.ContainsKey(name) && this.Entries[name] is StreamEntry)
            {
                this.OriginalEntries[name] = (StreamEntry)this.Entries[name];
            }

            CopyFromFileEntry entry = new CopyFromFileEntry();
            entry.Path = path;
            entry.Size = (int)(new FileInfo(path).Length);

            this.Entries[name] = entry;
        }

        public void ExportEntry(string name, Stream output)
        {
            if (this.Entries.ContainsKey(name) == false)
            {
                throw new KeyNotFoundException();
            }

            if (this.Entries[name] is StreamEntry)
            {
                var entry = (StreamEntry)this.Entries[name];

                this.Stream.Seek(entry.Offset, SeekOrigin.Begin);

                if (entry.CompressionType == Packages.PackageCompressionType.Zlib)
                {
                    // this is a hack until I fix zlib handling
                    using (var memory = this.Stream.ReadToMemoryStream(entry.CompressedSize))
                    {
                        var zlib = new InflaterInputStream(memory);
                        output.WriteFromStream(zlib, entry.Size);
                    }
                }
                else if (entry.CompressionType == Packages.PackageCompressionType.None)
                {
                    output.WriteFromStream(this.Stream, entry.Size);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else if (this.Entries[name] is MemoryEntry)
            {
                MemoryEntry entry = (MemoryEntry)this.Entries[name];
                output.Write(entry.Data, 0, entry.Data.Length);
            }
            else if (this.Entries[name] is CopyFromFileEntry)
            {
                CopyFromFileEntry entry = (CopyFromFileEntry)this.Entries[name];
                using (var input = File.Open(entry.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    output.WriteFromStream(input, entry.Size);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Commit(Packages.PackageCompressionType compressionType)
        {
            Stream clean;
            string tempFileName = Path.GetTempFileName();

            tempFileName = Path.GetTempFileName();
            clean = File.Open(tempFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

            IPackageFile packageFile = null;

            if (this.Version == 3)
            {
                packageFile = new Packages.PackageFile3();
            }
            else if (this.Version == 4)
            {
                packageFile = new Packages.PackageFile4();
            }
            else if (this.Version == 6)
            {
                packageFile = new Packages.PackageFile6();
            }
            else
            {
                throw new NotSupportedException();
            }

            foreach (KeyValuePair<string, Entry> kvp in this.Entries)
            {
                var packageEntry = new Packages.PackageEntry();
                packageEntry.Name = kvp.Key;
                packageEntry.CompressedSize = -1;
                packageEntry.UncompressedSize = kvp.Value.Size;
                packageEntry.Offset = 0;
                packageFile.Entries.Add(packageEntry);
            }

            int headerSize = packageFile.EstimateHeaderSize();
            clean.Seek(headerSize, SeekOrigin.Begin);

            int uncompressedDataSize = 0;
            int compressedDataSize = 0;

            if (compressionType == Packages.PackageCompressionType.None)
            {
                long offset = 0;
                foreach (Packages.PackageEntry packageEntry in packageFile.Entries)
                {
                    packageEntry.Offset = offset;

                    this.ExportEntry(packageEntry.Name, clean);

                    int align = packageEntry.UncompressedSize.Align(2048) - packageEntry.UncompressedSize;
                    if (align > 0)
                    {
                        byte[] block = new byte[align];
                        clean.Write(block, 0, (int)align);
                    }

                    offset += packageEntry.UncompressedSize + align;
                    uncompressedDataSize += packageEntry.UncompressedSize + align;
                }
            }
            else if (compressionType == Packages.PackageCompressionType.Zlib)
            {
                long offset = 0;
                foreach (Packages.PackageEntry packageEntry in packageFile.Entries)
                {
                    packageEntry.Offset = offset;

                    byte[] uncompressedData = this.GetEntry(packageEntry.Name);
                    using (var temp = new MemoryStream())
                    {
                        var zlib = new DeflaterOutputStream(temp, new Deflater(Deflater.DEFAULT_COMPRESSION));
                        zlib.Write(uncompressedData, 0, uncompressedData.Length);
                        zlib.Flush();

                        temp.Position = 0;
                        clean.WriteFromStream(temp, temp.Length);
                        packageEntry.CompressedSize = (int)temp.Length;

                        int align = packageEntry.CompressedSize.Align(2048) - packageEntry.CompressedSize;
                        if (align > 0)
                        {
                            byte[] block = new byte[align];
                            clean.Write(block, 0, (int)align);
                        }

                        offset += packageEntry.CompressedSize + align;
                        uncompressedDataSize += packageEntry.UncompressedSize;
                        compressedDataSize += packageEntry.CompressedSize + align;
                    }
                }
            }
            else if (compressionType == Packages.PackageCompressionType.SolidZlib)
            {
                using (var compressed = new MemoryStream())
                {
                    var zlib = new DeflaterOutputStream(compressed, new Deflater(Deflater.DEFAULT_COMPRESSION));

                    long offset = 0;
                    foreach (Packages.PackageEntry packageEntry in packageFile.Entries)
                    {
                        packageEntry.Offset = offset;

                        this.ExportEntry(packageEntry.Name, zlib);

                        int align = packageEntry.UncompressedSize.Align(2048) - packageEntry.UncompressedSize;
                        if (align > 0)
                        {
                            byte[] block = new byte[align];
                            zlib.Write(block, 0, (int)align);
                        }

                        offset += packageEntry.UncompressedSize + align;
                        uncompressedDataSize += packageEntry.UncompressedSize + align;
                    }

                    zlib.Close();

                    compressed.Seek(0, SeekOrigin.Begin);
                    clean.WriteFromStream(compressed, compressed.Length);

                    compressedDataSize = (int)compressed.Length;
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            packageFile.PackageSize = (int)clean.Length;
            packageFile.UncompressedDataSize = uncompressedDataSize;
            packageFile.CompressedDataSize = compressedDataSize;

            clean.Seek(0, SeekOrigin.Begin);
            packageFile.Serialize(clean, this.LittleEndian, compressionType);

            // copy clean to real stream
            {
                this.Stream.Seek(0, SeekOrigin.Begin);
                clean.Seek(0, SeekOrigin.Begin);
                this.Stream.WriteFromStream(clean, clean.Length);
            }

            this.Stream.SetLength(clean.Length);
            clean.Close();

            if (tempFileName != null)
            {
                File.Delete(tempFileName);
            }

            this.Entries.Clear();
            this.OriginalEntries.Clear();

            foreach (Packages.PackageEntry entry in packageFile.Entries)
            {
                this.Entries.Add(entry.Name, new StreamEntry()
                {
                    Offset = entry.Offset,
                    Size = entry.UncompressedSize,
                    CompressedSize = entry.CompressedSize,
                    CompressionType = entry.CompressionType,
                });
            }
        }

        public void Rollback()
        {
            if (this.IsSolid == true)
            {
                throw new NotSupportedException("cannot write to solid packages");
            }
            else if (this.Stream.CanWrite == false)
            {
                throw new NotSupportedException("stream is not writable");
            }

            foreach (KeyValuePair<string, StreamEntry> entry in this.OriginalEntries)
            {
                this.Entries[entry.Key] = entry.Value;
            }

            foreach (KeyValuePair<string, Entry> entry in this.Entries.Where(entry => entry.Value is MemoryEntry).ToList())
            {
                this.Entries.Remove(entry.Key);
            }
        }
    }
}
