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
                 { ElementType.Unknown200, () => new RawElement() },
                 { ElementType.Unknown300, () => new RawElement() },
                 { ElementType.Unknown400, () => new RawElement() },
                 { ElementType.Unknown500, () => new RawElement() },
                 { ElementType.Unknown600, () => new RawElement() },
                 { ElementType.Unknown700, () => new RawElement() },
                 { ElementType.UnknownC00, () => new RawElement() },
                 { ElementType.UnknownD00, () => new RawElement() },
                 { ElementType.UnknownE00, () => new RawElement() },
                 { ElementType.Unknown1000, () => new RawElement() },
                 { ElementType.Unknown1100, () => new RawElement() },
                 { ElementType.Unknown2000, () => new RawElement() },
                 { ElementType.Unknown3000, () => new RawElement() },
                 { ElementType.Unknown4000, () => new RawElement() },
                 { ElementType.Unknown7677, () => new RawElement() },
                 { ElementType.Unknown7678, () => new RawElement() },
                 { ElementType.Unknown7680, () => new RawElement() },
                 { ElementType.Unknown7681, () => new RawElement() },
                 { ElementType.Unknown7682, () => new RawElement() },
                 { ElementType.Unknown7777, () => new RawElement() },
                 { ElementType.Unknown7778, () => new RawElement() },
                 { ElementType.Unknown7779, () => new RawElement() },
                 { ElementType.Unknown7900, () => new RawElement() },
                 { ElementType.Unknown7901, () => new RawElement() },
                 { ElementType.Unknown10000, () => new RawElement() },
                 { ElementType.Unknown20000, () => new RawElement() },
                 { ElementType.Unknown30000, () => new RawElement() },
                 { ElementType.Unknown40000, () => new RawElement() },
                 { ElementType.Unknown50000, () => new RawElement() },
                 { ElementType.Unknown60000, () => new RawElement() },
                 { ElementType.Unknown70000, () => new RawElement() },
                 { ElementType.Unknown1000000, () => new RawElement() },
                 { ElementType.Unknown1000001, () => new RawElement() },
                 { ElementType.Unknown2000000, () => new RawElement() },
                 { ElementType.Unknown3000000, () => new RawElement() },
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
