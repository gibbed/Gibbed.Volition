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

using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.IO;

namespace Gibbed.RedFaction2.FileFormats
{
    public class TableOfContentsGroupFile
    {
        #region Fields
        private string _BaseName;
        private string _BasePath;
        private uint _InternalPriority;
        private string _InternalDataPath;
        private readonly List<InternalEntry> _InternalEntries;
        private readonly List<ExternalEntry> _ExternalEntries;
        #endregion

        public TableOfContentsGroupFile()
        {
            this._InternalEntries = new List<InternalEntry>();
            this._ExternalEntries = new List<ExternalEntry>();
        }

        #region Properties
        public string BaseName
        {
            get { return this._BaseName; }
            set { this._BaseName = value; }
        }

        public string BasePath
        {
            get { return this._BasePath; }
            set { this._BasePath = value; }
        }

        public uint InternalPriority
        {
            get { return this._InternalPriority; }
            set { this._InternalPriority = value; }
        }

        public string InternalDataPath
        {
            get { return this._InternalDataPath; }
            set { this._InternalDataPath = value; }
        }

        public List<InternalEntry> InternalEntries
        {
            get { return this._InternalEntries; }
        }

        public List<ExternalEntry> ExternalEntries
        {
            get { return this._ExternalEntries; }
        }
        #endregion

        public void Serialize(Stream output, Endian endian)
        {
            output.WriteStringZ(this._BaseName, Encoding.ASCII);
            output.WriteStringZ(this._BasePath, Encoding.ASCII);
            output.WriteValueU32(this._InternalPriority, endian);
            output.WriteValueS32(this._ExternalEntries.Count, endian);

            if (this._InternalPriority != 0)
            {
                output.WriteStringZ(this._InternalDataPath, Encoding.ASCII);
                output.WriteValueS32(this._InternalEntries.Count, endian);

                foreach (var entry in this._InternalEntries)
                {
                    entry.Write(output, endian);
                }
            }

            foreach (var entry in this._ExternalEntries)
            {
                entry.Write(output, endian);
            }
        }

        public void Deserialize(Stream input, Endian endian)
        {
            var baseName = input.ReadStringZ(Encoding.ASCII);
            var basePath = input.ReadStringZ(Encoding.ASCII);
            var internalPriority = input.ReadValueU32(endian);
            var externalCount = input.ReadValueU32(endian);

            string internalDataPath = null;
            uint internalCount = 0;
            if (internalPriority != 0)
            {
                internalDataPath = input.ReadStringZ(Encoding.ASCII);
                internalCount = input.ReadValueU32(endian);
            }

            var internalEntries = new InternalEntry[internalCount];
            for (uint i = 0; i < internalCount; i++)
            {
                internalEntries[i] = InternalEntry.Read(input, endian);
            }

            if (input.Position == input.Length)
            {
                externalCount = 0; // cheap hack for All_Levels.toc_group
            }

            var externalEntries = new ExternalEntry[externalCount];
            for (uint i = 0; i < externalCount; i++)
            {
                externalEntries[i] = ExternalEntry.Read(input, endian);
            }

            this._BaseName = baseName;
            this._BasePath = basePath;
            this._InternalPriority = internalPriority;
            this._InternalDataPath = internalDataPath;
            this._InternalEntries.Clear();
            this._InternalEntries.AddRange(internalEntries);
            this._ExternalEntries.Clear();
            this._ExternalEntries.AddRange(externalEntries);
        }

        public struct InternalEntry
        {
            public string Name;
            public uint Size;
            public uint Offset;

            public static InternalEntry Read(Stream input, Endian endian)
            {
                InternalEntry instance;
                instance.Name = input.ReadStringZ(Encoding.ASCII);
                instance.Size = input.ReadValueU32(endian);
                instance.Offset = input.ReadValueU32(endian);
                return instance;
            }

            public static void Write(Stream output, InternalEntry instance, Endian endian)
            {
                output.WriteStringZ(instance.Name, Encoding.ASCII);
                output.WriteValueU32(instance.Size, endian);
                output.WriteValueU32(instance.Offset, endian);
            }

            public void Write(Stream output, Endian endian)
            {
                Write(output, this, endian);
            }
        }

        public struct ExternalEntry
        {
            public string Name;
            public uint Size;
            public string Path;

            public static ExternalEntry Read(Stream input, Endian endian)
            {
                ExternalEntry instance;
                instance.Name = input.ReadStringZ(Encoding.ASCII);
                instance.Size = input.ReadValueU32(endian);
                instance.Path = input.ReadStringZ(Encoding.ASCII);
                return instance;
            }

            public static void Write(Stream output, ExternalEntry instance, Endian endian)
            {
                output.WriteStringZ(instance.Name, Encoding.ASCII);
                output.WriteValueU32(instance.Size, endian);
                output.WriteStringZ(instance.Path, Encoding.ASCII);
            }

            public void Write(Stream output, Endian endian)
            {
                Write(output, this, endian);
            }
        }
    }
}
