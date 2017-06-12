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

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    public class NavPointElement : ISerializableElement
    {
        #region Fields
        private uint _Uid;
        private bool _IsHidden;
        private float _Height;
        private Vector3 _Position;
        private float _Radius;
        private NavPointFlags _Flags;
        private Transform _Transform;
        private float _Unknown7;
        private float _PauseTime;
        private int _Unknown9;
        private readonly List<uint> _LinkUids;
        #endregion

        public NavPointElement()
        {
            this._LinkUids = new List<uint>();
        }

        #region Properties
        public uint Uid
        {
            get { return this._Uid; }
            set { this._Uid = value; }
        }

        public bool IsHidden
        {
            get { return this._IsHidden; }
            set { this._IsHidden = value; }
        }

        public float Height
        {
            get { return this._Height; }
            set { this._Height = value; }
        }

        public Vector3 Position
        {
            get { return this._Position; }
            set { this._Position = value; }
        }

        public float Radius
        {
            get { return this._Radius; }
            set { this._Radius = value; }
        }

        public NavPointFlags Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public Transform Transform
        {
            get { return this._Transform; }
            set { this._Transform = value; }
        }

        public float Unknown7
        {
            get { return this._Unknown7; }
            set { this._Unknown7 = value; }
        }

        public float PauseTime
        {
            get { return this._PauseTime; }
            set { this._PauseTime = value; }
        }

        public int Unknown9
        {
            get { return this._Unknown9; }
            set { this._Unknown9 = value; }
        }

        public List<uint> LinkUids
        {
            get { return this._LinkUids; }
        }
        #endregion

        public void Read(Stream input, uint version, Endian endian)
        {
            this._Uid = input.ReadValueU32(endian);
            this._IsHidden = input.ReadValueB8();
            this._Height = input.ReadValueF32(endian);
            this._Position = Vector3.Read(input, endian);
            this._Radius = input.ReadValueF32(endian);

            var flags = NavPointFlags.None;

            if (version < 213)
            {
                var type = input.ReadValueU32(endian);
                if (type != 0 && type != 1)
                {
                    throw new FormatException();
                }

                if (type == 1)
                {
                    flags |= NavPointFlags.Flying;
                }

                if (input.ReadValueB8() == true)
                {
                    flags |= NavPointFlags.Directional;
                }
            }
            else
            {
                flags = (NavPointFlags)input.ReadValueU32(endian);

                if (version < 249)
                {
                    flags |= (NavPointFlags)0xE000;
                }
            }

            if ((flags & NavPointFlags.Unknown12) != 0)
            {
                this._Radius = 0.0f;
            }

            if ((flags & NavPointFlags.Directional) != 0)
            {
                this._Transform = Transform.Read(input, endian);
                this._Unknown7 = version >= 214 ? input.ReadValueF32(endian) : 90.0f; // cos(value * 0.5 * 0.017453292)
            }

            if (version < 213)
            {
                if (input.ReadValueB8() == true)
                {
                    flags |= NavPointFlags.Cover;
                }

                //var hide = input.ReadValueB8();
                input.Seek(1, SeekOrigin.Current); // bool

                if (input.ReadValueB8() == true)
                {
                    flags |= NavPointFlags.Crouch;
                }
            }

            this._PauseTime = input.ReadValueF32(endian);
            this._Unknown9 = version >= 277 ? input.ReadValueS32(endian) : -1;

            if (version >= 225 && version <= 232)
            {
                input.Seek(4, SeekOrigin.Current); // uint
            }

            var linkCount = input.ReadValueU32(endian);
            if (linkCount > 4)
            {
                // "too many links (max %d) from nav uid: %d"
                throw new FormatException();
            }

            var linkUids = new uint[linkCount];
            for (uint i = 0; i < linkCount; i++)
            {
                linkUids[i] = input.ReadValueU32(endian);
            }
            this._LinkUids.Clear();
            this._LinkUids.AddRange(linkUids);

            this._Flags = flags;
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }
    }
}
