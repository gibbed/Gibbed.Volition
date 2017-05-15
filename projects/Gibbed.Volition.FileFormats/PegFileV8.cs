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
using System.Text;
using Gibbed.IO;

namespace Gibbed.Volition.FileFormats
{
    public class PegFileV8
    {
        public const uint Signature = 0x564B4547; // 'VKEG'

        private Endian _Endian;
        private ushort _Version;
        private Platform _Platform;
        private uint _DataSize;
        private readonly List<Peg.Texture<Peg.FrameV8>> _Textures;

        public PegFileV8()
        {
            this._Textures = new List<Peg.Texture<Peg.FrameV8>>();
        }

        public Endian Endian
        {
            get { return this._Endian; }
            set { this._Endian = value; }
        }

        public ushort Version
        {
            get { return this._Version; }
            set { this._Version = value; }
        }

        public Platform Platform
        {
            get { return this._Platform; }
            set { this._Platform = value; }
        }

        public uint DataSize
        {
            get { return this._DataSize; }
            set { this._DataSize = value; }
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

            var version = input.ReadValueU16(endian);
            if (version != 8)
            {
                throw new FormatException("unsupported peg version");
            }

            var platform = input.ReadValueU16(endian);
            if (Enum.IsDefined(typeof(Platform), platform) == false)
            {
                throw new FormatException("unsupported peg platform");
            }

            var headerSize = input.ReadValueU32(endian);
            if (baseOffset + headerSize > input.Length)
            {
                throw new EndOfStreamException();
            }

            var textureCount = input.ReadValueU16(endian);
            var unknown12 = input.ReadValueU16(endian);
            var frameCount = input.ReadValueU16(endian);
            var unknown16 = input.ReadValueU8();
            var unknown17 = input.ReadValueU8();

            if (unknown12 != 0 ||
                unknown16 != 0 ||
                unknown17 != 16)
            {
                throw new FormatException("unexpected unknown values");
            }

            var textures = new Peg.Texture<Peg.FrameV8>[textureCount];
            int totalFrames = 0;
            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = new Peg.Texture<Peg.FrameV8>();

                var frame = new Peg.FrameV8();
                frame.Deserialize(input, endian);
                textures[i].Frames.Add(frame);
                totalFrames++;

                if (frame.FrameCount == 0)
                {
                    throw new FormatException("frame count is 0");
                }

                for (int j = 1; j < frame.FrameCount; j++)
                {
                    var extraFrame = new Peg.FrameV8();
                    extraFrame.Deserialize(input, endian);

                    if (extraFrame.FrameCount != 1)
                    {
                        throw new FormatException("frame count is not 1");
                    }

                    textures[i].Frames.Add(extraFrame);
                    totalFrames++;
                }
            }

            if (totalFrames != frameCount)
            {
                throw new FormatException("did not read correct amount of frames");
            }

            foreach (var texture in textures)
            {
                texture.Name = input.ReadStringZ(Encoding.ASCII);
            }

            this._Endian = endian;
            this._Version = version;
            this._Platform = (Platform)platform;
            this._DataSize = input.ReadValueU32(endian);
            this._Textures.Clear();
            this._Textures.AddRange(textures);
        }

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }
    }
}
