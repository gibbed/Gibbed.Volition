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

#include <windows.h>
#include <shlwapi.h>
#include <stdio.h>
#include <string.h>

#include "game.hpp"
#include "patch.hpp"

#define UINT32(x) (*(unsigned int *)x)

typedef int (__cdecl *LUA_GETTOP)(void *);
typedef const char * (__cdecl *LUA_TOLSTRING)(void *, int, size_t *);

LUA_GETTOP lua_gettop = NULL;
LUA_TOLSTRING lua_tolstring = NULL;

int __cdecl DebugPrint(void *L)
{
	const char *msg;
	
	msg = lua_tolstring(L, 1, NULL);
	if (_strcmpi(msg, "vint") == 0)
	{
		msg = lua_tolstring(L, 2, NULL);
	}

	printf("%s", msg);
	return 0;
}

bool HookGame(void)
{
	unsigned int debugPrintAddress = (unsigned int)&DebugPrint;

	// 949013 as DX9
	if (UINT32(0x00B7E2F2) == 949013)
	{
		PatchCode(0x00DF02FC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DDE6D0;
		lua_tolstring = (LUA_TOLSTRING)0x00DDEA90;
		return true;
	}

	// 949013 as DX11
	else if (UINT32(0x00B7C8E2) == 949013)
	{
		PatchCode(0x00DF02FC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DDE6D0;
		lua_tolstring = (LUA_TOLSTRING)0x00DDEA90;
		return true;
	}

	// 951269 as DX9
	else if (UINT32(0x00B7DD42) == 951269)
	{
		PatchCode(0x00DF02FC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DDE7E0;
		lua_tolstring = (LUA_TOLSTRING)0x00DDEBA0;
		return true;
	}

	// 951269 as DX11
	else if (UINT32(0x00B7C9F2) == 951269)
	{
		PatchCode(0x00E5E26C, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DE21F0;
		lua_tolstring = (LUA_TOLSTRING)0x00DE25B0;
		return true;
	}

	// 953575 as DX9
	else if (UINT32(0x00B7E2D2) == 953575)
	{
		PatchCode(0x00DF03AC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DDE890;
		lua_tolstring = (LUA_TOLSTRING)0x00DDEC50;
		return true;
	}

	// 953575 as DX11
	else if (UINT32(0x00B7C502) == 953575)
	{
		PatchCode(0x00E5DF1C, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DE21F0;
		lua_tolstring = (LUA_TOLSTRING)0x00DE25B0;
		return true;
	}

	// 960264 as DX9
	else if (UINT32(0x00B94A92) == 960264)
	{
		PatchCode(0x00E085FC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DF6AD0;
		lua_tolstring = (LUA_TOLSTRING)0x00DF6E90;
		return true;
	}

	// 960264 as DX11
	else if (UINT32(0x00B93C12) == 960264)
	{
		PatchCode(0x00E76BCC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DFAEE0;
		lua_tolstring = (LUA_TOLSTRING)0x00DFB2A0;
		return true;
	}

	// 960956 wasn't an EXE patch

	// 964345 as DX9
	else if (UINT32(0x00B9A432) == 877048)
	{
		PatchCode(0x00E0F2BC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DFD7A0;
		lua_tolstring = (LUA_TOLSTRING)0x00DFDB60;
		return true;
	}

	// 964345 as DX11
	else if (UINT32(0x00B99532) == 877048)
	{
		PatchCode(0x00E7D8EC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00E01D80;
		lua_tolstring = (LUA_TOLSTRING)0x00E02140;
		return true;
	}

	// 965967 as DX9
	else if (UINT32(0x00B9A242) == 965967)
	{
		PatchCode(0x00E0F2DC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DFD7C0;
		lua_tolstring = (LUA_TOLSTRING)0x00DFDB80;
		return true;
	}

	// 965967 as DX11
	else if (UINT32(0x00B997C2) == 965967)
	{
		PatchCode(0x00E7DAAC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00E01E50;
		lua_tolstring = (LUA_TOLSTRING)0x00E02210;
		return true;
	}

	// 966663 wasn't an EXE patch

	// 966711 as DX9
	else if (UINT32(0x00B9A232) == 966711)
	{
		PatchCode(0x00E0F2DC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DFD7C0;
		lua_tolstring = (LUA_TOLSTRING)0x00DFDB80;
		return true;
	}

	// 966711 as DX11
	else if (UINT32(0x00B99782) == 966711)
	{
		PatchCode(0x00E7DAAC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00E01E50;
		lua_tolstring = (LUA_TOLSTRING)0x00E02210;
		return true;
	}

	// 968252 as DX9
	else if (UINT32(0x00B9A2C2) == 968252)
	{
		PatchCode(0x00E0F60C, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DFDAE0;
		lua_tolstring = (LUA_TOLSTRING)0x00DFDEA0;
		return true;
	}

	// 968252 as DX11
	else if (UINT32(0x00B998C2) == 968252)
	{
		PatchCode(0x00E7DCFC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00E02350;
		lua_tolstring = (LUA_TOLSTRING)0x00E02710;
		return true;
	}

	// 970861 as DX9
	else if (UINT32(0x00B9A432) == 877048)
	{
		PatchCode(0x00E0F2BC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00DFD7A0;
		lua_tolstring = (LUA_TOLSTRING)0x00DFDB60;
		return true;
	}

	// 970861 as DX11
	else if (UINT32(0x00B99532) == 877048)
	{
		PatchCode(0x00E7D8EC, &debugPrintAddress, 4);
		lua_gettop = (LUA_GETTOP)0x00E01D80;
		lua_tolstring = (LUA_TOLSTRING)0x00E02140;
		return true;
	}

	return false;
}

bool GameAttach(void)
{
	/*
	if (IdentifyGameVersion() == false)
	{
		MessageBoxW(0, L"Failed to identify game version.", L"gibbed's xinput hook for SR:TT", MB_OK | MB_ICONERROR);
		return false;
	}
	*/

	if (HookGame() == false)
	{
		//MessageBoxW(0, L"Failed to hook game.", L"gibbed's xinput hook for SR:TT", MB_OK | MB_ICONERROR);
		MessageBoxW(0, L"Failed to identify game version.", L"gibbed's xinput hook for SR:TT", MB_OK | MB_ICONERROR);
		return false;
	}

	AllocConsole();
	FILE *dummy;
	freopen_s(&dummy, "CONIN$", "rb", stdin);
	freopen_s(&dummy, "CONOUT$", "wb", stdout);
	freopen_s(&dummy, "CONOUT$", "wb", stderr);
	return true;
}

void GameDetach(void)
{
}
