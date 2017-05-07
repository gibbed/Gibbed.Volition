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
    public class PegFileV13
    {
        public Endian Endian;
        public ushort Version;
        public Platform Platform;
        public uint DataSize;
        public List<Peg.Texture<Peg.FrameV13>> Textures
            = new List<Peg.Texture<Peg.FrameV13>>();

        public void Deserialize(Stream input)
        {
            var baseOffset = input.Position;

            var magic = input.ReadValueU32(Endian.Little);
            if (magic != 0x564B4547 && // VKEG
                magic != 0x47454B56)
            {
                throw new FormatException("not a peg file");
            }

            var endian = magic == 0x564B4547 ? Endian.Little : Endian.Big;
            this.Endian = endian;

            var version = input.ReadValueU16(endian);
            if (version != 13)
            {
                throw new FormatException("unsupported peg version");
            }
            this.Version = version;

            var platform = input.ReadValueU16(endian);
            if (Enum.IsDefined(typeof(Platform), platform) == false)
            {
                throw new FormatException("unsupported peg platform");
            }
            this.Platform = (Platform)platform;

            var headerSize = input.ReadValueU32(endian);
            if (baseOffset + headerSize > input.Length)
            {
                throw new EndOfStreamException();
            }

            this.DataSize = input.ReadValueU32(endian);

            var textureCount = input.ReadValueU16(endian);
            var unknown12 = input.ReadValueU16(endian);
            var frameCount = input.ReadValueU16(endian);
            var unknown16 = input.ReadValueU8();
            var unknown17 = input.ReadValueU8();

            if (unknown12 != 0 ||
                unknown16 != 16 ||
                unknown17 != 0)
            {
                throw new FormatException("unexpected unknown values");
            }

            var textures = new Peg.Texture<Peg.FrameV13>[textureCount];
            int totalFrames = 0;
            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = new Peg.Texture<Peg.FrameV13>();

                var frame = new Peg.FrameV13();
                frame.Deserialize(input, endian);
                textures[i].Frames.Add(frame);
                totalFrames++;

                if (frame.FrameCount == 0)
                {
                    throw new FormatException("frame count is 0");
                }

                for (int j = 1; j < frame.FrameCount; j++)
                {
                    var extraFrame = new Peg.FrameV13();
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

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i].Name = input.ReadStringZ(Encoding.ASCII);
            }

            this.Textures.AddRange(textures);
        }

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }
    }
}
