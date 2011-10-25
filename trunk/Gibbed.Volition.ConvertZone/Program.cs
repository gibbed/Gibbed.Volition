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
using Gibbed.IO;
using Gibbed.Volition.FileFormats;

namespace Gibbed.Volition.ConvertZone
{
    internal class Program
    {
        private static void PrintCRC(string text)
        {
            Console.WriteLine("[{0:X8} {1:X8}] [{2:X8}] {3}",
                text.HashVolition(),
                text.HashVolition().Swap(),
                text.CrcVolition(),
                text);
        }

        public static void Main(string[] args)
        {
            PrintCRC("edit_obj");
            PrintCRC("obj_chunk_ref");
            PrintCRC("stitch_piece");
            PrintCRC("decal");
            PrintCRC("terrain_decal");
            PrintCRC("obj_zone");
            PrintCRC("layer");
            PrintCRC("terrain_decal");
            PrintCRC("navpoint");
            PrintCRC("constraint_point");
            PrintCRC("constraint_hinge");
            PrintCRC("constraint_prism");
            PrintCRC("constraint_motor");
            PrintCRC("object_guard_node");
            PrintCRC("object_action_node");
            PrintCRC("object_dummy");
            PrintCRC("object_raid_node");
            PrintCRC("object_house_arrest_node");
            PrintCRC("object_demolitions_master_node");
            PrintCRC("object_riding_shotgun_node");
            PrintCRC("object_area_defense_node");
            PrintCRC("object_npc_spawn_node");
            PrintCRC("object_squad_spawn_node");
            PrintCRC("object_activity_spawn");
            PrintCRC("object_vehicle_spawn_node");
            PrintCRC("object_turret_spawn_node");
            PrintCRC("object_roadblock_node");
            PrintCRC("object_convoy_end_point");
            PrintCRC("object_courier_end_point");
            PrintCRC("object_patrol");
            PrintCRC("object_delivery_node");
            PrintCRC("object_bftp_node");
            PrintCRC("item");
            PrintCRC("weapon");
            PrintCRC("cover_node");
            PrintCRC("obj_light");
            PrintCRC("player_start");
            PrintCRC("obj_occluder");
            PrintCRC("object_effect");
            PrintCRC("trigger_region");
            PrintCRC("marauder_ambush_region");
            PrintCRC("object_bounding_box");
            PrintCRC("object_spawn_region");
            PrintCRC("object_ambient_behavior_region");
            PrintCRC("multi_object_marker");
            PrintCRC("object_restricted_area");
            PrintCRC("shape_cutter");
            PrintCRC("ladder");
            PrintCRC("magnet");
            PrintCRC("navmesh_cutout");
            PrintCRC("object_path_road");
            PrintCRC("object_road_path");
            PrintCRC("invisible_wall");
            PrintCRC("collision_box");
            PrintCRC("note");
            PrintCRC("multi_object_flag");
            PrintCRC("object_mission_start_node");
            PrintCRC("object_safehouse");
            PrintCRC("object_foliage");
            PrintCRC("force_field_door");
            PrintCRC("object_upgrade_node");
            PrintCRC("effect_streaming_node");
            PrintCRC("sound_spline");
            PrintCRC("spawn_resource_data");
            PrintCRC("e_zone");
            PrintCRC("e_ref");
            PrintCRC("constraint_hindge");
            PrintCRC("object_squad");
            PrintCRC("turret");
            PrintCRC("safehouse");
            PrintCRC("district");
            PrintCRC("zone_flags");
            PrintCRC("terrain_file_name");
            PrintCRC("wind_min_speed");
            PrintCRC("wind_max_speed");
            PrintCRC("zone_flags");
            PrintCRC("ambient_spawn");
        }
    }
}
