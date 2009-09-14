using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.Helpers;
using Ionic.Zlib;

namespace Gibbed.Volition.FileFormats
{
    public class Package
    {
        public uint Version;
        public bool LittleEndian;

        private bool IsSolid;

        private Stream Stream;
        private long BaseOffset;

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
            this.BaseOffset = this.Stream.Position;

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

            if (this.Version < 3 || this.Version > 4)
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
                ZlibStream zlib = new ZlibStream(input, CompressionMode.Decompress, true);

                // Decompress solid data
                {
                    int left = solid.Length;
                    int offset = 0;
                    while (left > 0)
                    {
                        int read = zlib.Read(solid, offset, Math.Min(4096, left));
                        if (read < 0)
                        {
                            throw new InvalidOperationException("zlib error");
                        }
                        else if (read == 0)
                        {
                            break;
                        }

                        left -= read;
                        offset += read;
                    }
                }

                foreach (Packages.PackageEntry packageEntry in packageFile.Entries)
                {
                    MemoryEntry entry = new MemoryEntry();
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
                StreamEntry entry = (StreamEntry)this.Entries[name];

                this.Stream.Seek(this.BaseOffset + entry.Offset, SeekOrigin.Begin);

                byte[] data = new byte[entry.Size];

                if (entry.CompressionType == Packages.PackageCompressionType.Zlib)
                {
                    ZlibStream zlib = new ZlibStream(this.Stream, CompressionMode.Decompress, true);
                    
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

        public void SetEntryStream(string name, Stream stream)
        {
            byte[] data = new byte[stream.Length];

            long oldPosition = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(data, 0, data.Length);
            stream.Seek(oldPosition, SeekOrigin.Begin);

            this.SetEntry(name, data);
        }

        public void ExportEntry(string name, Stream output)
        {
            if (this.Entries.ContainsKey(name) == false)
            {
                throw new KeyNotFoundException();
            }

            if (this.Entries[name] is StreamEntry)
            {
                StreamEntry entry = (StreamEntry)this.Entries[name];

                this.Stream.Seek(this.BaseOffset + entry.Offset, SeekOrigin.Begin);

                if (entry.CompressionType == Packages.PackageCompressionType.Zlib)
                {
                    // this is a hack until I fix zlib handling
                    MemoryStream memory = this.Stream.ReadToMemoryStream(entry.CompressedSize);
                    
                    ZlibStream zlib = new ZlibStream(memory, CompressionMode.Decompress, true);
                    
                    int left = entry.Size;
                    byte[] block = new byte[4096];
                    while (left > 0)
                    {
                        int read = zlib.Read(block, 0, Math.Min(block.Length, left));
                        if (read == 0)
                        {
                            break;
                        }
                        else if (read < 0)
                        {
                            throw new InvalidOperationException("zlib error");
                        }

                        output.Write(block, 0, read);
                        left -= read;
                    }
                }
                else if (entry.CompressionType == Packages.PackageCompressionType.None)
                {
                    int left = entry.Size;
                    byte[] data = new byte[4096];
                    while (left > 0)
                    {
                        int block = (int)(Math.Min(left, 4096));
                        this.Stream.Read(data, 0, block);
                        output.Write(data, 0, block);
                        left -= block;
                    }
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
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Commit()
        {
            this.Commit(false);
        }

        public void Commit(bool cleanCommit)
        {
            throw new NotImplementedException();
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
