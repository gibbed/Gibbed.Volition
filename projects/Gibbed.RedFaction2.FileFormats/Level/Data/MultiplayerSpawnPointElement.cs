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
    public class MultiplayerSpawnPointElement : ISerializableElement
    {
        #region Fields
        private int _Uid;
        private Vector3 _Position;
        private Transform _Transform;
        private string _ScriptName;
        private bool _IsHidden;
        private int _TeamId;
        private bool _IsRedTeam;
        private bool _IsBlueTeam;
        private bool _IsBot;
        #endregion

        #region Properties
        public int Uid
        {
            get { return this._Uid; }
            set { this._Uid = value; }
        }

        public Vector3 Position
        {
            get { return this._Position; }
            set { this._Position = value; }
        }

        public Transform Transform
        {
            get { return this._Transform; }
            set { this._Transform = value; }
        }

        public string ScriptName
        {
            get { return this._ScriptName; }
            set { this._ScriptName = value; }
        }

        public bool IsHidden
        {
            get { return this._IsHidden; }
            set { this._IsHidden = value; }
        }

        public int TeamId
        {
            get { return this._TeamId; }
            set { this._TeamId = value; }
        }

        public bool IsRedTeam
        {
            get { return this._IsRedTeam; }
            set { this._IsRedTeam = value; }
        }

        public bool IsBlueTeam
        {
            get { return this._IsBlueTeam; }
            set { this._IsBlueTeam = value; }
        }

        public bool IsBot
        {
            get { return this._IsBot; }
            set { this._IsBot = value; }
        }
        #endregion

        public void Read(Stream input, uint version, Endian endian)
        {
            this._Uid = input.ReadValueS32(endian);
            this._Position = Vector3.Read(input, endian);
            this._Transform = Transform.Read(input, endian);
            this._ScriptName = input.ReadStringU16(32, Encoding.ASCII, endian);
            this._IsHidden = input.ReadValueB8();
            this._TeamId = input.ReadValueS32(endian);
            this._IsRedTeam = input.ReadValueB8();
            this._IsBlueTeam = input.ReadValueB8();
            this._IsBot = input.ReadValueB8();
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }

        public class ArrayElement : SerializableArrayElement<MultiplayerSpawnPointElement>
        {
        }
    }
}
