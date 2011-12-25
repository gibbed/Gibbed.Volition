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

#include "original.hpp"
#include "game.hpp"

HINSTANCE RealDLL;

void LoadStubs(HINSTANCE dll);
BOOL APIENTRY DllMain(
	HINSTANCE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
		case DLL_PROCESS_ATTACH:
		{
			wchar_t path[MAX_PATH];
			GetSystemDirectoryW(path, MAX_PATH);
			PathAppend(path, L"XINPUT1_3.dll");

			RealDLL = LoadLibraryW(path);
			if (RealDLL == NULL)
			{
				MessageBoxW(0, L"Failed to load real XINPUT1_3.dll!", L"Error", MB_OK | MB_ICONERROR);
				ExitProcess(0);
				return FALSE;
			}
			LoadStubs(RealDLL);
			
			if (GameAttach() == false)
			{
				return FALSE;
			}

			break;
		}

		case DLL_PROCESS_DETACH:
		{
			GameDetach();
			FreeLibrary(RealDLL);
			break;
		}
	}

	return TRUE;
}
