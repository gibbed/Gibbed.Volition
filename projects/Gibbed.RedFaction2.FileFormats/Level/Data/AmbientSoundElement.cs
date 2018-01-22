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

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AmbientSoundElement : ISerializableElement
    {
        #region Fields
        private int _Uid;
        private Vector3 _Position;
        private bool _PlayInEditor;
        private string _SoundFileName;
        private float _MinimumDistance;
        private float _VolumeScale;
        private float _Rolloff;
        private uint _StartDelay;
        #endregion

        #region Properties
        [JsonProperty("uid")]
        public int Uid
        {
            get { return this._Uid; }
            set { this._Uid = value; }
        }

        [JsonProperty("pos")]
        public Vector3 Position
        {
            get { return this._Position; }
            set { this._Position = value; }
        }

        [JsonProperty("play_in_editor")]
        public bool PlayInEditor
        {
            get { return this._PlayInEditor; }
            set { this._PlayInEditor = value; }
        }

        [JsonProperty("sound_file")]
        public string SoundFileName
        {
            get { return this._SoundFileName; }
            set { this._SoundFileName = value; }
        }

        [JsonProperty("min_dist")]
        public float MinimumDistance
        {
            get { return this._MinimumDistance; }
            set { this._MinimumDistance = value; }
        }

        [JsonProperty("vol_scale")]
        public float VolumeScale
        {
            get { return this._VolumeScale; }
            set { this._VolumeScale = value; }
        }

        [JsonProperty("rolloff")]
        public float Rolloff
        {
            get { return this._Rolloff; }
            set { this._Rolloff = value; }
        }

        [JsonProperty("start_delay")]
        public uint StartDelay
        {
            get { return this._StartDelay; }
            set { this._StartDelay = value; }
        }
        #endregion

        public void Read(Stream input, uint version, Endian endian)
        {
            this._Uid = input.ReadValueS32(endian);
            this._Position = Vector3.Read(input, endian);
            this._PlayInEditor = input.ReadValueB8();
            this._SoundFileName = input.ReadStringU16(32, Encoding.ASCII, endian);
            this._MinimumDistance = input.ReadValueF32(endian);
            this._VolumeScale = input.ReadValueF32(endian);
            this._Rolloff = input.ReadValueF32(endian);
            this._StartDelay = input.ReadValueU32(endian);
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            output.WriteValueS32(this._Uid, endian);
            Vector3.Write(output, this._Position, endian);
            output.WriteValueB8(this._PlayInEditor);
            output.WriteStringU16(this._SoundFileName, 32, Encoding.ASCII, endian);
            output.WriteValueF32(this._MinimumDistance, endian);
            output.WriteValueF32(this._VolumeScale, endian);
            output.WriteValueF32(this._Rolloff, endian);
            output.WriteValueU32(this._StartDelay, endian);
        }

        public class ArrayElement : SerializableArrayElement<AmbientSoundElement>
        {
        }
    }
}
