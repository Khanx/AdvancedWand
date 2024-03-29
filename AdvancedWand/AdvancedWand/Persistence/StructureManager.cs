﻿using Pipliz;
using System.Collections.Generic;
using System.IO;

namespace AdvancedWand.Persistence
{
    [ModLoader.ModManager]
    public static class StructureManager
    {
        public static string MOD_FOLDER = @"";

        public static string Blueprint_FOLDER = "";
        public static string Schematic_FOLDER = "";

        public static Dictionary<string, string> _structures = new Dictionary<string, string>();

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAssemblyLoaded, "Khanx.AdvancedWand.GetModPath")]
        public static void GetModPath(string path)
        {
            MOD_FOLDER = Path.GetDirectoryName(path).Replace("\\", "/");

            Blueprint_FOLDER = MOD_FOLDER + "/Blueprints/";
            Schematic_FOLDER = MOD_FOLDER + "/Schematics/";
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterWorldLoad, "Khanx.AdvancedWand.LoadStructures")]
        public static void LoadStructures()
        {
            _structures.Clear();

            if (Directory.Exists(Blueprint_FOLDER))
            {
                string[] prefixFiles = Directory.GetFiles(Blueprint_FOLDER, "*.b", SearchOption.AllDirectories);
                Log.Write(string.Format("<color=blue>Loading blueprints: {0}</color>", prefixFiles.Length));

                foreach (string file in prefixFiles)
                {
                    string blueprint_name = file.Substring(file.LastIndexOf("/") + 1).Trim().ToLower();
                    blueprint_name = blueprint_name.Substring(0, blueprint_name.Length - 2);

                    _structures.Add(blueprint_name, file);
                    Log.Write(string.Format("<color=blue>Loaded blueprint: {0}</color>", blueprint_name));
                }

            }
            else
                Directory.CreateDirectory(Blueprint_FOLDER);

            if (Directory.Exists(Schematic_FOLDER))
            {
                string[] prefixFiles = Directory.GetFiles(Schematic_FOLDER, "*.csschematic", SearchOption.AllDirectories);
                Log.Write(string.Format("<color=blue>Loading schematics: {0}</color>", prefixFiles.Length));

                foreach (string file in prefixFiles)
                {
                    string schematic_name = file.Substring(file.LastIndexOf("/") + 1).Trim().ToLower();
                    schematic_name = schematic_name.Substring(0, schematic_name.Length - 12);

                    if (_structures.ContainsKey(schematic_name))
                    {
                        Log.Write(string.Format("<color=red>The {0} schematic has not been added since a blueprint with the same name already exists.</color>", schematic_name));
                        continue;
                    }
                    else
                        _structures.Add(schematic_name, file);

                    Log.Write(string.Format("<color=blue>Loaded blueprint: {0}</color>", schematic_name));
                }
            }
            else
                Directory.CreateDirectory(Schematic_FOLDER);
        }

        public static Structure GetStructure(string name)
        {
            if (!_structures.TryGetValue(name, out string file))
                file = "";

            if (file.Equals(""))
                return null;

            if (file.Contains(".csschematic"))
                return new Schematic(file);

            return new Blueprint(file);

        }

        public static bool SaveStructure(Structure structure, string name)
        {
            if (_structures.ContainsKey(name))
                return false;

            structure.Save(name);
            LoadStructures();

            return true;
        }
    }
}
