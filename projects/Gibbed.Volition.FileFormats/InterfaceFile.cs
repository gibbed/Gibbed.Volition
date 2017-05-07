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

namespace Gibbed.Volition.FileFormats
{
    public class InterfaceFile
    {
        public Endian Endian;
        public string Name;
        public ushort Version;
        public float AnimationTime;

        public List<Interface.CriticalResource> CriticalResources
            = new List<Interface.CriticalResource>();
        public List<Interface.Metadata> Metadata
            = new List<Interface.Metadata>();
        public List<Interface.Object> Elements
            = new List<Interface.Object>();
        public List<Interface.Object> Animations
            = new List<Interface.Object>();

        public void Serialize(Stream output)
        {
            var endian = this.Endian;

            var strings = new Interface.StringTable();

            output.WriteValueU32(0x3027, endian);
            output.WriteValueS32(strings.WriteIndex(this.Name), endian);
            output.WriteValueU16(this.Version, endian);
            output.WriteValueF32(this.AnimationTime, endian);

            output.WriteValueS32(this.Metadata.Count, endian);
            output.WriteValueS32(this.CriticalResources.Count, endian);

            var stringTableOffsetOffset = output.Position;
            output.WriteValueU32(0xFFFFFFFF, endian); // string table stub

            output.WriteValueU16((ushort)this.Elements.Count);
            output.WriteValueU16((ushort)this.Animations.Count);

            foreach (var criticalResource in this.CriticalResources)
            {
                output.WriteValueEnum<Interface.CriticalResource>(criticalResource.Type);
                strings.WriteIndex(output, endian, criticalResource.Name);

                if (this.Version >= 2)
                {
                    output.WriteValueB8(criticalResource.Autoload);
                }
                else
                {
                    if (criticalResource.Autoload == true)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            foreach (var metadata in this.Metadata)
            {
                strings.WriteIndex(output, endian, metadata.Name);
                strings.WriteIndex(output, endian, metadata.Value);
            }

            foreach (var element in this.Elements)
            {
                element.Serialize(output, endian, strings);
            }

            foreach (var animation in this.Animations)
            {
                animation.Serialize(output, endian, strings);
            }

            var stringTableOffset = output.Position;
            strings.Serialize(output, endian);

            output.Seek(stringTableOffsetOffset, SeekOrigin.Begin);
            output.WriteValueU32((uint)stringTableOffset, endian);
        }

        public void Deserialize(Stream input)
        {
            var magic = input.ReadValueU32(Endian.Little);
            if (magic != 0x3027 &&
                magic.Swap() != 0x3027)
            {
                throw new FormatException();
            }
            var endian = magic == 0x3027 ? Endian.Little : Endian.Big;

            var nameIndex = input.ReadValueS32(endian);
            this.Version = input.ReadValueU16(endian);
            this.AnimationTime = input.ReadValueF32(endian);
            var metadataCount = input.ReadValueU32(endian);
            var criticalResourceCount = input.ReadValueU32(endian);
            var stringTableOffset = input.ReadValueU32(endian);
            var elementCount = input.ReadValueU16(endian);
            var animationCount = input.ReadValueU16(endian);

            if (this.Version != 1 &&
                this.Version != 2)
            {
                throw new FormatException();
            }

            if (stringTableOffset >= input.Length)
            {
                throw new FormatException();
            }

            var position = input.Position;
            input.Seek(stringTableOffset, SeekOrigin.Begin);
            var strings = new Interface.StringTable();
            strings.Deserialize(input, endian);
            input.Seek(position, SeekOrigin.Begin);

            if (nameIndex != 0)
            {
                throw new FormatException();
            }

            this.Name = strings.ReadString(nameIndex);

            for (uint i = 0; i < criticalResourceCount; i++)
            {
                var type = input.ReadValueEnum<Interface.CriticalResourceType>();
                if (type != Interface.CriticalResourceType.Peg &&
                    type != Interface.CriticalResourceType.Document)
                {
                    throw new FormatException();
                }

                var name = strings.ReadString(input, endian);
                var autoload = this.Version >= 2 ? input.ReadValueB8() : false;

                if (autoload == true &&
                    type != Interface.CriticalResourceType.Peg)
                {
                    throw new FormatException();
                }

                this.CriticalResources.Add(new Interface.CriticalResource()
                    {
                        Type = type,
                        Name = name,
                        Autoload = autoload,
                    });
            }

            this.Metadata.Clear();
            for (uint i = 0; i < metadataCount; i++)
            {
                var name = strings.ReadString(input, endian);
                var value = strings.ReadString(input, endian);
                this.Metadata.Add(new Interface.Metadata(name, value));
            }

            this.Elements.Clear();
            for (ushort i = 0; i < elementCount; i++)
            {
                var element = new Interface.Object();
                element.Deserialize(input, endian, strings);
                this.Elements.Add(element);
            }

            this.Animations.Clear();
            for (ushort i = 0; i < animationCount; i++)
            {
                var animation = new Interface.Object();
                animation.Deserialize(input, endian, strings);
                this.Animations.Add(animation);
            }

            if (input.Position != stringTableOffset)
            {
                throw new FormatException();
            }
        }
    }
}
