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

#include "patch.hpp"

bool PatchData(unsigned int address, void *data, int length)
{
	DWORD old, junk;
	if (VirtualProtect((void *)address, length, PAGE_EXECUTE_READWRITE, &old) == FALSE)
	{
		return false;
	}
	memcpy((void *)address, data, length);
	VirtualProtect((void *)address, length, old, &junk);
	return true;
}

bool PatchCode(unsigned int address, void *data, int length)
{
	if (PatchData(address, data, length) == false)
	{
		return false;
	}

	if (FlushInstructionCache(GetCurrentProcess(), (void *)address, length) == FALSE)
	{
		return false;
	}

	return true;
}

bool PatchJumpInternal(unsigned int address, unsigned char opcode, unsigned int target)
{
	unsigned char jump[5];

	jump[0] = opcode;
	*(DWORD *)(&jump[1]) = target;

	return PatchCode(address, jump, 5);
}

bool PatchCall(unsigned int address, unsigned int target)
{
	return PatchJumpInternal(address, 0xE8, target - address - 5);
}

bool PatchJump(unsigned int address, unsigned int target)
{
	return PatchJumpInternal(address, 0xE9, target - address - 5);
}
