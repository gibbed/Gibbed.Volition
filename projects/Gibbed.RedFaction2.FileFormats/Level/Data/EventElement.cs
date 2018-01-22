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
    public class EventElement : ISerializableElement
    {
        #region Fields
        private int _Uid;
        private string _ClassName = "";
        private Vector3 _Position;
        private string _ScriptName = "";
        private bool _IsHidden;
        private float _Delay;
        private int _Unknown6 = -1;
        private int _Unknown7;
        private bool _BoolArgument1;
        private bool _BoolArgument2;
        private int _IntArgument1;
        private int _IntArgument2;
        private int _IntArgument3 = -1;
        private int _IntArgument4 = -1;
        private float _FloatArgument1;
        private float _FloatArgument2;
        private float _FloatArgument3;
        private string _StringArgument1 = "";
        private string _StringArgument2 = "";
        private string _StringArgument3 = "";
        private readonly List<int> _Links;
        private Transform _Transform;
        private Color _EventColor;
        #endregion

        public EventElement()
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

        [JsonProperty("pos")]
        public Vector3 Position
        {
            get { return this._Position; }
            set { this._Position = value; }
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

        [JsonProperty("delay")]
        public float Delay
        {
            get { return this._Delay; }
            set { this._Delay = value; }
        }

        [JsonProperty("__u6")]
        public int Unknown6
        {
            get { return this._Unknown6; }
            set { this._Unknown6 = value; }
        }

        [JsonProperty("__u7")]
        public int Unknown7
        {
            get { return this._Unknown7; }
            set { this._Unknown7 = value; }
        }

        [JsonProperty("bool_arg_1")]
        public bool BoolArgument1
        {
            get { return this._BoolArgument1; }
            set { this._BoolArgument1 = value; }
        }

        [JsonProperty("bool_arg_2")]
        public bool BoolArgument2
        {
            get { return this._BoolArgument2; }
            set { this._BoolArgument2 = value; }
        }

        [JsonProperty("int_arg_1")]
        public int IntArgument1
        {
            get { return this._IntArgument1; }
            set { this._IntArgument1 = value; }
        }

        [JsonProperty("int_arg_2")]
        public int IntArgument2
        {
            get { return this._IntArgument2; }
            set { this._IntArgument2 = value; }
        }

        [JsonProperty("int_arg_3")]
        public int IntArgument3
        {
            get { return this._IntArgument3; }
            set { this._IntArgument3 = value; }
        }

        [JsonProperty("int_arg_4")]
        public int IntArgument4
        {
            get { return this._IntArgument4; }
            set { this._IntArgument4 = value; }
        }

        [JsonProperty("float_arg_1")]
        public float FloatArgument1
        {
            get { return this._FloatArgument1; }
            set { this._FloatArgument1 = value; }
        }

        [JsonProperty("float_arg_2")]
        public float FloatArgument2
        {
            get { return this._FloatArgument2; }
            set { this._FloatArgument2 = value; }
        }

        [JsonProperty("float_arg_3")]
        public float FloatArgument3
        {
            get { return this._FloatArgument3; }
            set { this._FloatArgument3 = value; }
        }

        [JsonProperty("str_arg_1")]
        public string StringArgument1
        {
            get { return this._StringArgument1; }
            set { this._StringArgument1 = value; }
        }

        [JsonProperty("str_arg_2")]
        public string StringArgument2
        {
            get { return this._StringArgument2; }
            set { this._StringArgument2 = value; }
        }

        [JsonProperty("str_arg_3")]
        public string StringArgument3
        {
            get { return this._StringArgument3; }
            set { this._StringArgument3 = value; }
        }

        [JsonProperty("links")]
        public List<int> Links
        {
            get { return this._Links; }
        }

        [JsonProperty("transform")]
        public Transform Transform
        {
            get { return this._Transform; }
            set { this._Transform = value; }
        }

        [JsonProperty("event_color")]
        public Color EventColor
        {
            get { return this._EventColor; }
            set { this._EventColor = value; }
        }
        #endregion

        public void Read(Stream input, uint version, Endian endian)
        {
            this._Uid = input.ReadValueS32(endian);
            this._ClassName = input.ReadStringU16(32, Encoding.ASCII, endian);
            this._Position = Vector3.Read(input, endian);
            this._ScriptName = input.ReadStringU16(128, Encoding.ASCII, endian);
            this._IsHidden = input.ReadValueB8();
            this._Delay = input.ReadValueF32(endian);
            this._Unknown6 = version >= 210 ? input.ReadValueS32(endian) : -1;
            this._Unknown7 = version >= 268 ? input.ReadValueS32(endian) : 0;
            this._BoolArgument1 = input.ReadValueB8();
            this._BoolArgument2 = input.ReadValueB8();
            this._IntArgument1 = input.ReadValueS32(endian);
            this._IntArgument2 = input.ReadValueS32(endian);
            this._IntArgument3 = version >= 282 ? input.ReadValueS32(endian) : -1;
            this._IntArgument4 = version >= 283 ? input.ReadValueS32(endian) : -1;
            this._FloatArgument1 = input.ReadValueF32(endian);
            this._FloatArgument2 = input.ReadValueF32(endian);
            this._FloatArgument3 = version >= 237 ? input.ReadValueF32(endian) : 0.0f;
            this._StringArgument1 = input.ReadStringU16(64, Encoding.ASCII, endian);
            this._StringArgument2 = input.ReadStringU16(64, Encoding.ASCII, endian);
            this._StringArgument3 = version >= 237 ? input.ReadStringU16(64, Encoding.ASCII, endian) : "";

            var linkCount = input.ReadValueU32(endian);
            if (linkCount > 8)
            {
                throw new FormatException("too many links from event");
            }

            this._Links.Clear();
            for (uint i = 0; i < linkCount; i++)
            {
                this._Links.Add(input.ReadValueS32(endian));
            }

            if (this._ClassName == "Teleport" || this._ClassName == "Alarm" || this._ClassName == "Play_Explosion")
            {
                this._Transform = Transform.Read(input, endian);
            }

            this._EventColor = Color.Read(input);
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            output.WriteValueS32(this._Uid, endian);
            output.WriteStringU16(this._ClassName, 32, Encoding.ASCII, endian);
            Vector3.Write(output, this._Position, endian);
            output.WriteStringU16(this._ScriptName, 128, Encoding.ASCII, endian);
            output.WriteValueB8(this._IsHidden);
            output.WriteValueF32(this._Delay, endian);

            if (version >= 210)
            {
                output.WriteValueS32(this._Unknown6, endian);
            }

            if (version >= 268)
            {
                output.WriteValueS32(this._Unknown7, endian);
            }

            output.WriteValueB8(this._BoolArgument1);
            output.WriteValueB8(this._BoolArgument2);
            output.WriteValueS32(this._IntArgument1, endian);
            output.WriteValueS32(this._IntArgument2, endian);

            if (version >= 282)
            {
                output.WriteValueS32(this._IntArgument3, endian);
            }

            if (version >= 283)
            {
                output.WriteValueS32(this._IntArgument4, endian);
            }

            output.WriteValueF32(this._FloatArgument1, endian);
            output.WriteValueF32(this._FloatArgument2, endian);

            if (version >= 237)
            {
                output.WriteValueF32(this._FloatArgument3, endian);
            }

            output.WriteStringU16(this._StringArgument1, 64, Encoding.ASCII, endian);
            output.WriteStringU16(this._StringArgument2, 64, Encoding.ASCII, endian);

            if (version >= 237)
            {
                output.WriteStringU16(this._StringArgument3, 64, Encoding.ASCII, endian);
            }

            if (this._Links.Count > 8)
            {
                throw new FormatException("too many links from event");
            }

            output.WriteValueS32(this._Links.Count, endian);
            foreach (var link in this._Links)
            {
                output.WriteValueS32(link, endian);
            }

            if (this._ClassName == "Teleport" || this._ClassName == "Alarm" || this._ClassName == "Play_Explosion")
            {
                Transform.Write(output, this._Transform, endian);
            }

            Color.Write(output, this._EventColor);
        }

        public class ArrayElement : SerializableArrayElement<EventElement>
        {
        }
    }
}
