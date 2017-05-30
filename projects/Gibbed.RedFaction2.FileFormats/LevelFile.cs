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
using Gibbed.RedFaction2.FileFormats.Level;

namespace Gibbed.RedFaction2.FileFormats
{
    public class LevelFile
    {
        public const uint Signature = 0xD4BADA55; // "DA BAD ASS" (sigh)

        private Endian _Endian;
        private uint _Version;
        private uint _Unknown1;
        private uint _Unknown2;
        private uint _Unknown3;
        private uint _Unknown4;
        private uint _Unknown5;
        private string _Name;
        private readonly Dictionary<MetadataType, IElement> _Metadatas;
        private readonly Dictionary<ElementType, IElement> _Elements;

        public LevelFile()
        {
            this._Metadatas = new Dictionary<MetadataType, IElement>();
            this._Elements = new Dictionary<ElementType, IElement>();
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

        public uint Unknown1
        {
            get { return this._Unknown1; }
            set { this._Unknown1 = value; }
        }

        public uint Unknown2
        {
            get { return this._Unknown2; }
            set { this._Unknown2 = value; }
        }

        public uint Unknown3
        {
            get { return this._Unknown3; }
            set { this._Unknown3 = value; }
        }

        public uint Unknown4
        {
            get { return this._Unknown4; }
            set { this._Unknown4 = value; }
        }

        public uint Unknown5
        {
            get { return this._Unknown5; }
            set { this._Unknown5 = value; }
        }

        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }

        public Dictionary<MetadataType, IElement> Metadatas
        {
            get { return this._Metadatas; }
        }

        public Dictionary<ElementType, IElement> Elements
        {
            get { return this._Elements; }
        }

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            var magic = input.ReadValueU32(Endian.Little);
            if (magic != Signature && magic.Swap() != Signature)
            {
                throw new FormatException();
            }
            var endian = magic == Signature ? Endian.Little : Endian.Big;

            var version = input.ReadValueU32(endian);
            if (version < 201)
            {
                throw new NotSupportedException("level version is too old");
            }
            if (version > 295)
            {
                throw new NotSupportedException("level version is too new");
            }

            var unknown1 = version >= 114 ? input.ReadValueU32(endian) : 0;
            var unknown2 = input.ReadValueU32(endian);
            var unknown3 = input.ReadValueU32(endian);
            var unknown4 = version >= 160 ? input.ReadValueU32(endian) : 35;
            var unknown5 = version >= 160 ? input.ReadValueU32(endian) : 0;
            var name = version >= 170 ? input.ReadStringU16(256, Encoding.ASCII, endian) : "<untitled>";
            var metadatas = new Dictionary<MetadataType, IElement>();
            var elements = new Dictionary<ElementType, IElement>();

            bool hasData = false;
            while (input.Position < input.Length)
            {
                var dataType = (MetadataType)input.ReadValueU32(endian);
                var dataLength = input.ReadValueU32(endian);

                if (dataType == MetadataType.None)
                {
                    break;
                }

                var element = MetadataFactory.Create(dataType);
                var dataPosition = input.Position;
                using (var data = input.ReadToMemoryStream(dataLength))
                {
                    element.Read(data, version, endian);
                    if (data.Position != data.Length)
                    {
                        throw new FormatException();
                    }
                }
                metadatas.Add(dataType, element);

                if (dataType == MetadataType.Data)
                {
                    hasData = true;
                    break;
                }
            }

            if (hasData == true)
            {
                while (input.Position < input.Length)
                {
                    var dataType = (ElementType)input.ReadValueU32(endian);
                    var dataLength = input.ReadValueU32(endian);

                    if (dataType == ElementType.None)
                    {
                        break;
                    }

                    var element = ElementFactory.Create(dataType);
                    var dataPosition = input.Position;
                    using (var data = input.ReadToMemoryStream(dataLength))
                    {
                        element.Read(data, version, endian);
                        if (data.Position != data.Length)
                        {
                            throw new FormatException();
                        }
                    }
                    elements.Add(dataType, element);
                }
            }

            if (input.Position != input.Length)
            {
                throw new FormatException();
            }

            this._Endian = endian;
            this._Version = version;
            this._Unknown1 = unknown1;
            this._Unknown2 = unknown2;
            this._Unknown3 = unknown3;
            this._Unknown4 = unknown4;
            this._Unknown5 = unknown5;
            this._Name = name;
            this._Metadatas.Clear();
            foreach (var kv in metadatas)
            {
                this._Metadatas.Add(kv.Key, kv.Value);
            }
            this._Elements.Clear();
            foreach (var kv in elements)
            {
                this._Elements.Add(kv.Key, kv.Value);
            }
        }
    }
}
