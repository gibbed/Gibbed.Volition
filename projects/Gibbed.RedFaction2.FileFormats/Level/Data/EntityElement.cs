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
using System.Text;
using Gibbed.IO;
using Newtonsoft.Json;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EntityElement : ObjectElement
    {
        #region Fields
        private uint _Cooperation;
        private uint _Friendliness;
        private uint _Unknown1;
        private int _TeamId;
        private string _WaypointList;
        private string _WaypointMethod;
        private bool _RunWaypointList;
        private bool _StartHidden;
        private bool _WearHelmet;
        private bool _EndGameIfKilled;
        private bool _CowerFromWeapon;
        private bool _QuestionUnarmedPlayer;
        private bool _DoNotHum;
        private bool _NoShadow;
        private bool _AlwaysSimulate;
        private bool _HasPerfectAim;
        private bool _PermanentCorpse;
        private bool _NeverFlee;
        private bool _NeverLeave;
        private bool _NoPersonaMessages;
        private bool _Unknown2;
        private bool _FadeCorpseImmediately;
        private bool _NeverCollideWithPlayer;
        private bool _UseCustomAttackRange;
        private bool _Unknown19;
        private bool _IsBoarded;
        private bool _ReadyToFireState;
        private bool _OnlyAttackPlayer;
        private bool _WeaponIsHolstered;
        private bool _IsDeaf;
        private bool _IgnoreTerrainWhenFiring;
        private bool _Unknown20;
        private bool _StartCrouched;
        private bool _Unknown3;
        private byte _Unknown4;
        private bool _Unknown5;
        private bool _Unknown6;
        private bool _Unknown7;
        private bool _Unknown8;
        private bool _Unknown9;
        private bool _Unknown10;
        private bool _Unknown11;
        private bool _Unknown12;
        private bool _Unknown13;
        private bool _Unknown14;
        private bool _Unknown15;
        private sbyte _Unknown16;
        private bool _Unknown17;
        private bool _Unknown18;
        private int _SweepMinimumAngle;
        private int _SweepMaximumAngle;
        private float _Life;
        private float _Armor;
        private float _Unknown21;
        private float _Unknown22;
        private uint _FieldOfView;
        private string _DefaultPrimary;
        private string _DefaultSecondary;
        private string _ItemDrop;
        private string _StateAnimation;
        private string _CorpsePose;
        private string _Skin;
        private string _DeathAnimation;
        private string _Unknown23;
        private EntityAIMode _AIMode;
        private EntityAIAttackStyle _AIAttackStyle;
        private readonly List<KeyValuePair<string, uint>> _Unknown24;
        private int _TurretUid;
        private int _AlertCameraUid;
        private int _AlarmEventUid;
        private float _CustomAttackRange;
        private float _Unknown25;
        private string _LeftHandHolding;
        private string _RightHandHolding;
        private int _Unknown26;
        private int _Unknown27;
        private float _Unknown28;
        private float _Unknown29;
        private float _Unknown30;
        private bool _Unknown31;
        private readonly List<KeyValuePair<string, string>> _Unknown32;
        #endregion

        public EntityElement()
        {
            this._Unknown24 = new List<KeyValuePair<string, uint>>();
            this._Unknown32 = new List<KeyValuePair<string, string>>();
        }

        protected override ushort ClassNameMaximumLength
        {
            get { return ushort.MaxValue; }
        }

        protected override ushort ScriptNameMaximumLength
        {
            get { return 32; }
        }

        #region Properties
        [JsonProperty("cooperation")]
        public uint Cooperation
        {
            get { return this._Cooperation; }
            set { this._Cooperation = value; }
        }

        [JsonProperty("friendliness")]
        public uint Friendliness
        {
            get { return this._Friendliness; }
            set { this._Friendliness = value; }
        }

        [JsonProperty("__u1")]
        public uint Unknown1
        {
            get { return this._Unknown1; }
            set { this._Unknown1 = value; }
        }

        [JsonProperty("team_id")]
        public int TeamId
        {
            get { return this._TeamId; }
            set { this._TeamId = value; }
        }

        [JsonProperty("waypoint_list")]
        public string WaypointList
        {
            get { return this._WaypointList; }
            set { this._WaypointList = value; }
        }

        [JsonProperty("waypoint_method")]
        public string WaypointMethod
        {
            get { return this._WaypointMethod; }
            set { this._WaypointMethod = value; }
        }

        [JsonProperty("run_waypoint_list")]
        public bool RunWaypointList
        {
            get { return this._RunWaypointList; }
            set { this._RunWaypointList = value; }
        }

        [JsonProperty("start_hidden")]
        public bool StartHidden
        {
            get { return this._StartHidden; }
            set { this._StartHidden = value; }
        }

        [JsonProperty("wear_helmet")]
        public bool WearHelmet
        {
            get { return this._WearHelmet; }
            set { this._WearHelmet = value; }
        }

        [JsonProperty("end_game_if_killed")]
        public bool EndGameIfKilled
        {
            get { return this._EndGameIfKilled; }
            set { this._EndGameIfKilled = value; }
        }

        [JsonProperty("cower_from_weapon")]
        public bool CowerFromWeapon
        {
            get { return this._CowerFromWeapon; }
            set { this._CowerFromWeapon = value; }
        }

        [JsonProperty("question_unarmed_player")]
        public bool QuestionUnarmedPlayer
        {
            get { return this._QuestionUnarmedPlayer; }
            set { this._QuestionUnarmedPlayer = value; }
        }

        [JsonProperty("do_not_hum")]
        public bool DoNotHum
        {
            get { return this._DoNotHum; }
            set { this._DoNotHum = value; }
        }

        [JsonProperty("no_shadow")]
        public bool NoShadow
        {
            get { return this._NoShadow; }
            set { this._NoShadow = value; }
        }

        [JsonProperty("always_simulate")]
        public bool AlwaysSimulate
        {
            get { return this._AlwaysSimulate; }
            set { this._AlwaysSimulate = value; }
        }

        [JsonProperty("has_perfect_aim")]
        public bool HasPerfectAim
        {
            get { return this._HasPerfectAim; }
            set { this._HasPerfectAim = value; }
        }

        [JsonProperty("permanent_corpse")]
        public bool PermanentCorpse
        {
            get { return this._PermanentCorpse; }
            set { this._PermanentCorpse = value; }
        }

        [JsonProperty("never_flee")]
        public bool NeverFlee
        {
            get { return this._NeverFlee; }
            set { this._NeverFlee = value; }
        }

        [JsonProperty("never_leave")]
        public bool NeverLeave
        {
            get { return this._NeverLeave; }
            set { this._NeverLeave = value; }
        }

        [JsonProperty("no_persona_messages")]
        public bool NoPersonaMessages
        {
            get { return this._NoPersonaMessages; }
            set { this._NoPersonaMessages = value; }
        }

        [JsonProperty("__u2")]
        public bool Unknown2
        {
            get { return this._Unknown2; }
            set { this._Unknown2 = value; }
        }

        [JsonProperty("fade_corpse_immediately")]
        public bool FadeCorpseImmediately
        {
            get { return this._FadeCorpseImmediately; }
            set { this._FadeCorpseImmediately = value; }
        }

        [JsonProperty("never_collide_with_player")]
        public bool NeverCollideWithPlayer
        {
            get { return this._NeverCollideWithPlayer; }
            set { this._NeverCollideWithPlayer = value; }
        }

        [JsonProperty("use_custom_attack_range")]
        public bool UseCustomAttackRange
        {
            get { return this._UseCustomAttackRange; }
            set { this._UseCustomAttackRange = value; }
        }

        [JsonProperty("__u19")]
        public bool Unknown19
        {
            get { return this._Unknown19; }
            set { this._Unknown19 = value; }
        }

        [JsonProperty("boarded")]
        public bool IsBoarded
        {
            get { return this._IsBoarded; }
            set { this._IsBoarded = value; }
        }

        [JsonProperty("ready_to_fire_state")]
        public bool ReadyToFireState
        {
            get { return this._ReadyToFireState; }
            set { this._ReadyToFireState = value; }
        }

        [JsonProperty("only_attack_player")]
        public bool OnlyAttackPlayer
        {
            get { return this._OnlyAttackPlayer; }
            set { this._OnlyAttackPlayer = value; }
        }

        [JsonProperty("weapon_is_holstered")]
        public bool WeaponIsHolstered
        {
            get { return this._WeaponIsHolstered; }
            set { this._WeaponIsHolstered = value; }
        }

        [JsonProperty("deaf")]
        public bool IsDeaf
        {
            get { return this._IsDeaf; }
            set { this._IsDeaf = value; }
        }

        [JsonProperty("ignore_terrain_when_firing")]
        public bool IgnoreTerrainWhenFiring
        {
            get { return this._IgnoreTerrainWhenFiring; }
            set { this._IgnoreTerrainWhenFiring = value; }
        }

        [JsonProperty("__u20")]
        public bool Unknown20
        {
            get { return this._Unknown20; }
            set { this._Unknown20 = value; }
        }

        [JsonProperty("start_crouched")]
        public bool StartCrouched
        {
            get { return this._StartCrouched; }
            set { this._StartCrouched = value; }
        }

        [JsonProperty("__u3")]
        public bool Unknown3
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

        [JsonProperty("__u5")]
        public bool Unknown5
        {
            get { return this._Unknown5; }
            set { this._Unknown5 = value; }
        }

        [JsonProperty("__u6")]
        public bool Unknown6
        {
            get { return this._Unknown6; }
            set { this._Unknown6 = value; }
        }

        [JsonProperty("__u7")]
        public bool Unknown7
        {
            get { return this._Unknown7; }
            set { this._Unknown7 = value; }
        }

        [JsonProperty("__u8")]
        public bool Unknown8
        {
            get { return this._Unknown8; }
            set { this._Unknown8 = value; }
        }

        [JsonProperty("__u9")]
        public bool Unknown9
        {
            get { return this._Unknown9; }
            set { this._Unknown9 = value; }
        }

        [JsonProperty("__u10")]
        public bool Unknown10
        {
            get { return this._Unknown10; }
            set { this._Unknown10 = value; }
        }

        [JsonProperty("__u11")]
        public bool Unknown11
        {
            get { return this._Unknown11; }
            set { this._Unknown11 = value; }
        }

        [JsonProperty("__u12")]
        public bool Unknown12
        {
            get { return this._Unknown12; }
            set { this._Unknown12 = value; }
        }

        [JsonProperty("__u13")]
        public bool Unknown13
        {
            get { return this._Unknown13; }
            set { this._Unknown13 = value; }
        }

        [JsonProperty("__u14")]
        public bool Unknown14
        {
            get { return this._Unknown14; }
            set { this._Unknown14 = value; }
        }

        [JsonProperty("__u15")]
        public bool Unknown15
        {
            get { return this._Unknown15; }
            set { this._Unknown15 = value; }
        }

        [JsonProperty("__u16")]
        public sbyte Unknown16
        {
            get { return this._Unknown16; }
            set { this._Unknown16 = value; }
        }

        [JsonProperty("__u17")]
        public bool Unknown17
        {
            get { return this._Unknown17; }
            set { this._Unknown17 = value; }
        }

        [JsonProperty("__u18")]
        public bool Unknown18
        {
            get { return this._Unknown18; }
            set { this._Unknown18 = value; }
        }

        [JsonProperty("sweep_min_angle")]
        public int SweepMinimumAngle
        {
            get { return this._SweepMinimumAngle; }
            set { this._SweepMinimumAngle = value; }
        }

        [JsonProperty("sweep_max_angle")]
        public int SweepMaximumAngle
        {
            get { return this._SweepMaximumAngle; }
            set { this._SweepMaximumAngle = value; }
        }

        [JsonProperty("life")]
        public float Life
        {
            get { return this._Life; }
            set { this._Life = value; }
        }

        [JsonProperty("armor")]
        public float Armor
        {
            get { return this._Armor; }
            set { this._Armor = value; }
        }

        [JsonProperty("__u21")]
        public float Unknown21
        {
            get { return this._Unknown21; }
            set { this._Unknown21 = value; }
        }

        [JsonProperty("__u22")]
        public float Unknown22
        {
            get { return this._Unknown22; }
            set { this._Unknown22 = value; }
        }

        [JsonProperty("fov")]
        public uint FieldOfView
        {
            get { return this._FieldOfView; }
            set { this._FieldOfView = value; }
        }

        [JsonProperty("default_primary")]
        public string DefaultPrimary
        {
            get { return this._DefaultPrimary; }
            set { this._DefaultPrimary = value; }
        }

        [JsonProperty("default_secondary")]
        public string DefaultSecondary
        {
            get { return this._DefaultSecondary; }
            set { this._DefaultSecondary = value; }
        }

        [JsonProperty("item_drop")]
        public string ItemDrop
        {
            get { return this._ItemDrop; }
            set { this._ItemDrop = value; }
        }

        [JsonProperty("state_anim")]
        public string StateAnimation
        {
            get { return this._StateAnimation; }
            set { this._StateAnimation = value; }
        }

        [JsonProperty("corpse_pose")]
        public string CorpsePose
        {
            get { return this._CorpsePose; }
            set { this._CorpsePose = value; }
        }

        [JsonProperty("skin")]
        public string Skin
        {
            get { return this._Skin; }
            set { this._Skin = value; }
        }

        [JsonProperty("death_anim")]
        public string DeathAnimation
        {
            get { return this._DeathAnimation; }
            set { this._DeathAnimation = value; }
        }

        [JsonProperty("__u23")]
        public string Unknown23
        {
            get { return this._Unknown23; }
            set { this._Unknown23 = value; }
        }

        [JsonProperty("ai_mode")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public EntityAIMode AIMode
        {
            get { return this._AIMode; }
            set { this._AIMode = value; }
        }

        [JsonProperty("ai_attack_style")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public EntityAIAttackStyle AIAttackStyle
        {
            get { return this._AIAttackStyle; }
            set { this._AIAttackStyle = value; }
        }

        [JsonProperty("__u24")]
        public List<KeyValuePair<string, uint>> Unknown24
        {
            get { return this._Unknown24; }
        }

        [JsonProperty("turret_uid")]
        public int TurretUid
        {
            get { return this._TurretUid; }
            set { this._TurretUid = value; }
        }

        [JsonProperty("alert_camera_uid")]
        public int AlertCameraUid
        {
            get { return this._AlertCameraUid; }
            set { this._AlertCameraUid = value; }
        }

        [JsonProperty("alarm_event_uid")]
        public int AlarmEventUid
        {
            get { return this._AlarmEventUid; }
            set { this._AlarmEventUid = value; }
        }

        [JsonProperty("custom_attack_range")]
        public float CustomAttackRange
        {
            get { return this._CustomAttackRange; }
            set { this._CustomAttackRange = value; }
        }

        [JsonProperty("__u25")]
        public float Unknown25
        {
            get { return this._Unknown25; }
            set { this._Unknown25 = value; }
        }

        [JsonProperty("left_hand_holding")]
        public string LeftHandHolding
        {
            get { return this._LeftHandHolding; }
            set { this._LeftHandHolding = value; }
        }

        [JsonProperty("right_hand_holding")]
        public string RightHandHolding
        {
            get { return this._RightHandHolding; }
            set { this._RightHandHolding = value; }
        }

        [JsonProperty("__u26")]
        public int Unknown26
        {
            get { return this._Unknown26; }
            set { this._Unknown26 = value; }
        }

        [JsonProperty("__u27")]
        public int Unknown27
        {
            get { return this._Unknown27; }
            set { this._Unknown27 = value; }
        }

        [JsonProperty("__u28")]
        public float Unknown28
        {
            get { return this._Unknown28; }
            set { this._Unknown28 = value; }
        }

        [JsonProperty("__u29")]
        public float Unknown29
        {
            get { return this._Unknown29; }
            set { this._Unknown29 = value; }
        }

        [JsonProperty("__u30")]
        public float Unknown30
        {
            get { return this._Unknown30; }
            set { this._Unknown30 = value; }
        }

        [JsonProperty("__u31")]
        public bool Unknown31
        {
            get { return this._Unknown31; }
            set { this._Unknown31 = value; }
        }

        [JsonProperty("__u32")]
        public List<KeyValuePair<string, string>> Unknown32
        {
            get { return this._Unknown32; }
        }
        #endregion

        public override void Read(Stream input, uint version, Endian endian)
        {
            base.Read(input, version, endian);

            this._Cooperation = input.ReadValueU32(endian);
            this._Friendliness = input.ReadValueU32(endian);
            this._Unknown1 = version >= 274 ? input.ReadValueU32(endian) : 0;
            this._TeamId = input.ReadValueS32(endian);
            this._WaypointList = input.ReadStringU16(32, Encoding.ASCII, endian);
            this._WaypointMethod = input.ReadStringU16(32, Encoding.ASCII, endian);

            if (version >= 248)
            {
                var flags = input.ReadValueU32(endian);
                var flags2 = version >= 263 ? input.ReadValueU32(endian) : 0;

                this._RunWaypointList = (flags & (1u << 0)) != 0;
                this._StartHidden = (flags & (1u << 1)) != 0;
                this._EndGameIfKilled = (flags & (1u << 3)) != 0;
                this._CowerFromWeapon = (flags & (1u << 4)) != 0;
                this._QuestionUnarmedPlayer = (flags & (1u << 5)) != 0;
                this._DoNotHum = (flags & (1u << 6)) != 0;
                this._NoShadow = (flags & (1u << 7)) != 0;
                this._AlwaysSimulate = (flags & (1u << 8)) != 0;
                this._HasPerfectAim = (flags & (1u << 9)) != 0;
                this._PermanentCorpse = (flags & (1u << 10)) != 0;
                this._NeverFlee = (flags & (1u << 11)) != 0;
                this._NeverLeave = (flags & (1u << 12)) != 0;
                this._NoPersonaMessages = (flags & (1u << 13)) != 0;
                this._Unknown2 = (flags & (1u << 14)) != 0;
                this._FadeCorpseImmediately = (flags & (1u << 15)) != 0;
                this._NeverCollideWithPlayer = (flags & (1u << 16)) != 0;
                this._UseCustomAttackRange = (flags & (1u << 17)) != 0;
                this._IsBoarded = (flags & (1u << 19)) != 0;
                this._ReadyToFireState = (flags & (1u << 20)) != 0;
                this._OnlyAttackPlayer = (flags & (1u << 21)) != 0;
                this._IsDeaf = (flags & (1u << 23)) != 0;
                this._IgnoreTerrainWhenFiring = (flags & (1u << 24)) != 0;
                this._StartCrouched = (flags & (1u << 25)) != 0;
                this._Unknown3 = (flags & (1u << 26)) != 0;
                this._Unknown4 = (byte)((flags >> 28) & 3);
                this._Unknown5 = (flags & (1u << 31)) != 0;
                this._Unknown6 = (flags2 & (1u << 1)) != 0;
                this._Unknown7 = (flags & (1u << 30)) != 0;
                this._Unknown8 = (flags2 & (1u << 2)) != 0;
                this._Unknown9 = (flags2 & (1u << 3)) != 0;
                this._Unknown10 = (flags2 & (1u << 4)) != 0;
                this._Unknown11 = (flags2 & (1u << 0)) != 0;
                this._Unknown12 = (flags2 & (1u << 6)) != 0;
                this._Unknown13 = (flags2 & (1u << 7)) != 0;
                this._Unknown14 = (flags2 & (1u << 8)) != 0;
                this._Unknown15 = (flags2 & (1u << 9)) != 0;
                this._Unknown16 = (sbyte)((sbyte)((flags2 >> 10) & 7) - 1);
                this._Unknown17 = (flags2 & (1u << 13)) != 0;
                this._Unknown18 = (flags2 & (1u << 14)) != 0;

                if (this._Unknown11 == true)
                {
                    this._Unknown16 = 1;
                }
                else
                {
                    this._Unknown11 = this._Unknown16 >= 0;
                }
            }
            else
            {
                this._Unknown19 = input.ReadValueB8();
                this._IsBoarded = input.ReadValueB8();
                this._ReadyToFireState = input.ReadValueB8();
                this._OnlyAttackPlayer = input.ReadValueB8();
                this._WeaponIsHolstered = input.ReadValueB8();
                this._IsDeaf = input.ReadValueB8();
                this._Unknown3 = false;
                this._Unknown7 = false;
                this._Unknown11 = false;
                this._Unknown12 = false;
                this._Unknown13 = false;
                this._Unknown14 = false;
                this._Unknown15 = false;
            }

            this._SweepMinimumAngle = input.ReadValueS32(endian);
            this._SweepMaximumAngle = input.ReadValueS32(endian);

            if (version < 248)
            {
                this._IgnoreTerrainWhenFiring = input.ReadValueB8();
                this._Unknown20 = input.ReadValueB8();
                this._StartCrouched = input.ReadValueB8();
            }

            this._Life = input.ReadValueF32(endian);
            this._Armor = input.ReadValueF32(endian);
            this._Unknown21 = version >= 242 ? input.ReadValueF32(endian) : 100.0f;
            this._Unknown22 = version >= 242 ? input.ReadValueF32(endian) : 100.0f;
            this._FieldOfView = input.ReadValueU32(endian);
            this._DefaultPrimary = input.ReadStringU16(32, Encoding.ASCII, endian);
            this._DefaultSecondary = input.ReadStringU16(32, Encoding.ASCII, endian);
            this._ItemDrop = input.ReadStringU16(32, Encoding.ASCII, endian);
            this._StateAnimation = input.ReadStringU16(32, Encoding.ASCII, endian);
            this._CorpsePose = input.ReadStringU16(32, Encoding.ASCII, endian);
            this._Skin = input.ReadStringU16(32, Encoding.ASCII, endian);
            this._DeathAnimation = input.ReadStringU16(32, Encoding.ASCII, endian);
            this._Unknown23 = version >= 254 ? input.ReadStringU16(32, Encoding.ASCII, endian) : "";
            this._AIMode = (EntityAIMode)input.ReadValueU8();
            this._AIAttackStyle = (EntityAIAttackStyle)input.ReadValueU8();

            this._Unknown24.Clear();
            if (version < 232)
            {
                var unknown24Count = input.ReadValueU32(endian);
                for (uint i = 0; i < unknown24Count; i++)
                {
                    var unknown24Key = input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
                    var unknown24Value = input.ReadValueU32(endian);
                    this._Unknown24.Add(new KeyValuePair<string, uint>(unknown24Key, unknown24Value));
                }
            }

            this._TurretUid = input.ReadValueS32(endian);
            this._AlertCameraUid = input.ReadValueS32(endian);
            this._AlarmEventUid = input.ReadValueS32(endian);

            if (version < 248)
            {
                this._RunWaypointList = input.ReadValueB8();
                this._StartHidden = input.ReadValueB8();
                this._WearHelmet = input.ReadValueB8();
                this._EndGameIfKilled = input.ReadValueB8();
                this._CowerFromWeapon = input.ReadValueB8();
                this._QuestionUnarmedPlayer = input.ReadValueB8();
                this._DoNotHum = input.ReadValueB8();
                this._NoShadow = input.ReadValueB8();
                this._AlwaysSimulate = input.ReadValueB8();
                this._HasPerfectAim = input.ReadValueB8();
                this._PermanentCorpse = input.ReadValueB8();
                this._NeverFlee = input.ReadValueB8();
                this._NeverLeave = input.ReadValueB8();
                this._NoPersonaMessages = input.ReadValueB8();
                this._Unknown2 = version >= 246 && input.ReadValueB8() == true;
                this._FadeCorpseImmediately = input.ReadValueB8();
                this._NeverCollideWithPlayer = input.ReadValueB8();
                this._UseCustomAttackRange = input.ReadValueB8();
            }

            this._CustomAttackRange = this._UseCustomAttackRange == true ? input.ReadValueF32(endian) : 15.0f;
            this._Unknown25 = this._UseCustomAttackRange == true && version >= 214 ? input.ReadValueF32(endian) : 0.0f;

            if (version < 254)
            {
                this._LeftHandHolding = input.ReadStringU16(128, Encoding.ASCII, endian);
                this._RightHandHolding = input.ReadStringU16(128, Encoding.ASCII, endian);
            }

            this._Unknown26 = version >= 218 ? input.ReadValueS32(endian) : -1;
            this._Unknown27 = version >= 218 ? input.ReadValueU8() : 0;
            this._Unknown28 = version >= 253 ? input.ReadValueF32(endian) : 10.0f;
            this._Unknown29 = version >= 255 ? input.ReadValueF32(endian) : 1.0f;
            this._Unknown30 = version >= 281 ? input.ReadValueF32(endian) : 1.0f;
            this._Unknown31 = version >= 241 && version <= 247 && input.ReadValueB8() == true;

            this._Unknown32.Clear();
            if (version >= 239)
            {
                var unknown32Count = version >= 267 ? 19 : 18;
                for (uint i = 0; i < unknown32Count; i++)
                {
                    var unknown32Key = input.ReadStringU16(128, Encoding.ASCII, endian);
                    var unknown32Value = input.ReadStringU16(23, Encoding.ASCII, endian);
                    this._Unknown32.Add(new KeyValuePair<string, string>(unknown32Key, unknown32Value));
                }
            }
        }

        public override void Write(Stream output, uint version, Endian endian)
        {
            base.Write(output, version, endian);
            throw new NotImplementedException();
        }

        public class ArrayElement : SerializableArrayElement<EntityElement>
        {
        }
    }
}
