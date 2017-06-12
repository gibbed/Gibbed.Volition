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
using Gibbed.IO;

namespace Gibbed.RedFaction2.FileFormats.Level
{
    public struct Color
    {
        public static readonly Color White = new Color(0xFF, 0xFF, 0xFF, 0xFF);

        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Color(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public static Color Read(Stream input)
        {
            Color instance;
            instance.R = input.ReadValueU8();
            instance.G = input.ReadValueU8();
            instance.B = input.ReadValueU8();
            instance.A = input.ReadValueU8();
            return instance;
        }

        public static void Write(Stream output, Color instance)
        {
            output.WriteValueU8(instance.R);
            output.WriteValueU8(instance.G);
            output.WriteValueU8(instance.B);
            output.WriteValueU8(instance.A);
        }

        public void Write(Stream output)
        {
            Write(output, this);
        }

        public override string ToString()
        {
            return string.Format("#{3:X2}{0:X2}{1:X2}{2:X2}", this.R, this.G, this.B, this.A);
        }
    }
}
