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
    public class MoverKeyframeElement : ISerializableElement
    {
        #region Fields
        private int _Uid;
        private Vector3 _Position;
        private Transform _Transform;
        private string _ScriptName;
        private bool _IsHidden;
        private float _PauseTime;
        private float _DepartingTravelTime;
        private float _ReturningTravelTime;
        private float _AccelerationTime;
        private float _DecelerationTime;
        private int _TriggerEventWithUid;
        private int _ContainsItemUid1;
        private int _ContainsItemUid2;
        private float _RotationDegreesAboutAxis;
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

        [JsonProperty("pause_time")]
        public float PauseTime
        {
            get { return this._PauseTime; }
            set { this._PauseTime = value; }
        }

        [JsonProperty("departing_travel_time")]
        public float DepartingTravelTime
        {
            get { return this._DepartingTravelTime; }
            set { this._DepartingTravelTime = value; }
        }

        [JsonProperty("returning_travel_time")]
        public float ReturningTravelTime
        {
            get { return this._ReturningTravelTime; }
            set { this._ReturningTravelTime = value; }
        }

        [JsonProperty("acceleration_time")]
        public float AccelerationTime
        {
            get { return this._AccelerationTime; }
            set { this._AccelerationTime = value; }
        }

        [JsonProperty("deceleration_time")]
        public float DecelerationTime
        {
            get { return this._DecelerationTime; }
            set { this._DecelerationTime = value; }
        }

        [JsonProperty("trigger_event_with_uid")]
        public int TriggerEventWithUid
        {
            get { return this._TriggerEventWithUid; }
            set { this._TriggerEventWithUid = value; }
        }

        [JsonProperty("contains_item_uid_1")]
        public int ContainsItemUid1
        {
            get { return this._ContainsItemUid1; }
            set { this._ContainsItemUid1 = value; }
        }

        [JsonProperty("contains_item_uid_2")]
        public int ContainsItemUid2
        {
            get { return this._ContainsItemUid2; }
            set { this._ContainsItemUid2 = value; }
        }

        [JsonProperty("rotation_degrees_about_axis")]
        public float RotationDegreesAboutAxis
        {
            get { return this._RotationDegreesAboutAxis; }
            set { this._RotationDegreesAboutAxis = value; }
        }
        #endregion

        public void Read(Stream input, uint version, Endian endian)
        {
            this._Uid = input.ReadValueS32(endian);
            this._Position = Vector3.Read(input, endian);
            this._Transform = Transform.Read(input, endian);
            this._ScriptName = input.ReadStringU16(128, Encoding.ASCII, endian);
            this._IsHidden = input.ReadValueB8();
            this._PauseTime = input.ReadValueF32(endian);
            this._DepartingTravelTime = input.ReadValueF32(endian);
            this._ReturningTravelTime = input.ReadValueF32(endian);
            this._AccelerationTime = input.ReadValueF32(endian);
            this._DecelerationTime = input.ReadValueF32(endian);
            this._TriggerEventWithUid = input.ReadValueS32(endian);
            this._ContainsItemUid1 = input.ReadValueS32(endian);
            this._ContainsItemUid2 = input.ReadValueS32(endian);
            this._RotationDegreesAboutAxis = input.ReadValueF32(endian); // -(value * 0.017453292f)
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            output.WriteValueS32(this._Uid, endian);
            Vector3.Write(output, this._Position, endian);
            Transform.Write(output, this._Transform, endian);
            output.WriteStringU16(this._ScriptName, 128, Encoding.ASCII, endian);
            output.WriteValueB8(this._IsHidden);
            output.WriteValueF32(this._PauseTime, endian);
            output.WriteValueF32(this._DepartingTravelTime, endian);
            output.WriteValueF32(this._ReturningTravelTime, endian);
            output.WriteValueF32(this._AccelerationTime, endian);
            output.WriteValueF32(this._DecelerationTime, endian);
            output.WriteValueS32(this._TriggerEventWithUid, endian);
            output.WriteValueS32(this._ContainsItemUid1, endian);
            output.WriteValueS32(this._ContainsItemUid2, endian);
            output.WriteValueF32(this._RotationDegreesAboutAxis, endian);
        }
    }
}
