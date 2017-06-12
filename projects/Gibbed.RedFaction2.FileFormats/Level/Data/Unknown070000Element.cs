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
using System.IO;
using Gibbed.IO;
using Newtonsoft.Json;

namespace Gibbed.RedFaction2.FileFormats.Level.Data
{
    public class Unknown070000Element : IElement
    {
        private Vector3 _Unknown0;
        private Transform _Unknown1;

        public Vector3 Unknown0
        {
            get { return this._Unknown0; }
            set { this._Unknown0 = value; }
        }

        public Transform Unknown1
        {
            get { return this._Unknown1; }
            set { this._Unknown1 = value; }
        }

        public void Read(Stream input, uint version, Endian endian)
        {
            this._Unknown0 = Vector3.Read(input, endian);
            this._Unknown1 = Transform.Read(input, endian);
        }

        public void Write(Stream output, uint version, Endian endian)
        {
            Vector3.Write(output, this._Unknown0, endian);
            Transform.Write(output, this._Unknown1, endian);
        }

        public void ImportJson(JsonReader reader)
        {
            throw new NotImplementedException();
        }

        public void ExportJson(JsonWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
