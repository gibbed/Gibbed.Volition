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
using Gibbed.RedFaction2.FileFormats.Level.Data;

namespace Gibbed.RedFaction2.FileFormats.Level
{
    internal static class ElementFactory
    {
        private static readonly Dictionary<ElementType, Func<IElement>> _Instantiators;

        static ElementFactory()
        {
            _Instantiators = new Dictionary<ElementType, Func<IElement>>()
            {
                 { ElementType.GeoRegions, () => new RawElement() }, // string array
                 { ElementType.Lights, () => new RawElement() }, // string array
                 { ElementType.CutsceneCameras, () => new CutsceneCameraElement.ArrayElement() },
                 { ElementType.AmbientSounds, () => new AmbientSoundElement.ArrayElement() },
                 { ElementType.Events, () => new EventElement.ArrayElement() },
                 { ElementType.SpawnPoints, () => new MultiplayerSpawnPointElement.ArrayElement() },
                 { ElementType.Unknown800, () => new RawElement() }, // string array
                 { ElementType.RoomEffects, () => new RawElement() }, // string array
                 { ElementType.ClimbingRegions, () => new ClimbingRegionElement.ArrayElement() },
                 { ElementType.BoltEmitters, () => new BoltEmitterElement.ArrayElement() },
                 { ElementType.Targets, () => new TargetElement.ArrayElement() },
                 { ElementType.Decals, () => new DecalElement.ArrayElement() },
                 { ElementType.PushRegions, () => new PushRegionElement.ArrayElement() },
                 { ElementType.Unknown2000, () => new Unknown002000Element.ArrayElement() },
                 { ElementType.Movers, () => new MoverElement.ArrayElement() },
                 { ElementType.Cutscenes, () => new CutsceneElement.ArrayElement() },
                 { ElementType.Unknown6000, () => new Unknown006000Element() },
                 { ElementType.Mirrors, () => new MirrorElement.ArrayElement() },
                 { ElementType.Glares, () => new GlareElement.ArrayElement() },
                 { ElementType.Unknown7680, () => new Unknown007680Element.ArrayElement() },
                 { ElementType.Unknown7681, () => new Unknown007681Element.ArrayElement() },
                 { ElementType.Unknown7682, () => new Unknown007682Element() },
                 { ElementType.Unknown7777, () => new Unknown007777Element.ArrayElement() },
                 { ElementType.Unknown7778, () => new Unknown007778Element() },
                 { ElementType.SplinePaths, () => new SplinePathElement.ArrayElement() },
                 { ElementType.Unknown7900, () => new Unknown007900Element() },
                 { ElementType.Unknown7901, () => new Unknown007901Element() },
                 { ElementType.Unknown10000, () => new Unknown010000Element.ArrayElement() },
                 { ElementType.NavPoints, () => new NavPointArrayElement() },
                 { ElementType.Entities, () => new EntityElement.ArrayElement() },
                 { ElementType.Items, () => new ItemElement.ArrayElement() },
                 { ElementType.Clutters, () => new ClutterElement.ArrayElement() },
                 { ElementType.Triggers, () => new TriggerElement.ArrayElement() },
                 { ElementType.Unknown70000, () => new Unknown070000Element() },
                 { ElementType.Unknown1000000, () => new RawElement() },
                 { ElementType.Unknown1000001, () => new Unknown100001Element() },
                 { ElementType.Brushes, () => new RawElement() },
                 { ElementType.UserDefinedGroups, () => new RawElement() },
            };
        }

        public static IElement Create(ElementType type)
        {
            Func<IElement> instantiator;
            if (_Instantiators.TryGetValue(type, out instantiator) == false)
            {
                throw new NotSupportedException();
            }
            return instantiator();
        }
    }
}
