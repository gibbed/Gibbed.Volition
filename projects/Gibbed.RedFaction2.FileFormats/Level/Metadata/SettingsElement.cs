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

namespace Gibbed.RedFaction2.FileFormats.Level.Metadata
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SettingsElement : BasicElement
    {
        #region Fields
        private string _DefaultTexture;
        private int _Hardness;
        private Color _AmbientLightColor;
        private bool _Unknown3;
        private Color _DistanceBasedFogColor;
        private float _DistanceBasedFogNearClipPlane;
        private float _DistanceBasedFogFarClipPlane;
        private Color _Unknown7;
        private byte _Unknown8;
        #endregion

        #region Properties
        [JsonProperty("default_texture")]
        public string DefaultTexture
        {
            get { return this._DefaultTexture; }
            set { this._DefaultTexture = value; }
        }

        [JsonProperty("hardness")]
        public int Hardness
        {
            get { return this._Hardness; }
            set { this._Hardness = value; }
        }

        [JsonProperty("ambient_light_color")]
        public Color AmbientLightColor
        {
            get { return this._AmbientLightColor; }
            set { this._AmbientLightColor = value; }
        }

        [JsonProperty("__u3")]
        public bool Unknown3
        {
            get { return this._Unknown3; }
            set { this._Unknown3 = value; }
        }

        [JsonProperty("dist_based_fog_color")]
        public Color DistanceBasedFogColor
        {
            get { return this._DistanceBasedFogColor; }
            set { this._DistanceBasedFogColor = value; }
        }

        [JsonProperty("dist_based_fog_near_clip_plane")]
        public float DistanceBasedFogNearClipPlane
        {
            get { return this._DistanceBasedFogNearClipPlane; }
            set { this._DistanceBasedFogNearClipPlane = value; }
        }

        [JsonProperty("dist_based_fog_far_clip_plane")]
        public float DistanceBasedFogFarClipPlane
        {
            get { return this._DistanceBasedFogFarClipPlane; }
            set { this._DistanceBasedFogFarClipPlane = value; }
        }

        [JsonProperty("__u7")]
        public Color Unknown7
        {
            get { return this._Unknown7; }
            set { this._Unknown7 = value; }
        }

        [JsonProperty("__u8")]
        public byte Unknown8
        {
            get { return this._Unknown8; }
            set { this._Unknown8 = value; }
        }
        #endregion

        public override void Read(Stream input, uint version, Endian endian)
        {
            this._DefaultTexture = input.ReadStringU16(40, Encoding.ASCII, endian);
            this._Hardness = input.ReadValueS32(endian);
            this._AmbientLightColor = Color.Read(input);
            this._Unknown3 = input.ReadValueB8();
            this._DistanceBasedFogColor = Color.Read(input);
            this._DistanceBasedFogNearClipPlane = input.ReadValueF32(endian);
            this._DistanceBasedFogFarClipPlane = input.ReadValueF32(endian);

            if (version >= 247)
            {
                input.Seek(4, SeekOrigin.Current); // color
                input.Seek(4, SeekOrigin.Current); // float
                input.Seek(4, SeekOrigin.Current); // float
            }

            if (version >= 272)
            {
                input.Seek(4, SeekOrigin.Current); // float
            }

            if (version >= 258)
            {
                input.Seek(4, SeekOrigin.Current); // float
            }

            if (version >= 270)
            {
                this._Unknown7 = Color.Read(input);
                this._Unknown8 = (byte)input.ReadValueU32(endian);
            }

            if (version >= 287)
            {
                input.Seek(4, SeekOrigin.Current); // float
            }
        }

        public override void Write(Stream output, uint version, Endian endian)
        {
            output.WriteStringU16(this._DefaultTexture, 40, Encoding.ASCII, endian);
            output.WriteValueS32(this._Hardness, endian);
            Color.Write(output, this._AmbientLightColor);
            output.WriteValueB8(this._Unknown3);
            Color.Write(output, this._DistanceBasedFogColor);
            output.WriteValueF32(this._DistanceBasedFogNearClipPlane, endian);
            output.WriteValueF32(this._DistanceBasedFogFarClipPlane, endian);

            if (version >= 247)
            {
                output.WriteValueU32(0, endian); // color
                output.WriteValueU32(0, endian); // float
                output.WriteValueU32(0, endian); // float
            }

            if (version >= 272)
            {
                output.WriteValueU32(0, endian); // float
            }

            if (version >= 258)
            {
                output.WriteValueU32(0, endian); // float
            }

            if (version >= 270)
            {
                Color.Write(output, this._Unknown7);
                output.WriteValueU32((uint)this._Unknown8, endian);
            }

            if (version >= 287)
            {
                output.WriteValueU32(0, endian); // float
            }
        }
    }
}
