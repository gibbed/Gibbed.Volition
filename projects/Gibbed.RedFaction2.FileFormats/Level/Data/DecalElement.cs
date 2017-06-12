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
using System.Text;
using Gibbed.IO;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    public class DecalElement : ObjectElement
    {
        #region Fields
        private Vector3 _BoxSize;
        private string _DecalBitmap;
        private uint _Alpha;
        private bool _IsSelfIlluminated;
        private bool _Unknown12;
        private bool _Unknown13;
        private bool _Unknown14;
        private bool _Unknown15;
        private DecalTileMode _Tiling;
        private float _Scale;
        private uint _Unknown18;
        private Color _Unknown19;
        #endregion

        #region Properties
        public Vector3 BoxSize
        {
            get { return this._BoxSize; }
            set { this._BoxSize = value; }
        }

        public string DecalBitmap
        {
            get { return this._DecalBitmap; }
            set { this._DecalBitmap = value; }
        }

        public uint Alpha
        {
            get { return this._Alpha; }
            set { this._Alpha = value; }
        }

        public bool IsSelfIlluminated
        {
            get { return this._IsSelfIlluminated; }
            set { this._IsSelfIlluminated = value; }
        }

        public bool Unknown12
        {
            get { return this._Unknown12; }
            set { this._Unknown12 = value; }
        }

        public bool Unknown13
        {
            get { return this._Unknown13; }
            set { this._Unknown13 = value; }
        }

        public bool Unknown14
        {
            get { return this._Unknown14; }
            set { this._Unknown14 = value; }
        }

        public bool Unknown15
        {
            get { return this._Unknown15; }
            set { this._Unknown15 = value; }
        }

        public DecalTileMode Tiling
        {
            get { return this._Tiling; }
            set { this._Tiling = value; }
        }

        public float Scale
        {
            get { return this._Scale; }
            set { this._Scale = value; }
        }

        public uint Unknown18
        {
            get { return this._Unknown18; }
            set { this._Unknown18 = value; }
        }

        public Color Unknown19
        {
            get { return this._Unknown19; }
            set { this._Unknown19 = value; }
        }
        #endregion

        protected override ushort Unknown1MaximumLength
        {
            get { return ushort.MaxValue; }
        }

        protected override ushort ScriptNameMaximumLength
        {
            get { return ushort.MaxValue; }
        }

        public override void Read(Stream input, uint version, Endian endian)
        {
            base.Read(input, version, endian);
            this._BoxSize = Vector3.Read(input, endian);
            this._DecalBitmap = input.ReadStringU16(32, Encoding.ASCII, endian);
            this._Alpha = input.ReadValueU32(endian);
            this._IsSelfIlluminated = input.ReadValueB8();
            this._Unknown12 = version >= 265 ? input.ReadValueB8() : false;
            this._Unknown13 = version >= 294 ? input.ReadValueB8() : false;
            this._Unknown14 = version >= 280 ? input.ReadValueB8() : true;
            this._Unknown15 = version >= 280 ? input.ReadValueB8() : true;
            this._Tiling = (DecalTileMode)input.ReadValueU32(endian);
            this._Scale = input.ReadValueF32(endian);
            this._Unknown18 = version >= 261 ? input.ReadValueU32(endian) : 0;
            this._Unknown19 = version >= 262 ? Color.Read(input) : Color.White;
        }

        public override void Write(Stream output, uint version, Endian endian)
        {
            base.Write(output, version, endian);
            throw new NotImplementedException();
        }

        public class ArrayElement : SerializableArrayElement<DecalElement>
        {
        }
    }
}
