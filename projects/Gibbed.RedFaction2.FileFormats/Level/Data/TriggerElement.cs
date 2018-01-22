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
using System.Collections.Generic;
using Gibbed.IO;
using Newtonsoft.Json;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TriggerElement : ISerializableElement
    {
        #region Fields
        private int _Uid;
        private string _ClassName;
        private bool _IsHidden;
        private TriggerShape _Shape;
        private float _TriggerResetsAfterSeconds;
        private int _TriggerResetCount;
        private bool _UseKeyRequiredToActivateTrigger;
        private bool _PlayerWeaponActivatesTrigger;
        private string _KeyName;
        private TriggerActivatedBy _ActivatedBy;
        private bool _IsNpcTrigger;
        private bool _IsAutoTrigger;
        private bool _PlayerInVehicle;
#if !RF1_HACK
        private bool _Unknown1;
#endif
        private Vector3 _Position;
        private float _Radius;
        private Transform _Transform;
        private Vector3 _BoxSize;
        private int _AirLockRoomUid;
        private int _AttachedToUid;
        private int _UseClutterUid;
        private bool _IsDisabled;
        private float _ButtonActiveTime;
        private float _InsideTime;
#if !RF1_HACK
        private bool _Unknown2;
#endif
        private bool _OneWay;
        private int _TeamId;
#if !RF1_HACK
        private byte _Unknown3;
        private byte _Unknown4;
#endif
        private readonly List<int> _Links;
        #endregion

        public TriggerElement()
        {
            this._Links = new List<int>();
        }

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

        [JsonProperty("hidden")]
        public bool IsHidden
        {
            get { return this._IsHidden; }
            set { this._IsHidden = value; }
        }

        [JsonProperty("shape")]
        public TriggerShape Shape
        {
            get { return this._Shape; }
            set { this._Shape = value; }
        }

        [JsonProperty("trigger_resets_after")]
        public float TriggerResetsAfterSeconds
        {
            get { return this._TriggerResetsAfterSeconds; }
            set { this._TriggerResetsAfterSeconds = value; }
        }

        [JsonProperty("trigger_reset_count")]
        public int TriggerResetCount
        {
            get { return this._TriggerResetCount; }
            set { this._TriggerResetCount = value; }
        }

        [JsonProperty("use_key_required_to_activate_trigger")]
        public bool UseKeyRequiredToActivateTrigger
        {
            get { return this._UseKeyRequiredToActivateTrigger; }
            set { this._UseKeyRequiredToActivateTrigger = value; }
        }

        [JsonProperty("player_weapon_activates_trigger")]
        public bool PlayerWeaponActivatesTrigger
        {
            get { return this._PlayerWeaponActivatesTrigger; }
            set { this._PlayerWeaponActivatesTrigger = value; }
        }

        [JsonProperty("key_name")]
        public string KeyName
        {
            get { return this._KeyName; }
            set { this._KeyName = value; }
        }

        [JsonProperty("activated_by")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public TriggerActivatedBy ActivatedBy
        {
            get { return this._ActivatedBy; }
            set { this._ActivatedBy = value; }
        }

        [JsonProperty("is_npc_trigger")]
        public bool IsNpcTrigger
        {
            get { return this._IsNpcTrigger; }
            set { this._IsNpcTrigger = value; }
        }

        [JsonProperty("is_auto_trigger")]
        public bool IsAutoTrigger
        {
            get { return this._IsAutoTrigger; }
            set { this._IsAutoTrigger = value; }
        }

        [JsonProperty("player_in_vehicle")]
        public bool PlayerInVehicle
        {
            get { return this._PlayerInVehicle; }
            set { this._PlayerInVehicle = value; }
        }

#if !RF1_HACK
        [JsonProperty("__u1")]
        public bool Unknown1
        {
            get { return this._Unknown1; }
            set { this._Unknown1 = value; }
        }
#endif

        [JsonProperty("pos")]
        public Vector3 Position
        {
            get { return this._Position; }
            set { this._Position = value; }
        }

        [JsonProperty("radius")]
        public float Radius
        {
            get { return this._Radius; }
            set { this._Radius = value; }
        }

        [JsonProperty("transform")]
        public Transform Transform
        {
            get { return this._Transform; }
            set { this._Transform = value; }
        }

        [JsonProperty("box_size")]
        public Vector3 BoxSize
        {
            get { return this._BoxSize; }
            set { this._BoxSize = value; }
        }

        [JsonProperty("airlock_room_uid")]
        public int AirLockRoomUid
        {
            get { return this._AirLockRoomUid; }
            set { this._AirLockRoomUid = value; }
        }

        [JsonProperty("attached_to_uid")]
        public int AttachedToUid
        {
            get { return this._AttachedToUid; }
            set { this._AttachedToUid = value; }
        }

        [JsonProperty("use_clutter_uid")]
        public int UseClutterUid
        {
            get { return this._UseClutterUid; }
            set { this._UseClutterUid = value; }
        }

        [JsonProperty("disabled")]
        public bool IsDisabled
        {
            get { return this._IsDisabled; }
            set { this._IsDisabled = value; }
        }

        [JsonProperty("button_active_time")]
        public float ButtonActiveTime
        {
            get { return this._ButtonActiveTime; }
            set { this._ButtonActiveTime = value; }
        }

        [JsonProperty("inside_time")]
        public float InsideTime
        {
            get { return this._InsideTime; }
            set { this._InsideTime = value; }
        }

#if !RF1_HACK
        [JsonProperty("__u2")]
        public bool Unknown2
        {
            get { return this._Unknown2; }
            set { this._Unknown2 = value; }
        }
#endif

        [JsonProperty("one_way")]
        public bool OneWay
        {
            get { return this._OneWay; }
            set { this._OneWay = value; }
        }

        [JsonProperty("team_id")]
        public int TeamId
        {
            get { return this._TeamId; }
            set { this._TeamId = value; }
        }

#if !RF1_HACK
        [JsonProperty("__u3")]
        public byte Unknown3
        {
            get { return this._Unknown3; }
            set { this._Unknown3 = value; }
        }

        [JsonProperty("__u4")]
        public byte Unknown4
        {
            get { return this._Unknown4; }
            set { this._Unknown4 = value; }
        }
#endif
        #endregion

        public void Read(Stream input, uint version, Endian endian)
        {
            this._Uid = input.ReadValueS32(endian);
            this._ClassName = input.ReadStringU16(128, Encoding.ASCII, endian);
            this._IsHidden = input.ReadValueB8();
            this._Shape = (TriggerShape)input.ReadValueU32(endian);
            this._TriggerResetsAfterSeconds = input.ReadValueF32(endian); // value * 1000.0f
            this._TriggerResetCount = input.ReadValueS32(endian);
            
            if (version < 245)
            {
                this._UseKeyRequiredToActivateTrigger = input.ReadValueB8();
            }

            this._KeyName = input.ReadStringU16(32, Encoding.ASCII, endian);

            if (version < 245)
            {
                this._PlayerWeaponActivatesTrigger = input.ReadValueB8();
            }

            this._ActivatedBy = (TriggerActivatedBy)input.ReadValueU8();

            if (version < 245)
            {
                this._IsNpcTrigger = input.ReadValueB8();
                this._IsAutoTrigger = input.ReadValueB8();
                this._PlayerInVehicle = input.ReadValueB8();
#if !RF1_HACK
                this._Unknown1 = input.ReadValueB8();
#endif
            }

            if (this._Shape == TriggerShape.Sphere)
            {
                this._Position = Vector3.Read(input, endian);
                this._Radius = input.ReadValueF32(endian);
            }
            else if (this._Shape == TriggerShape.Box)
            {
                this._Position = Vector3.Read(input, endian);
                this._Transform = Transform.Read(input, endian);
                this._BoxSize = Vector3.Read(input, endian);

                if (version < 245)
                {
                    this._OneWay = input.ReadValueB8() == true;
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            this._AirLockRoomUid = input.ReadValueS32(endian);
            this._AttachedToUid = input.ReadValueS32(endian);
            this._UseClutterUid = input.ReadValueS32(endian);

            if (version < 245)
            {
                this._IsDisabled = input.ReadValueB8();
            }

            this._ButtonActiveTime = input.ReadValueF32(endian);
            this._InsideTime = input.ReadValueF32(endian);

#if !RF1_HACK
            if (version < 245)
            {
                this._Unknown2 = input.ReadValueB8();
            }
#endif

            this._TeamId = input.ReadValueS32(endian);

#if !RF1_HACK
            if (version < 245)
            {
                var flags = input.ReadValueU8();
                this._Unknown3 = version <= 218 ? (byte)7u : (byte)((flags >> 5) & 7u);
                this._Unknown4 = (byte)(flags & 7u);
            }

            if (version >= 245)
            {
                var flags = input.ReadValueU32(endian);
                this._UseKeyRequiredToActivateTrigger = (flags & 0x001) != 0;
                this._PlayerWeaponActivatesTrigger = (flags & 0x002) != 0;
                this._IsNpcTrigger = (flags & 0x004) != 0;
                this._IsAutoTrigger = (flags & 0x008) != 0;
                this._IsDisabled = (flags & 0x010) != 0;
                this._OneWay = (flags & 0x020) != 0;
                this._Unknown1 = (flags & 0x080) != 0;
                this._PlayerInVehicle = (flags & 0x100) != 0;
                this._Unknown2 = (flags & 0x400) != 0;
                this._Unknown3 = (byte)((flags >> 16) & 7u);
                this._Unknown4 = (byte)((flags >> 12) & 7u);
            }
#endif

            var linkCount = input.ReadValueU32(endian);
            if (linkCount >= 16)
            {
                throw new FormatException();
            }
            this._Links.Clear();
            for (uint i = 0; i < linkCount; i++)
            {
                this._Links.Add(input.ReadValueS32(endian));
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            output.WriteValueS32(this._Uid, endian);
            output.WriteStringU16(this._ClassName,128, Encoding.ASCII, endian);
            output.WriteValueB8(this._IsHidden);
            output.WriteValueU32((uint)this._Shape, endian);
            output.WriteValueF32(this._TriggerResetsAfterSeconds, endian);
            output.WriteValueS32(this._TriggerResetCount, endian);

            if (version < 245)
            {
                output.WriteValueB8(this._UseKeyRequiredToActivateTrigger);
            }

            output.WriteStringU16(this._KeyName, 32, Encoding.ASCII, endian);

            if (version < 245)
            {
                output.WriteValueB8(this._PlayerWeaponActivatesTrigger);
            }

            output.WriteValueU8((byte)this._ActivatedBy);

            if (version < 245)
            {
                output.WriteValueB8(this._IsNpcTrigger);
                output.WriteValueB8(this._IsAutoTrigger);
                output.WriteValueB8(this._PlayerInVehicle);
#if !RF1_HACK
                output.WriteValueB8(this._Unknown1);
#endif
            }

            if (this._Shape == TriggerShape.Sphere)
            {
                Vector3.Write(output, this._Position, endian);
                output.WriteValueF32(this._Radius, endian);
            }
            else if (this._Shape == TriggerShape.Box)
            {
                Vector3.Write(output, this._Position, endian);
                Transform.Write(output, this._Transform, endian);
                Vector3.Write(output, this._BoxSize, endian);

                if (version < 245)
                {
                    output.WriteValueB8(this._OneWay);
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            output.WriteValueS32(this._AirLockRoomUid, endian);
            output.WriteValueS32(this._AttachedToUid, endian);
            output.WriteValueS32(this._UseClutterUid, endian);

            if (version < 245)
            {
                output.WriteValueB8(this._IsDisabled);
            }

            output.WriteValueF32(this._ButtonActiveTime, endian);
            output.WriteValueF32(this._InsideTime, endian);

#if !RF1_HACK
            if (version < 245)
            {
                output.WriteValueB8(this._Unknown2);
            }
#endif

            output.WriteValueS32(this._TeamId, endian);

#if !RF1_HACK
            if (version < 245)
            {
                byte flags = 0;
                if (version >= 219)
                {
                    flags |= (byte)((this._Unknown3 & 7u) << 5);
                }
                flags |= (byte)(this.Unknown4 & 7u);
                output.WriteValueU8(flags);
            }

            if (version >= 245)
            {
                uint flags = 0;
                if (this._UseKeyRequiredToActivateTrigger == true)
                {
                    flags |= 0x001;
                }
                if (this._PlayerWeaponActivatesTrigger == true)
                {
                    flags |= 0x002;
                }
                if (this._IsNpcTrigger == true)
                {
                    flags |= 0x004;
                }
                if (this._IsAutoTrigger == true)
                {
                    flags |= 0x008;
                }
                if (this._IsDisabled == true)
                {
                    flags |= 0x010;
                }
                if (this._OneWay == true)
                {
                    flags |= 0x020;
                }
                if (this._Unknown1 == true)
                {
                    flags |= 0x080;
                }
                if (this._PlayerInVehicle == true)
                {
                    flags |= 0x100;
                }
                if (this._Unknown2 == true)
                {
                    flags |= 0x400;
                }
                flags |= (this._Unknown3 & 7u) << 16;
                flags |= (this._Unknown4 & 7u) << 12;
                output.WriteValueU32(flags, endian);
            }
#endif

            if (this._Links.Count >= 16)
            {
                throw new FormatException();
            }
            output.WriteValueS32(this._Links.Count, endian);
            foreach (var link in this._Links)
            {
                output.WriteValueS32(link, endian);
            }
        }

        public class ArrayElement : SerializableArrayElement<TriggerElement>
        {
        }
    }
}
