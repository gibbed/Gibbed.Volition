/* Copyright (c) 2017 Rick (rick 'at' gibbed 'dot' us)
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

namespace Gibbed.RedFaction2.FileFormats
{
    public class PegFile
    {
        public const uint Signature = 0x564B4547; // 'VKEG'

        private Endian _Endian;
        private uint _Version;
        private uint _DataSize;
        private readonly List<Peg.Texture> _Textures;

        public PegFile()
        {
            this._Textures = new List<Peg.Texture>();
        }

        public Endian Endian
        {
            get { return this._Endian; }
            set { this._Endian = value; }
        }

        public uint Version
        {
            get { return this._Version; }
            set { this._Version = value; }
        }

        public uint DataSize
        {
            get { return this._DataSize; }
            set { this._DataSize = value; }
        }

        public List<Peg.Texture> Textures
        {
            get { return this._Textures; }
        }

        public void Deserialize(Stream input)
        {
            var baseOffset = input.Position;

            var magic = input.ReadValueU32(Endian.Little);
            if (magic != Signature && magic.Swap() != Signature)
            {
                throw new FormatException("not a peg file");
            }

            var endian = magic == Signature ? Endian.Little : Endian.Big;

            var version = input.ReadValueU32(endian);
            if (version != 6)
            {
                throw new FormatException("unsupported peg version");
            }

            var textureHeaderSize = input.ReadValueU32(endian);
            if (baseOffset + 32 + textureHeaderSize > input.Length)
            {
                throw new EndOfStreamException();
            }

            var dataSize = input.ReadValueU32(endian);
            if (baseOffset + 32 + textureHeaderSize + dataSize > input.Length)
            {
                throw new EndOfStreamException();
            }

            var textureCount = input.ReadValueU32(endian);
            var unknown14 = input.ReadValueU32(endian);
            var frameCount = input.ReadValueU32(endian);
            var unknown1C = input.ReadValueU32(endian);

            if (unknown14 != 0 || unknown1C != 16)
            {
                throw new FormatException("unexpected unknown values");
            }

            var textures = new Peg.Texture[textureCount];
            int totalFrames = 0;
            for (int i = 0; i < textures.Length; i++)
            {
                var texture = textures[i] = Peg.Texture.Read(input, endian);
                if (texture.FrameCount == 0)
                {
                    throw new FormatException("frame count is 0");
                }
                totalFrames += texture.FrameCount > 1 ? (1 + texture.FrameCount) : 1;
            }

            if (totalFrames != frameCount)
            {
                throw new FormatException("did not read correct amount of frames");
            }

            this._Endian = endian;
            this._Version = version;
            this._DataSize = dataSize;
            this._Textures.Clear();
            this._Textures.AddRange(textures);
        }

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }
    }
}
