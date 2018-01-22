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
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CutsceneElement : ISerializableElement
    {
        #region Fields
        private int _Uid;
        private bool _HidePlayerDuringCutscene;
        private float _FieldOfView;
        private string _Unknown3;
        private readonly List<Camera> _Cameras;
        #endregion

        public CutsceneElement()
        {
            this._Cameras = new List<Camera>();
        }

        #region Properties
        [JsonProperty("uid")]
        public int Uid
        {
            get { return this._Uid; }
            set { this._Uid = value; }
        }

        [JsonProperty("hide_player_during_cutscene")]
        public bool HidePlayerDuringCutscene
        {
            get { return this._HidePlayerDuringCutscene; }
            set { this._HidePlayerDuringCutscene = value; }
        }

        [JsonProperty("fov")]
        public float FieldOfView
        {
            get { return this._FieldOfView; }
            set { this._FieldOfView = value; }
        }

        [JsonProperty("__u3")]
        public string Unknown3
        {
            get { return this._Unknown3; }
            set { this._Unknown3 = value; }
        }

        [JsonProperty("cameras")]
        public List<Camera> Cameras
        {
            get { return this._Cameras; }
        }
        #endregion

        public void Read(Stream input, uint version, Endian endian)
        {
            this._Uid = input.ReadValueS32(endian);
            this._HidePlayerDuringCutscene = input.ReadValueB8();
            this._FieldOfView = input.ReadValueF32(endian);
            this._Unknown3 = version >= 291 ? input.ReadStringU16(64, Encoding.ASCII, endian) : "";
            
            var cameraCount = input.ReadValueU32(endian);
            this._Cameras.Clear();
            for (uint i = 0; i < cameraCount; i++)
            {
                var camera = new Camera();
                camera.Read(input, version, endian);
                this._Cameras.Add(camera);
            }
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            throw new NotImplementedException();
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class Camera : ISerializableElement
        {
            #region Fields
            private uint _Uid;
            private float _PreWait;
            private float _PathTime;
            private float _PostWait;
            private int _LookAtUid;
            private int _TriggerUid;
#if !RF1_HACK
            private int _Unknown11;
            private float _Unknown12;
#else
            private string _PathName;
#endif
            #endregion

            #region Properties
            [JsonProperty("uid")]
            public uint Uid
            {
                get { return this._Uid; }
                set { this._Uid = value; }
            }

            [JsonProperty("pre_wait")]
            public float PreWait
            {
                get { return this._PreWait; }
                set { this._PreWait = value; }
            }

            [JsonProperty("path_time")]
            public float PathTime
            {
                get { return this._PathTime; }
                set { this._PathTime = value; }
            }

            [JsonProperty("post_wait")]
            public float PostWait
            {
                get { return this._PostWait; }
                set { this._PostWait = value; }
            }

            [JsonProperty("look_at_uid")]
            public int LookAtUid
            {
                get { return this._LookAtUid; }
                set { this._LookAtUid = value; }
            }

            [JsonProperty("trigger_uid")]
            public int TriggerUid
            {
                get { return this._TriggerUid; }
                set { this._TriggerUid = value; }
            }

#if !RF1_HACK
            [JsonProperty("__u11")]
            public int Unknown11
            {
                get { return this._Unknown11; }
                set { this._Unknown11 = value; }
            }

            [JsonProperty("__u12")]
            public float Unknown12
            {
                get { return this._Unknown12; }
                set { this._Unknown12 = value; }
            }
#else
            [JsonProperty("path_name")]
            public string PathName
            {
                get { return this._PathName; }
                set { this._PathName = value; }
            }
#endif
            #endregion

            public void Read(Stream input, uint version, Endian endian)
            {
                this._Uid = input.ReadValueU32(endian);
                this._PreWait = input.ReadValueF32(endian);
                this._PathTime = input.ReadValueF32(endian);
                this._PostWait = input.ReadValueF32(endian);
                this._LookAtUid = input.ReadValueS32(endian);
                this._TriggerUid = input.ReadValueS32(endian);
#if !RF1_HACK
                this._Unknown11 = input.ReadValueS32(endian);
                this._Unknown12 = input.ReadValueF32(endian);
#else
                this._PathName = input.ReadStringU16(ushort.MaxValue, Encoding.ASCII, endian);
#endif
            }

            public void Write(Stream output, uint version, Endian endian)
            {
                throw new NotImplementedException();
            }
        }

        public class ArrayElement : SerializableArrayElement<CutsceneElement>
        {
        }
    }
}
