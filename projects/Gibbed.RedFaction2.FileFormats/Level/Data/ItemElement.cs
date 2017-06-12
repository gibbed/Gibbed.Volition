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
using System.IO;
using Gibbed.IO;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    public class ItemElement : ObjectElement
    {
        #region Fields
        private int _Count;
        private int _RespawnTime;
        private int _TeamId;
        private int _Unknown9;
        private bool _Unknown10;
        private bool _Unknown11;
        #endregion

        #region Properties
        public int Count
        {
            get { return this._Count; }
            set { this._Count = value; }
        }

        public int RespawnTime
        {
            get { return this._RespawnTime; }
            set { this._RespawnTime = value; }
        }

        public int TeamId
        {
            get { return this._TeamId; }
            set { this._TeamId = value; }
        }

        public int Unknown9
        {
            get { return this._Unknown9; }
            set { this._Unknown9 = value; }
        }

        public bool Unknown10
        {
            get { return this._Unknown10; }
            set { this._Unknown10 = value; }
        }

        public bool Unknown11
        {
            get { return this._Unknown11; }
            set { this._Unknown11 = value; }
        }
        #endregion

        protected override ushort Unknown1MaximumLength
        {
            get { return 32; }
        }

        protected override ushort ScriptNameMaximumLength
        {
            get { return 32; }
        }

        public override void Read(Stream input, uint version, Endian endian)
        {
            base.Read(input, version, endian);
            this._Count = input.ReadValueS32(endian);
            this._RespawnTime = input.ReadValueS32(endian);
            this._TeamId = input.ReadValueS32(endian);
            this._Unknown9 = version >= 222 ? input.ReadValueS32(endian) : -1;
            this._Unknown10 = version >= 279 && input.ReadValueB8() == true;
            this._Unknown11 = version >= 293 && input.ReadValueB8() == true;
        }

        public override void Write(Stream output, uint version, Endian endian)
        {
            base.Write(output, version, endian);
            throw new NotImplementedException();
        }

        public class ArrayElement : SerializableArrayElement<ItemElement>
        {
        }
    }
}
