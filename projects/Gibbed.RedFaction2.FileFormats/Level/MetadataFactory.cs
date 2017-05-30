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
using Gibbed.RedFaction2.FileFormats.Level.Metadata;

namespace Gibbed.RedFaction2.FileFormats.Level
{
    internal static class MetadataFactory
    {
        private static readonly Dictionary<MetadataType, Func<IElement>> _Instantiators;

        static MetadataFactory()
        {
            _Instantiators = new Dictionary<MetadataType, Func<IElement>>()
            {
                { MetadataType.Data, () => new DataElement() },
                { MetadataType.RequiredAnimations, () => new RequiredAnimationArrayElement() },
                { MetadataType.RequiredClothModels, () => new RequiredClothModelArrayElement() },
                { MetadataType.RequiredEffects, () => new RequiredEffectArrayElement() },
                { MetadataType.RequiredModels, () => new RequiredModelArrayElement() },
                { MetadataType.RequiredSpawnTextures, () => new RequiredSpawnTextureArrayElement() },
                { MetadataType.RequiredTextures, () => new RequiredTextureArrayElement() },
                { MetadataType.Settings, () => new SettingsElement() },
                { MetadataType.Unknown1300, () => { throw new NotImplementedException(); } },
            };
        }

        public static IElement Create(MetadataType type)
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
