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

namespace Gibbed.Volition.FileFormats.Peg
{
    public enum PixelFormat : ushort
    {
        // Supported by: SR2, RFG, RFA
        //    DX9: D3DFMT_DXT1
        //   DX10: DXGI_FORMAT_BC1_TYPELESS
        DXT1 = 400,
        
        // Supported by: SR2, RFG, RFA
        //    DX9: D3DFMT_DXT3 (DX9)
        //   DX10: DXGI_FORMAT_BC2_TYPELESS
        DXT3 = 401,
        
        // Supported by: SR2, RFG, RFA
        //    DX9: D3DFMT_DXT5
        //   DX10: DXGI_FORMAT_BC3_TYPELESS
        DXT5 = 402,

        // Supported by: SR2, RFG, RFA
        //    DX9: D3DFMT_R5G6B5
        //   DX10: DXGI_FORMAT_B5G6R5_UNORM
        R5G6B5 = 403,

        // Supported by: SR2, RFA (DX9)
        //    DX9: D3DFMT_A1R5G5B5
        A1R5G5B5 = 404,

        // Supported by: SR2, RFA (DX9)
        //    DX9: D3DFMT_A4R4G4B4
        A4R4G4B4 = 405,

        // Supported by: SR2, RFA (DX9)
        //    DX9: D3DFMT_R8G8B8
        R8G8B8 = 406,

        // Supported by: SR2, RFG, RFA
        //    DX9: D3DFMT_A8R8G8B8
        //   DX10: DXGI_FORMAT_B8G8R8A8_TYPELESS
        A8R8G8B8 = 407,

        // Supported by: SR2, RFG, RFA (DX9)
        //    DX9: D3DFMT_V8U8
        V8U8 = 408,

        // Supported by: SR2, RFA (DX9)
        //    DX9: D3DFMT_CxV8U8
        CxV8U8 = 409,

        // Supported by: SR2
        //    DX9: D3DFMT_A8
        A8 = 410,

        // XBox2 format?
        Unknown_603 = 603,
    }
}
