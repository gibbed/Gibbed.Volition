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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Gibbed.Volition.FileFormats;

namespace Gibbed.Volition.Test
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var input = File.OpenRead(@"T:\SR2\pc\meshes\sp_atv01.cvtf"))
            {
                var cvtf = new CVTFFile();
                cvtf.Deserialize(input, Gibbed.IO.Endian.Little);

            }
        }
    }
}
