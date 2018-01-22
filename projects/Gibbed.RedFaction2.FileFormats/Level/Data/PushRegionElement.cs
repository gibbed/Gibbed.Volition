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
    public class PushRegionElement : ObjectElement
    {
        private PushRegionShapeType _Shape;
        private float _SphereSize;
        private Vector3 _BoxSize;
        private float _Strength;
        private PushRegionFlags _Flags;
        private PushRegionIntensityType _Intensity;
        private byte _Turbulence;

        protected override ushort ClassNameMaximumLength
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

            this._Shape = (PushRegionShapeType)input.ReadValueU32(endian);

            if (this._Shape == PushRegionShapeType.Sphere)
            {
                this._SphereSize = input.ReadValueF32(endian);
            }
            else if (this._Shape == PushRegionShapeType.AxisAlignedBoundingBox ||
                     this._Shape == PushRegionShapeType.OrientedBoundingBox)
            {
                this._BoxSize = Vector3.Read(input, endian);
            }
            else
            {
                throw new FormatException();
            }

            this._Strength = input.ReadValueF32(endian);

            var flags = input.ReadValueU32(endian);
            if ((flags & ~0xF007Fu) != 0)
            {
                throw new FormatException();
            }

            this._Flags = (PushRegionFlags)(flags & ~(0xFu << 16));
            this._Intensity = (PushRegionIntensityType)((flags >> 2) & 0x3u);
            this._Turbulence = (byte)((flags >> 16) & 0xFu);
        }

        public override void Write(Stream output, uint version, Endian endian)
        {
            base.Write(output, version, endian);
            throw new NotImplementedException();
        }

        public class ArrayElement : SerializableArrayElement<PushRegionElement>
        {
        }
    }
}
