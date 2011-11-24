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
using System.Linq;  
using Gibbed.SaintsRow3.FileFormats;
using Gibbed.Volition.FileFormats;
using NDesk.Options;

namespace Gibbed.SaintsRow3.UpdateASM
{
    internal class Program
    {
        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }

        public static void Main(string[] args)
        {
            var verbose = false;
            var showHelp = false;
            bool overwriteFiles = false;

            var options = new OptionSet()
            {
                {
                    "v|verbose",
                    "be verbose", 
                    v => verbose = v != null
                },
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

            if (extras.Count < 2 || extras.Count > 3 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ [output_file] asm_file containers_dir", GetExecutableName());
                Console.WriteLine("Update ASM file.");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            var inputPath = extras.Count >= 3 ? extras[1] : extras[0];
            var outputPath = extras.Count >= 3 ? extras[0] : inputPath;
            var filePath = extras.Count >= 3 ? extras[2] : extras[1];

            var asm = new AsmFile();

            using (var input = File.OpenRead(inputPath))
            {
                asm.Deserialize(input);
            }

            foreach (var container in asm.Containers)
            {
                var containerName = Path.ChangeExtension(container.Name, ".str2_pc");
                var containerPath = Path.Combine(filePath, containerName);

                if (File.Exists(containerPath) == false)
                {
                    if (verbose == true)
                    {
                        Console.WriteLine("Warning: Container '{0}' not found in directory.",
                            container.Name);
                    }

                    continue;
                }

                //if (verbose == true)
                {
                    Console.WriteLine("=> '{0}'", container.Name);
                }

                if (container.Sizes.Count !=
                    container.Primitives.Count)
                {
                    Console.WriteLine("Error: count mismatch in container?");
                    return;
                }

                var pkg = new PackageFileV6();
                using (var input = File.OpenRead(containerPath))
                {
                    pkg.Deserialize(input);

                    container.DataOffset = (uint)pkg.DataOffset;
                    container.CompressedSize = pkg.CompressedSize;

                    var fileNames = pkg.Entries.Select(e => e.Name.ToLowerInvariant()).ToList();
                    var primitiveNames = container.Primitives.Select(p => p.Name.ToLowerInvariant()).ToList();

                    for (int i = 0; i < container.Primitives.Count; i++)
                    {
                        var primitive = container.Primitives[i];

                        //if (verbose == true)
                        {
                            Console.WriteLine("==> '{0}'", primitive.Name);
                        }

                        string headerName;
                        string dataName;

                        switch (primitive.Type)
                        {
                            // character meshes
                            case 5:
                            {
                                var extension = Path.GetExtension(primitive.Name);

                                if (extension == ".ccmesh_pc")
                                {
                                    headerName = primitive.Name;
                                    dataName = Path.ChangeExtension(headerName, ".gcmesh_pc");
                                }
                                else
                                {
                                    Console.WriteLine("Error: unexpected character mesh extension '{0}'",
                                        extension);
                                    return;
                                }

                                break;
                            }

                            // pegs
                            case 16:
                            {
                                var extension = Path.GetExtension(primitive.Name);

                                if (extension == ".cvbm_pc")
                                {
                                    headerName = primitive.Name;
                                    dataName = Path.ChangeExtension(headerName, ".gvbm_pc");
                                }
                                else if (extension == ".cpeg_pc")
                                {
                                    headerName = primitive.Name;
                                    dataName = Path.ChangeExtension(headerName, ".gpeg_pc");
                                }
                                else
                                {
                                    Console.WriteLine("Error: unexpected PEG extension '{0}'",
                                        extension);
                                    return;
                                }

                                break;
                            }

                            // static mesh
                            case 19:
                            {
                                var extension = Path.GetExtension(primitive.Name);

                                if (extension == ".csmesh_pc")
                                {
                                    headerName = primitive.Name;
                                    dataName = Path.ChangeExtension(headerName, ".gsmesh_pc");
                                }
                                else
                                {
                                    Console.WriteLine("Error: unexpected static mesh extension '{0}'",
                                        extension);
                                    return;
                                }

                                break;
                            }

                            case 7: // .cmorph_pc
                            case 15: // .cefc_pc
                            case 20: // .rig_pc
                            case 22: // .matlib_pc
                            case 26: // .vint_doc
                            case 27: // .lua
                            {
                                headerName = primitive.Name;
                                dataName = null;
                                break;
                            }

                            default:
                            {
                                Console.WriteLine("Error: unsupported primitive type {0} ('{1}')",
                                    primitive.Type,
                                    asm.PrimitiveTypes.SingleOrDefault(p => p.Id == primitive.Type).Name);
                                return;
                            }
                        }

                        if (headerName != null)
                        {
                            headerName = headerName.ToLowerInvariant();

                            var entry = pkg.Entries.SingleOrDefault(
                                e => e.Name.ToLowerInvariant() == headerName);
                            if (entry == null)
                            {
                                Console.WriteLine("Error: container '{0}' is missing primitive header '{1}'",
                                    container.Name,
                                    headerName);
                                return;
                            }

                            container.Sizes[i].HeaderSize = (int)entry.UncompressedSize;
                            primitive.HeaderSize = (int)entry.UncompressedSize;

                            primitiveNames.Remove(headerName);
                            fileNames.Remove(headerName);
                        }

                        if (dataName != null)
                        {
                            dataName = dataName.ToLowerInvariant();

                            var entry = pkg.Entries.SingleOrDefault(
                                e => e.Name.ToLowerInvariant() == dataName);
                            if (entry == null)
                            {
                                Console.WriteLine("Error: container '{0}' is missing primitive data '{1}'",
                                    container.Name,
                                    dataName);
                                return;
                            }

                            container.Sizes[i].DataSize = (int)entry.UncompressedSize;
                            primitive.DataSize = (int)entry.UncompressedSize;

                            primitiveNames.Remove(dataName);
                            fileNames.Remove(dataName);
                        }
                    }

                    if (fileNames.Count > 0)
                    {
                        Console.WriteLine("Error: container '{0}' is missing primitives for",
                            container.Name);
                        foreach (var name in fileNames)
                        {
                            Console.WriteLine("  '{0}'", name);
                        }
                        return;
                    }

                    if (primitiveNames.Count > 0)
                    {
                        Console.WriteLine("Error: package '{0}' is missing files for",
                            container.Name);
                        foreach (var name in primitiveNames)
                        {
                            Console.WriteLine("  '{0}'", name);
                        }
                        return;
                    }
                }
            }

            using (var output = File.Create(outputPath))
            {
                asm.Serialize(output);
            }
        }
    }
}
