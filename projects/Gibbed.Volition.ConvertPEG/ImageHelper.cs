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
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Peg = Gibbed.Volition.FileFormats.Peg;

namespace Gibbed.Volition.ConvertPEG
{
    internal static class ImageHelper
    {
        public static Bitmap ExportA8R8G8B8(
            int width, int height,
            byte[] buffer)
        {
            var bitmap = new Bitmap(
                width, height,
                PixelFormat.Format32bppArgb);

            /*
            for (uint i = 0; i < width * height * 4; i += 4)
            {
                byte r = buffer[i + 0];
                buffer[i + 0] = buffer[i + 2];
                buffer[i + 2] = r;
            }
            */

            var area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var data = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(buffer, 0, data.Scan0, (int)(width * height * 4));
            bitmap.UnlockBits(data);
            return bitmap;
        }

        public static Bitmap ExportDXT(
            Peg.PixelFormat format,
            int width, int height,
            byte[] buffer)
        {
            var flags = Squish.Native.Flags.None;
            
            switch (format)
            {
                case Peg.PixelFormat.DXT1:
                {
                    flags |= Squish.Native.Flags.DXT1;
                    break;
                }

                case Peg.PixelFormat.DXT3:
                {
                    flags |= Squish.Native.Flags.DXT3;
                    break;
                }

                case Peg.PixelFormat.DXT5:
                {
                    flags |= Squish.Native.Flags.DXT5;
                    break;
                }

                default:
                {
                    throw new NotSupportedException();
                }
            }

            buffer = Squish.Native.DecompressImage(
                buffer, width, height, flags);

            var bitmap = new Bitmap(
                width, height,
                PixelFormat.Format32bppArgb);

            for (uint i = 0; i < width * height * 4; i += 4)
            {
                byte r = buffer[i + 0];
                buffer[i + 0] = buffer[i + 2];
                buffer[i + 2] = r;
            }

            var area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var data = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(buffer, 0, data.Scan0, (int)(width * height * 4));
            bitmap.UnlockBits(data);
            return bitmap;
        }
    }
}
