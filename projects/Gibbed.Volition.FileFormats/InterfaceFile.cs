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
        public const uint Signature = 0x3027;

        #region Fields
        private Endian _Endian;
        private string _Name;
        private ushort _Version;
        private float _AnimationTime;
        private readonly List<Interface.CriticalResource> _CriticalResources;
        private readonly List<Interface.Metadata> _Metadata;
        private readonly List<Interface.Object> _Elements;
        private readonly List<Interface.Object> _Animations;
        #endregion

        public InterfaceFile()
        {
            this._CriticalResources = new List<Interface.CriticalResource>();
            this._Metadata = new List<Interface.Metadata>();
            this._Elements = new List<Interface.Object>();
            this._Animations = new List<Interface.Object>();
        }

        #region Properties
        public Endian Endian
        {
            get { return this._Endian; }
            set { this._Endian = value; }
        }

        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }

        public ushort Version
        {
            get { return this._Version; }
            set { this._Version = value; }
        }

        public float AnimationTime
        {
            get { return this._AnimationTime; }
            set { this._AnimationTime = value; }
        }

        public List<Interface.CriticalResource> CriticalResources
        {
            get { return this._CriticalResources; }
        }

        public List<Interface.Metadata> Metadata
        {
            get { return this._Metadata; }
        }

        public List<Interface.Object> Elements
        {
            get { return this._Elements; }
        }

        public List<Interface.Object> Animations
        {
            get { return this._Animations; }
        }
        #endregion

        public void Serialize(Stream output)
        {
            var endian = this._Endian;

            var strings = new Interface.StringTable();

            output.WriteValueU32(Signature, endian);
            output.WriteValueS32(strings.WriteIndex(this._Name), endian);
            output.WriteValueU16(this._Version, endian);
            output.WriteValueF32(this._AnimationTime, endian);

            output.WriteValueS32(this._Metadata.Count, endian);
            output.WriteValueS32(this._CriticalResources.Count, endian);

            var stringTableOffsetOffset = output.Position;
            output.WriteValueU32(0xFFFFFFFF, endian); // string table stub

            output.WriteValueU16((ushort)this._Elements.Count);
            output.WriteValueU16((ushort)this._Animations.Count);

            foreach (var criticalResource in this._CriticalResources)
            {
                output.WriteValueEnum<Interface.CriticalResource>(criticalResource.Type);
                strings.WriteIndex(output, endian, criticalResource.Name);

                if (this._Version >= 2)
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

            foreach (var metadata in this._Metadata)
            {
                strings.WriteIndex(output, endian, metadata.Name);
                strings.WriteIndex(output, endian, metadata.Value);
            }

            foreach (var element in this._Elements)
            {
                element.Serialize(output, endian, strings);
            }

            foreach (var animation in this._Animations)
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
            if (magic != Signature && magic.Swap() != Signature)
            {
                throw new FormatException();
            }
            var endian = magic == Signature ? Endian.Little : Endian.Big;

            var nameIndex = input.ReadValueS32(endian);
            var version = input.ReadValueU16(endian);
            var animationTime = input.ReadValueF32(endian);
            var metadataCount = input.ReadValueU32(endian);
            var criticalResourceCount = input.ReadValueU32(endian);
            var stringTableOffset = input.ReadValueU32(endian);
            var elementCount = input.ReadValueU16(endian);
            var animationCount = input.ReadValueU16(endian);

            if (version != 1 && version != 2)
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

            var fileName = strings.ReadString(nameIndex);

            var criticalResources = new Interface.CriticalResource[criticalResourceCount];
            for (uint i = 0; i < criticalResourceCount; i++)
            {
                var type = input.ReadValueEnum<Interface.CriticalResourceType>();
                if (type != Interface.CriticalResourceType.Peg &&
                    type != Interface.CriticalResourceType.Document)
                {
                    throw new FormatException();
                }

                var name = strings.ReadString(input, endian);
                var autoload = this._Version >= 2 ? input.ReadValueB8() : false;

                if (autoload == true &&
                    type != Interface.CriticalResourceType.Peg)
                {
                    throw new FormatException();
                }

                criticalResources[i] = new Interface.CriticalResource()
                {
                    Type = type,
                    Name = name,
                    Autoload = autoload,
                };
            }

            var metadatas = new Interface.Metadata[metadataCount];
            for (uint i = 0; i < metadataCount; i++)
            {
                var name = strings.ReadString(input, endian);
                var value = strings.ReadString(input, endian);
                metadatas[i] = new Interface.Metadata(name, value);
            }

            var elements = new Interface.Object[elementCount];
            for (ushort i = 0; i < elementCount; i++)
            {
                var element = new Interface.Object();
                element.Deserialize(input, endian, strings);
                elements[i] = element;
            }

            var animations = new Interface.Object[animationCount];
            for (ushort i = 0; i < animationCount; i++)
            {
                var animation = new Interface.Object();
                animation.Deserialize(input, endian, strings);
                animations[i] = animation;
            }

            if (input.Position != stringTableOffset)
            {
                throw new FormatException();
            }

            this._Endian = endian;
            this._Name = fileName;
            this._Version = version;
            this._AnimationTime = animationTime;
            this._CriticalResources.Clear();
            this._CriticalResources.AddRange(criticalResources);
            this._Metadata.Clear();
            this._Metadata.AddRange(metadatas);
            this._Elements.Clear();
            this._Elements.AddRange(elements);
            this._Animations.Clear();
            this._Animations.AddRange(animations);
        }
    }
}
