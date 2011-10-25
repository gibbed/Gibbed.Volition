/* Copyright (c) 2011 Rick (rick 'at' gibbed 'dot' us)
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
using Gibbed.IO;

namespace Gibbed.Volition.FileFormats.Vint
{
    public static class PropertyNames
    {
        private static Dictionary<UInt32, string> GenerateLookup()
        {
            Dictionary<UInt32, string> hashes = new Dictionary<uint, string>();
            string[] table = new string[]
			{
                // don't know what this one is from
                "app_visible",

				// element, in RFG, SR2
				"render_mode",
				"visible",
				"mask",
				"offset",
				"anchor",
				"tint",
				"alpha",
				"depth",
				"screen_size",
				"screen_nw",
				"screen_se",
				"rotation",
				"scale",
				"auto_offset",
                "unscaled_size", // Only in SR2?

				// group, in RFG, SR2
				"screen_size",
				"screen_se",
				"screen_nw",
				"offset",

				// animation, in RFG, SR2
				"start_time",
				"is_paused",
				"target_handle",

				// tween, in RFG, SR2
				"target_handle",
				"target_name",
                "target_name_crc", // Only in RFG?
				"target_property",
				"state",
				"start_time",
				"duration",
				"start_value",
				"end_value",
				"loop_mode",
				"algorithm",
				"max_loops",
				"start_event",
				"end_event",
				"per_frame_event",
				"start_value_type",
				"end_value_type",

				// bitmap, in RFG, SR2
				"custom_source_coords",
				"image",
				"image_crc",
                "image_raw", // Only in RFG?
                "image_badge", // Only in RFG?
				"source_nw",
				"source_se",

				// gradient, in SR2
				"gradient_nw",
				"gradient_ne",
				"gradient_sw",
				"gradient_se",
				"alpha_nw",
				"alpha_ne",
				"alpha_sw",
				"alpha_se",

				// text, in RFG, SR2
				"font",
				"text_tag",
				"text_tag_crc",
				"text_scale",
				"word_wrap",
				"wrap_width",
				"vert_align",
				"horz_align",
				"force_case",
				"leading",
				"insert_values",
				"screen_size",
				"line_frame_enable",
				"line_frame_w",
				"line_frame_m",
				"line_frame_e",

				// point, in RFG, SR2
				"screen_size",

				// clip, in RFG, SR2
				"clip_size",
				"clip_enabled",

				// bitmap_circle, in RFG, SR2
				"image",
				"screen_size",
				"source_nw",
				"source_se",
				"start_angle",
				"end_angle",
				"num_wedges",

                // map, in RFG
                "map_bogus",

				// sr2_map, in SR2
				"zoom",
				"map_mode",

				// video, in RFG, SR2
				"vid_id_handle",

                // framebuffer, in RFG
                "texture_handle",
			};

            foreach (string item in table)
            {
                hashes[item.CrcVolition()] = item;
            }

            return hashes;
        }
        private static Dictionary<UInt32, string> Table = GenerateLookup();
        public static string Lookup(UInt32 hash)
        {
            if (Table.ContainsKey(hash))
            {
                return Table[hash];
            }

            return "(unknown property : 0x" + hash.ToString("X8") + ")";
        }
    }
}
