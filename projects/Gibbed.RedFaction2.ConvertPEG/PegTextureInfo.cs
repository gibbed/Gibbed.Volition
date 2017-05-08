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

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Peg = Gibbed.RedFaction2.FileFormats.Peg;

namespace Gibbed.RedFaction2.ConvertPEG
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class PegTextureInfo
    {
        [JsonProperty("width")]
        public ushort Width { get; set; }

        [JsonProperty("height")]
        public ushort Height { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "format")]
        public Peg.TextureFormat Format { get; set; }

        [JsonProperty("format_arg", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public byte FormatArgument { get; set; }

        [JsonProperty("flags")]
        public byte Flags { get; set; }

        [JsonProperty("anim_delay", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public byte AnimationDelay { get; set; }

        [JsonProperty("mip_count")]
        public byte MipCount { get; set; }

        [JsonProperty("unknown_a", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public byte UnknownA { get; set; }

        [JsonProperty("unknown_b", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public byte UnknownB { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("frame_paths", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string> FramePaths { get; set; }
    }
}
