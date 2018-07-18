using System;
using System.Collections.Generic;
using System.IO;

using Pipliz;

namespace AdvancedWand.Helper
{
    [ModLoader.ModManager]
    public static class BlueprintManager
    {
        public static Dictionary<string, Blueprint> _blueprints = new Dictionary<string, Blueprint>();
        public static string MODPATH = "";


        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAssemblyLoaded, "Khanx.AdvancedWand.GetModPath")]
        public static void GetModPath(string path)
        {
            MODPATH = Path.GetDirectoryName(path).Replace("\\", "/");
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterWorldLoad, "Khanx.AdvancedWand.LoadBlueprints")]
        public static void LoadBlueprints()
        {
            if(Directory.Exists(MODPATH + "/blueprints/"))
            {
                string[] prefixFiles = Directory.GetFiles(MODPATH + "/blueprints/", "*.b", SearchOption.AllDirectories);

                Log.Write(string.Format("<color=blue>Loading blueprints: {0}</color>", prefixFiles.Length));

                foreach(string filename in prefixFiles)
                {
                    string blueprint_name = filename.Substring(filename.LastIndexOf("/")+1);
                    blueprint_name = blueprint_name.Substring(0, blueprint_name.Length - 2);

                    Blueprint b = new Blueprint(File.ReadAllBytes(filename));

                    _blueprints.Add(blueprint_name, new Blueprint(File.ReadAllBytes(filename)));

                    Log.Write(string.Format("<color=blue>Loaded blueprint: {0}</color>", blueprint_name));
                }
            }
            else
            {
                Directory.CreateDirectory(MODPATH + "/blueprints/");
            }
        }
    }
}
