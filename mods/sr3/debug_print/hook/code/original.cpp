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
#include "original.hpp"

extern "C"
{
	APIWRAPPER p_DllMain;
	APIWRAPPER p_XInputGetState;
	APIWRAPPER p_XInputSetState;
	APIWRAPPER p_XInputGetCapabilities;
	APIWRAPPER p_XInputEnable;
	APIWRAPPER p_XInputGetDSoundAudioDeviceGuids;
	APIWRAPPER p_XInputGetBatteryInformation;
	APIWRAPPER p_XInputGetKeystroke;
	APIWRAPPER p_XInputGetStateEx;
	APIWRAPPER p_XInputWaitForGuideButton;
	APIWRAPPER p_XInputCancelGuideButtonWait;
	APIWRAPPER p_XInputPowerOffController;
}

void __declspec(naked) h_DllMain() { _asm{ jmp p_DllMain } }
void __declspec(naked) h_XInputGetState() { _asm{ jmp p_XInputGetState } }
void __declspec(naked) h_XInputSetState() { _asm{ jmp p_XInputSetState } }
void __declspec(naked) h_XInputGetCapabilities() { _asm{ jmp p_XInputGetCapabilities } }
void __declspec(naked) h_XInputEnable() { _asm{ jmp p_XInputEnable } }
void __declspec(naked) h_XInputGetDSoundAudioDeviceGuids() { _asm{ jmp p_XInputGetDSoundAudioDeviceGuids } }
void __declspec(naked) h_XInputGetBatteryInformation() { _asm{ jmp p_XInputGetBatteryInformation } }
void __declspec(naked) h_XInputGetKeystroke() { _asm{ jmp p_XInputGetKeystroke } }
void __declspec(naked) h_XInputGetStateEx() { _asm{ jmp p_XInputGetStateEx } }
void __declspec(naked) h_XInputWaitForGuideButton() { _asm{ jmp p_XInputWaitForGuideButton } }
void __declspec(naked) h_XInputCancelGuideButtonWait() { _asm{ jmp p_XInputCancelGuideButtonWait } }
void __declspec(naked) h_XInputPowerOffController() { _asm{ jmp p_XInputPowerOffController } }

void LoadStubs(HINSTANCE dll)
{
	p_DllMain = (APIWRAPPER)GetProcAddress(dll, MAKEINTRESOURCEA(1));
	p_XInputGetState = (APIWRAPPER)GetProcAddress(dll, MAKEINTRESOURCEA(2));
	p_XInputSetState = (APIWRAPPER)GetProcAddress(dll, MAKEINTRESOURCEA(3));
	p_XInputGetCapabilities = (APIWRAPPER)GetProcAddress(dll, MAKEINTRESOURCEA(4));
	p_XInputEnable = (APIWRAPPER)GetProcAddress(dll, MAKEINTRESOURCEA(5));
	p_XInputGetDSoundAudioDeviceGuids = (APIWRAPPER)GetProcAddress(dll, MAKEINTRESOURCEA(6));
	p_XInputGetBatteryInformation = (APIWRAPPER)GetProcAddress(dll, MAKEINTRESOURCEA(7));
	p_XInputGetKeystroke = (APIWRAPPER)GetProcAddress(dll, MAKEINTRESOURCEA(8));
	p_XInputGetStateEx = (APIWRAPPER)GetProcAddress(dll, MAKEINTRESOURCEA(100));
	p_XInputWaitForGuideButton = (APIWRAPPER)GetProcAddress(dll, MAKEINTRESOURCEA(101));
	p_XInputCancelGuideButtonWait = (APIWRAPPER)GetProcAddress(dll, MAKEINTRESOURCEA(102));
	p_XInputPowerOffController = (APIWRAPPER)GetProcAddress(dll, MAKEINTRESOURCEA(103));
}
