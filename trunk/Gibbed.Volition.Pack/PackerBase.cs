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
using Gibbed.IO;
using Gibbed.Volition.FileFormats;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Package = Gibbed.Volition.FileFormats.Package;
using ZLIB = ComponentAce.Compression.Zlib;

namespace Gibbed.Volition.Pack
{
    public abstract class PackerBase<TPackage, TEntry>
        where TPackage : IPackageFile<TEntry>, new()
        where TEntry : IPackageEntry, new()
    {
        public bool SupportPS3Chunking = false;

        public abstract int Main(string[] args);

        protected void Build(
            TPackage package,
            IEnumerable<KeyValuePair<string, string>> paths,
            string outputPath,
            bool ps3)
        {
            var isCompressed = (package.Flags & Package.HeaderFlags.Compressed) != 0;
            var isCondensed = (package.Flags & Package.HeaderFlags.Condensed) != 0;

            package.Entries.Clear();
            foreach (var kv in paths)
            {
                package.Entries.Add(new TEntry()
                {
                    Name = kv.Key,
                });
            }

            var baseOffset = package.EstimateHeaderSize();

            package.Entries.Clear();
            using (var output = File.Create(outputPath))
            {
                if (isCondensed == true &&
                    isCompressed == true)
                {
                    output.Seek(baseOffset, SeekOrigin.Begin);

                    using (var compressed = new MemoryStream())
                    {
                        var z = new ZLIB.ZOutputStream(compressed, ZLIB.zlibConst.Z_BEST_COMPRESSION);
                        z.FlushMode = ZLIB.zlibConst.Z_SYNC_FLUSH;

                        long offset = 0;
                        foreach (var kv in paths)
                        {
                            using (var input = File.OpenRead(kv.Value))
                            {
                                var entry = new TEntry();
                                entry.Name = kv.Key;
                                entry.Offset = (uint)offset;
                                entry.UncompressedSize = (uint)input.Length;

                                long size = z.TotalOut;

                                z.WriteFromStream(input, input.Length);

                                size = z.TotalOut - size;

                                entry.CompressedSize = (uint)size;

                                offset += entry.UncompressedSize;

                                package.Entries.Add(entry);
                            }
                        }

                        package.CompressedSize = (uint)compressed.Length;
                        package.UncompressedSize = (uint)offset;

                        compressed.Position = 0;
                        output.WriteFromStream(compressed, compressed.Length);
                    }

                    output.Seek(0, SeekOrigin.Begin);
                    package.Serialize(output);
                }
                else if (
                    ps3 == true &&
                    isCompressed == true)
                {
                    output.Seek(baseOffset, SeekOrigin.Begin);

                    long offset = 0;
                    uint uncompressedSize = 0;

                    foreach (var kv in paths)
                    {
                        using (var input = File.OpenRead(kv.Value))
                        {
                            if (isCondensed == false)
                            {
                                var offsetPadding =
                                    offset.Align(2048) - offset;
                                if (offsetPadding > 0)
                                {
                                    offset += offsetPadding;
                                    output.Seek(offsetPadding, SeekOrigin.Current);
                                }

                                var sizePadding =
                                    uncompressedSize.Align(2048) - uncompressedSize;
                                if (sizePadding > 0)
                                {
                                    uncompressedSize += sizePadding;
                                }
                            }

                            var entry = new TEntry();
                            entry.Name = kv.Key;
                            entry.Offset = (uint)offset;
                            entry.UncompressedSize = (uint)input.Length;
                            entry.CompressedSize = 0;

                            var left = input.Length;
                            while (left > 0)
                            {
                                using (var compressed = new MemoryStream())
                                {
                                    var chunkUncompressedSize = (uint)Math.Min(0x10000, left);

                                    var zlib = new DeflaterOutputStream(compressed, new Deflater(9, true));
                                    zlib.WriteFromStream(input, chunkUncompressedSize);
                                    zlib.Finish();

                                    var chunkCompressedSize = (uint)compressed.Length;

                                    if (chunkCompressedSize > 0xFFFF)
                                    {
                                        throw new InvalidOperationException();
                                    }

                                    output.WriteValueU16((ushort)chunkCompressedSize, package.Endian);
                                    output.WriteValueU16(0, package.Endian);
                                    output.WriteValueU32(chunkUncompressedSize, package.Endian);

                                    entry.CompressedSize += 2 + 2 + 4;
                                    entry.CompressedSize += chunkCompressedSize;

                                    compressed.Position = 0;
                                    output.WriteFromStream(compressed, compressed.Length);

                                    left -= chunkUncompressedSize;
                                }
                            }

                            offset += entry.CompressedSize;
                            uncompressedSize += entry.UncompressedSize;

                            package.Entries.Add(entry);
                        }
                    }

                    package.CompressedSize = (uint)offset;
                    package.UncompressedSize = uncompressedSize;
                }
                else if (isCompressed == true)
                {
                    output.Seek(baseOffset, SeekOrigin.Begin);

                    long offset = 0;
                    uint uncompressedSize = 0;

                    foreach (var kv in paths)
                    {
                        using (var input = File.OpenRead(kv.Value))
                        {
                            if (isCondensed == false)
                            {
                                var offsetPadding =
                                    offset.Align(2048) - offset;
                                if (offsetPadding > 0)
                                {
                                    offset += offsetPadding;
                                    output.Seek(offsetPadding, SeekOrigin.Current);
                                }

                                var sizePadding =
                                    uncompressedSize.Align(2048) - uncompressedSize;
                                if (sizePadding > 0)
                                {
                                    uncompressedSize += sizePadding;
                                }
                            }

                            var entry = new TEntry();
                            entry.Name = kv.Key;
                            entry.Offset = (uint)offset;
                            entry.UncompressedSize = (uint)input.Length;

                            using (var compressed = new MemoryStream())
                            {
                                var zlib = new DeflaterOutputStream(compressed);
                                zlib.WriteFromStream(input, input.Length);
                                zlib.Finish();

                                entry.CompressedSize = (uint)compressed.Length;

                                compressed.Position = 0;
                                output.WriteFromStream(compressed, compressed.Length);
                            }

                            offset += entry.CompressedSize;
                            uncompressedSize += entry.UncompressedSize;

                            package.Entries.Add(entry);
                        }
                    }

                    package.CompressedSize = (uint)offset;
                    package.UncompressedSize = uncompressedSize;
                }
                else
                {
                    output.Seek(baseOffset, SeekOrigin.Begin);

                    long offset = 0;

                    foreach (var kv in paths)
                    {
                        using (var input = File.OpenRead(kv.Value))
                        {
                            if (isCondensed == false)
                            {
                                var padding = offset.Align(2048) - offset;
                                if (padding > 0)
                                {
                                    offset += padding;
                                    output.Seek(padding, SeekOrigin.Current);
                                }
                            }
                            else if (
                                isCondensed == true &&
                                isCompressed == false)
                            {
                                var padding = offset.Align(16) - offset;
                                if (padding > 0)
                                {
                                    offset += padding;
                                    output.Seek(padding, SeekOrigin.Current);
                                }
                            }

                            var entry = new TEntry();
                            entry.Name = kv.Key;
                            entry.Offset = (uint)offset;
                            entry.UncompressedSize = (uint)input.Length;

                            output.WriteFromStream(input, input.Length);

                            entry.CompressedSize = 0xFFFFFFFF;
                            offset += entry.UncompressedSize;

                            package.Entries.Add(entry);
                        }
                    }

                    package.CompressedSize = 0xFFFFFFFF;
                    package.UncompressedSize = (uint)offset;
                }

                package.TotalSize = (uint)output.Length;
                output.Seek(0, SeekOrigin.Begin);
                package.Serialize(output);
            }
        }
    }
}
