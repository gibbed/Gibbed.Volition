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

using System.Globalization;
using System.IO;
using Gibbed.IO;

namespace Gibbed.Volition.FileFormats.Interface
{
    public class ColorProperty : IProperty
    {
        public float R;
        public float G;
        public float B;

        public string Tag
        {
            get { return "color"; }
        }

        public override string ToString()
        {
            return this.R.ToString(CultureInfo.InvariantCulture) + "," +
                   this.G.ToString(CultureInfo.InvariantCulture) + "," +
                   this.B.ToString(CultureInfo.InvariantCulture);
        }

        public void Serialize(Stream output, Endian endian, StringTable strings)
        {
            output.WriteValueF32(this.R, endian);
            output.WriteValueF32(this.G, endian);
            output.WriteValueF32(this.B, endian);
        }

        public void Deserialize(Stream input, Endian endian, StringTable strings)
        {
            this.R = input.ReadValueF32(endian);
            this.G = input.ReadValueF32(endian);
            this.B = input.ReadValueF32(endian);
        }
    }
}
