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
    public class MoverElement : ISerializableElement
    {
        #region Fields
        private string _GroupName;
        private bool _Unknown1;
        private int _Uid;
        private bool _Unknown3;
        private readonly List<MoverKeyframeElement> _Keyframes;
        private readonly List<Tuple<uint, Vector3, Transform>> _Unknown17; 
        private bool _IsDoor;
        private bool _RotateInPlace;
        private bool _Unknown23;
        private bool _StartsMovingBackward;
        private bool _UseTravelTimeAsVelocity;
        private bool _ForceOrientation;
        private bool _NoPlayerCollide;
        private MoverMovementType _MovementType;
        private int _StartingKeyframe; 
        private string _StartSoundFileName;
        private float _StartSoundVolume;
        private string _LoopingSoundFileName;
        private float _LoopingSoundVolume;
        private string _StopSoundFileName;
        private float _StopSoundVolume;
        private string _CloseSoundFileName;
        private float _CloseSoundVolume;
        private int _Unknown32;
        private readonly List<int> _Unknown33;
        private readonly List<int> _Unknown34;
        private int _Unknown35;
        private Vector3 _Unknown36;
        private Transform _Unknown37;
        private uint _Unknown38;
        private readonly int[] _Unknown39;
        #endregion

        public MoverElement()
        {
            this._Keyframes = new List<MoverKeyframeElement>();
            this._Unknown17 = new List<Tuple<uint, Vector3, Transform>>();
            this._Unknown33 = new List<int>();
            this._Unknown34 = new List<int>();
            this._Unknown39 = new int[8];
        }

        #region Properties
        [JsonProperty("group_name")]
        public string GroupName
        {
            get { return this._GroupName; }
            set { this._GroupName = value; }
        }

        [JsonProperty("__u1")]
        public bool Unknown1
        {
            get { return this._Unknown1; }
            set { this._Unknown1 = value; }
        }

        [JsonProperty("uid")]
        public int Uid
        {
            get { return this._Uid; }
            set { this._Uid = value; }
        }

        [JsonProperty("__u3")]
        public bool Unknown3
        {
            get { return this._Unknown3; }
            set { this._Unknown3 = value; }
        }

        [JsonProperty("keyframes")]
        public List<MoverKeyframeElement> Keyframes
        {
            get { return this._Keyframes; }
        }

        [JsonProperty("__u17")]
        public List<Tuple<uint, Vector3, Transform>> Unknown17
        {
            get { return this._Unknown17; }
        }

        [JsonProperty("is_door")]
        public bool IsDoor
        {
            get { return this._IsDoor; }
            set { this._IsDoor = value; }
        }

        [JsonProperty("rotate_in_place")]
        public bool RotateInPlace
        {
            get { return this._RotateInPlace; }
            set { this._RotateInPlace = value; }
        }

        [JsonProperty("__u23")]
        public bool Unknown23
        {
            get { return this._Unknown23; }
            set { this._Unknown23 = value; }
        }

        [JsonProperty("starts_moving_backward")]
        public bool StartsMovingBackward
        {
            get { return this._StartsMovingBackward; }
            set { this._StartsMovingBackward = value; }
        }

        [JsonProperty("use_travel_time_as_velocity")]
        public bool UseTravelTimeAsVelocity
        {
            get { return this._UseTravelTimeAsVelocity; }
            set { this._UseTravelTimeAsVelocity = value; }
        }

        [JsonProperty("force_orientation")]
        public bool ForceOrientation
        {
            get { return this._ForceOrientation; }
            set { this._ForceOrientation = value; }
        }

        [JsonProperty("no_player_collide")]
        public bool NoPlayerCollide
        {
            get { return this._NoPlayerCollide; }
            set { this._NoPlayerCollide = value; }
        }

        [JsonProperty("movement_type")]
        public MoverMovementType MovementType
        {
            get { return this._MovementType; }
            set { this._MovementType = value; }
        }

        [JsonProperty("starting_keyframe")]
        public int StartingKeyframe
        {
            get { return this._StartingKeyframe; }
            set { this._StartingKeyframe = value; }
        }

        [JsonProperty("start_sound_file")]
        public string StartSoundFileName
        {
            get { return this._StartSoundFileName; }
            set { this._StartSoundFileName = value; }
        }

        [JsonProperty("start_sound_vol")]
        public float StartSoundVolume
        {
            get { return this._StartSoundVolume; }
            set { this._StartSoundVolume = value; }
        }

        [JsonProperty("looping_sound_file")]
        public string LoopingSoundFileName
        {
            get { return this._LoopingSoundFileName; }
            set { this._LoopingSoundFileName = value; }
        }

        [JsonProperty("looping_sound_vol")]
        public float LoopingSoundVolume
        {
            get { return this._LoopingSoundVolume; }
            set { this._LoopingSoundVolume = value; }
        }

        [JsonProperty("stop_sound_file")]
        public string StopSoundFileName
        {
            get { return this._StopSoundFileName; }
            set { this._StopSoundFileName = value; }
        }

        [JsonProperty("stop_sound_vol")]
        public float StopSoundVolume
        {
            get { return this._StopSoundVolume; }
            set { this._StopSoundVolume = value; }
        }

        [JsonProperty("close_sound_file")]
        public string CloseSoundFileName
        {
            get { return this._CloseSoundFileName; }
            set { this._CloseSoundFileName = value; }
        }

        [JsonProperty("close_sound_vol")]
        public float CloseSoundVolume
        {
            get { return this._CloseSoundVolume; }
            set { this._CloseSoundVolume = value; }
        }

        [JsonProperty("__u32")]
        public int Unknown32
        {
            get { return this._Unknown32; }
            set { this._Unknown32 = value; }
        }

        [JsonProperty("__u33")]
        public List<int> Unknown33
        {
            get { return this._Unknown33; }
        }

        [JsonProperty("__u34")]
        public List<int> Unknown34
        {
            get { return this._Unknown34; }
        }

        [JsonProperty("__u35")]
        public int Unknown35
        {
            get { return this._Unknown35; }
            set { this._Unknown35 = value; }
        }

        [JsonProperty("__u36")]
        public Vector3 Unknown36
        {
            get { return this._Unknown36; }
            set { this._Unknown36 = value; }
        }

        [JsonProperty("__u37")]
        public Transform Unknown37
        {
            get { return this._Unknown37; }
            set { this._Unknown37 = value; }
        }

        [JsonProperty("__u38")]
        public uint Unknown38
        {
            get { return this._Unknown38; }
            set { this._Unknown38 = value; }
        }

        [JsonProperty("__u39")]
        public int[] Unknown39
        {
            get { return this._Unknown39; }
        }
        #endregion

        public void Read(Stream input, uint version, Endian endian)
        {
            this._GroupName = input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
            this._Unknown1 = input.ReadValueB8();
            this._Uid = version >= 207 ? input.ReadValueS32(endian) : -1;
            this._Unknown3 = input.ReadValueB8();

            var keyframeCount = input.ReadValueU32(endian);
            var keyframes = new MoverKeyframeElement[keyframeCount];
            for (uint i = 0; i < keyframeCount; i++)
            {
                var keyframe = new MoverKeyframeElement();
                keyframe.Read(input, version, endian);
                keyframes[i] = keyframe;
            }
            this._Keyframes.Clear();
            this._Keyframes.AddRange(keyframes);

            var unknown17Count = input.ReadValueU32(endian);
            this._Unknown17.Clear();
            for (uint i = 0; i < unknown17Count; i++)
            {
                var unknown17Item1 = input.ReadValueU32(endian);
                var unknown17Item2 = Vector3.Read(input, endian);
                var unknown17Item3 = Transform.Read(input, endian);
                this._Unknown17.Add(new Tuple<uint, Vector3, Transform>(unknown17Item1, unknown17Item2, unknown17Item3));
            }

            this._IsDoor = input.ReadValueB8();
            this._RotateInPlace = input.ReadValueB8();
            this._Unknown23 = version >= 205 && input.ReadValueB8() == true;
            this._StartsMovingBackward = input.ReadValueB8();
            this._UseTravelTimeAsVelocity = input.ReadValueB8();
            this._ForceOrientation = input.ReadValueB8();
            this._NoPlayerCollide = input.ReadValueB8();
            this._MovementType = (MoverMovementType)input.ReadValueU32(endian);
            this._StartingKeyframe = input.ReadValueS32(endian);
            this._StartSoundFileName = input.ReadStringU16(64, Encoding.ASCII, endian);
            this._StartSoundVolume = input.ReadValueF32(endian);
            this._LoopingSoundFileName = input.ReadStringU16(64, Encoding.ASCII, endian);
            this._LoopingSoundVolume = input.ReadValueF32(endian);
            this._StopSoundFileName = input.ReadStringU16(64, Encoding.ASCII, endian);
            this._StopSoundVolume = input.ReadValueF32(endian);
            this._CloseSoundFileName = input.ReadStringU16(64, Encoding.ASCII, endian);
            this._CloseSoundVolume = input.ReadValueF32(endian);
            this._Unknown32 = version >= 211 ? input.ReadValueS32(endian) : -1;

            var unknown33Count = input.ReadValueU32(endian);
            this._Unknown33.Clear();
            for (uint i = 0; i < unknown33Count; i++)
            {
                this._Unknown33.Add(input.ReadValueS32(endian));
            }

            var unknown34Count = input.ReadValueU32(endian);
            this._Unknown34.Clear();
            for (uint i = 0; i < unknown34Count; i++)
            {
                this._Unknown34.Add(input.ReadValueS32(endian));
            }

#if RF1_HACK
            if (version >= 201)
#endif
            {
                this._Unknown35 = input.ReadValueS32(endian);
                if (this._Unknown35 != -1)
                {
                    this._Unknown36 = Vector3.Read(input, endian);
                    this._Unknown37 = Transform.Read(input, endian);
                }

                this._Unknown38 = version >= 207 ? input.ReadValueU32(endian) : 0;
                
                for (uint i = 0; i < 8; i++)
                {
                    this._Unknown39[i] = version >= 207 ? input.ReadValueS32(endian) : -1;
                }
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }

        public class ArrayElement : SerializableArrayElement<MoverElement>
        {
        }
    }
}
