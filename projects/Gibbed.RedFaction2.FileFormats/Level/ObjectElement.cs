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

using System.IO;
using System.Text;
using Gibbed.IO;
using Newtonsoft.Json;

namespace Gibbed.RedFaction2.FileFormats.Level
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ObjectElement : ISerializableElement
    {
        #region Fields
        private int _Uid;
        private string _ClassName;
        private Vector3 _Position;
        private Transform _Transform;
        private string _ScriptName;
        private bool _IsHidden;
        #endregion

        #region Properties
        [JsonProperty("uid")]
        public int Uid
        {
            get { return this._Uid; }
            set { this._Uid = value; }
        }

        [JsonProperty("class_name")]
        public string ClassName
        {
            get { return this._ClassName; }
            set { this._ClassName = value; }
        }

        [JsonProperty("pos")]
        public Vector3 Position
        {
            get { return this._Position; }
            set { this._Position = value; }
        }

        [JsonProperty("transform")]
        public Transform Transform
        {
            get { return this._Transform; }
            set { this._Transform = value; }
        }

        [JsonProperty("script_name")]
        public string ScriptName
        {
            get { return this._ScriptName; }
            set { this._ScriptName = value; }
        }

        [JsonProperty("hidden")]
        public bool IsHidden
        {
            get { return this._IsHidden; }
            set { this._IsHidden = value; }
        }
        #endregion

        protected abstract ushort ClassNameMaximumLength { get; }
        protected abstract ushort ScriptNameMaximumLength { get; }

        public virtual void Read(Stream input, uint version, Endian endian)
        {
            this._Uid = input.ReadValueS32(endian);
            this._ClassName = input.ReadStringU16(this.ClassNameMaximumLength, Encoding.ASCII, endian);
            this._Position = Vector3.Read(input, endian);
            this._Transform = Transform.Read(input, endian);
            this._ScriptName = input.ReadStringU16(this.ScriptNameMaximumLength, Encoding.ASCII, endian);
            this._IsHidden = input.ReadValueB8();
        }

        public virtual void Write(Stream output, uint version, Endian endian)
        {
            output.WriteValueS32(this._Uid, endian);
            output.WriteStringU16(this._ClassName, this.ClassNameMaximumLength, Encoding.ASCII, endian);
            Vector3.Write(output, this._Position, endian);
            Transform.Write(output, this._Transform, endian);
            output.WriteStringU16(this._ScriptName, this.ScriptNameMaximumLength, Encoding.ASCII, endian);
            output.WriteValueB8(this._IsHidden);
        }
    }
}
