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
    public class BoltEmitterElement : ObjectElement
    {
        #region Fields
        private int _TargetUid;
        private float _SourceControlDistance;
        private float _TargetControlDistance;
        private float _Thickness;
        private float _Jitter;
        private uint _SegmentCount;
        private float _SpawnDelay;
        private float _SpawnDelayExtra;
        private float _Decay;
        private float _DecayExtra;
        private Color _BoltColor;
        private string _BoltBitmap;
        private BoltEmitterFlags _Flags;
        private bool _EmitterIsInitiallyOn;
        #endregion

        #region Properties
        public int TargetUid
        {
            get { return this._TargetUid; }
            set { this._TargetUid = value; }
        }

        public float SourceControlDistance
        {
            get { return this._SourceControlDistance; }
            set { this._SourceControlDistance = value; }
        }

        public float TargetControlDistance
        {
            get { return this._TargetControlDistance; }
            set { this._TargetControlDistance = value; }
        }

        public float Thickness
        {
            get { return this._Thickness; }
            set { this._Thickness = value; }
        }

        public float Jitter
        {
            get { return this._Jitter; }
            set { this._Jitter = value; }
        }

        public uint SegmentCount
        {
            get { return this._SegmentCount; }
            set { this._SegmentCount = value; }
        }

        public float SpawnDelay
        {
            get { return this._SpawnDelay; }
            set { this._SpawnDelay = value; }
        }

        public float SpawnDelayExtra
        {
            get { return this._SpawnDelayExtra; }
            set { this._SpawnDelayExtra = value; }
        }

        public float Decay
        {
            get { return this._Decay; }
            set { this._Decay = value; }
        }

        public float DecayExtra
        {
            get { return this._DecayExtra; }
            set { this._DecayExtra = value; }
        }

        public Color BoltColor
        {
            get { return this._BoltColor; }
            set { this._BoltColor = value; }
        }

        public string BoltBitmap
        {
            get { return this._BoltBitmap; }
            set { this._BoltBitmap = value; }
        }

        public BoltEmitterFlags Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public bool EmitterIsInitiallyOn
        {
            get { return this._EmitterIsInitiallyOn; }
            set { this._EmitterIsInitiallyOn = value; }
        }
        #endregion

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
            this._TargetUid = input.ReadValueS32(endian);
            this._SourceControlDistance = input.ReadValueF32(endian);
            this._TargetControlDistance = input.ReadValueF32(endian);
            this._Thickness = input.ReadValueF32(endian);
            this._Jitter = input.ReadValueF32(endian);
            this._SegmentCount = input.ReadValueU32(endian);
            this._SpawnDelay = input.ReadValueF32(endian);
            this._SpawnDelayExtra = input.ReadValueF32(endian);
            this._Decay = input.ReadValueF32(endian);
            this._DecayExtra = input.ReadValueF32(endian);
            this._BoltColor = Color.Read(input);
            this._BoltBitmap = input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
            this._Flags = (BoltEmitterFlags)input.ReadValueU32(endian);
            this._EmitterIsInitiallyOn = input.ReadValueB8();
        }

        public override void Write(Stream output, uint version, Endian endian)
        {
            base.Write(output, version, endian);
            throw new NotImplementedException();
        }

        public class ArrayElement : SerializableArrayElement<BoltEmitterElement>
        {
        }
    }
}
