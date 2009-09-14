using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gibbed.Volition.FileFormats.Vint
{
    public static class PropertyNames
    {
        private static Dictionary<UInt32, string> GenerateLookup()
        {
            Dictionary<UInt32, string> hashes = new Dictionary<uint, string>();
            string[] table = new string[]
			{
				// element
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
				"unscaled_size",

				// group
				"screen_size",
				"screen_se",
				"screen_nw",
				"offset",

				// animation
				"start_time",
				"is_paused",
				"target_handle",

				// tween
				"target_handle",
				"target_name",
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

				// bitmap
				"custom_source_coords",
				"image",
				"image_crc",
				"source_nw",
				"source_se",

				// gradient
				"gradient_nw",
				"gradient_ne",
				"gradient_sw",
				"gradient_se",
				"alpha_nw",
				"alpha_ne",
				"alpha_sw",
				"alpha_se",

				// text
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

				// point
				"screen_size",

				// clip
				"clip_size",
				"clip_enabled",

				// bitmap_circle
				"image",
				"screen_size",
				"source_nw",
				"source_se",
				"start_angle",
				"end_angle",
				"num_wedges",

				// sr2_map
				"zoom",
				"map_mode",

				// video
				"vid_id_handle",
			};


            foreach (string item in table)
            {
                //hashes[item.KeyCRC32()] = item;
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

            if (hash != 4234861048)
            {
                throw new Exception();
            }

            return "(unknown property : 0x" + hash.ToString("X8") + ")";
        }
    }
}
