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
using System.Collections.Generic;
using System.IO;
using Gibbed.IO;
using Gibbed.PortableExecutable;
using NDesk.Options;

namespace Searcher
{
    internal class Program
    {
        private static string GetExecutablePath()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private static ByteSearch LoadPattern(string name)
        {
            using (var input = File.OpenRead(Path.Combine(GetExecutablePath(), "Patterns", name)))
            {
                var reader = new StreamReader(input);
                return new ByteSearch(reader.ReadToEnd());
            }
        }

        public static void Main(string[] args)
        {
            var showHelp = false;

            var options = new OptionSet()
            {
                {
                    "h|help",
                    "show this message and exit", 
                    v => showHelp = v != null
                },
            };

            List<string> extras;

            try
            {
                extras = options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("{0}: ", GetExecutableName());
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `{0} --help' for more information.", GetExecutableName());
                return;
            }

            if (extras.Count != 1 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_exe", GetExecutableName());
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            string inputPath = extras[0];

            var changelistByteSearch = LoadPattern("changelist.txt");
            var registerLuaFunctionsByteSearch = LoadPattern("register_lua_functions.txt");
            var luaIncludeByteSearch = LoadPattern("lua_include.txt");

            using (var input = File.OpenRead(inputPath))
            {
                var exe = new Executable32();
                exe.Deserialize(input);

                var section = exe.GetSection(".text");
                input.Seek(section.PointerToRawData, SeekOrigin.Begin);
                var code = input.ReadBytes(section.SizeOfRawData);

                var changelistOffset = changelistByteSearch.Match(code);
                if (changelistOffset == uint.MaxValue)
                {
                    Console.WriteLine("Failed to find changelist offset.");
                }
                else
                {
                    Console.WriteLine("   changelist = {0}", BitConverter.ToUInt32(code, (int)changelistOffset + 0x22));
                    
                    var changelistAddress = changelistOffset + 0x22;
                    changelistAddress += exe.NTHeaders32.OptionalHeader.ImageBase;
                    changelistAddress += section.VirtualAddress;

                    Console.WriteLine("   changelist @ 0x{0:X8}", changelistAddress);
                }

                var registerLuaFunctionsOffset = registerLuaFunctionsByteSearch.Match(code);
                if (registerLuaFunctionsOffset == uint.MaxValue)
                {
                    Console.WriteLine("Failed to find register lua functions offset.");
                }
                else
                {
                    var debugPrintOffsetAddress = registerLuaFunctionsOffset + 0x6C;
                    debugPrintOffsetAddress += exe.NTHeaders32.OptionalHeader.ImageBase;
                    debugPrintOffsetAddress += section.VirtualAddress;

                    var assertMsgOffsetAddress = registerLuaFunctionsOffset + 0x7C;
                    assertMsgOffsetAddress += exe.NTHeaders32.OptionalHeader.ImageBase;
                    assertMsgOffsetAddress += section.VirtualAddress;

                    Console.WriteLine("  debug_print @ 0x{0:X8}", debugPrintOffsetAddress);
                    Console.WriteLine("   assert_msg @ 0x{0:X8}", assertMsgOffsetAddress);

                    var includeAddress = BitConverter.ToUInt32(code, (int)registerLuaFunctionsOffset + 0xAC);
                    var includeOffset = exe.GetFileOffset(includeAddress, false);
                    if (includeOffset == 0)
                    {
                        Console.WriteLine("Failed to find include.");
                    }
                    else
                    {
                        input.Seek(includeOffset, SeekOrigin.Begin);
                        var includeCode = input.ReadBytes(luaIncludeByteSearch.Size);

                        if (luaIncludeByteSearch.Match(includeCode) == uint.MaxValue)
                        {
                            Console.WriteLine("Code mismatch for include.");
                        }
                        else
                        {
                            var gettopOffset = BitConverter.ToUInt32(includeCode, 0x07);
                            var tolstringOffset = BitConverter.ToUInt32(includeCode, 0x12);

                            var gettopAddress = (includeAddress + 0x0B) + gettopOffset;
                            var tolstringAddress = (includeAddress + 0x16) + tolstringOffset;

                            Console.WriteLine("   lua_gettop @ 0x{0:X8}", gettopAddress);
                            Console.WriteLine("lua_tolstring @ 0x{0:X8}", tolstringAddress);
                        }
                    }
                }
            }
        }
    }
}
