using Pipliz;
using Pipliz.JSON;
using System.IO;

namespace AdvancedWand.Helper
{
    [ModLoader.ModManager]
    public static class GameLoader
    {
        public static string MOD_FOLDER = @"";

        public static string Schematic_FOLDER = "";

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAssemblyLoaded, "Khanx.AdvancedWand.GetModPath")]
        public static void GetModPath(string path)
        {
            MOD_FOLDER = Path.GetDirectoryName(path).Replace("\\", "/");

            Schematic_FOLDER = MOD_FOLDER + "/Schematics/";

            if (!Directory.Exists(MOD_FOLDER))
                Directory.CreateDirectory(MOD_FOLDER);
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterWorldLoad, "Khanx.AdvancedWand.LoadSettings")]
        public static void LoadSettigns()
        {

            if (!File.Exists(MOD_FOLDER + "/settings.json"))
            {
                Log.Write("<color=red>AdvancedWand: Settings not found</color>");
                return;
            }

            JSONNode json = JSON.Deserialize(MOD_FOLDER + "/settings.json");

            AdvancedWand.default_limit = json.GetAsOrDefault<int>("default_limit", 100000);
            int increment = json.GetAsOrDefault<int>("speed", 250);

            if (increment <= 100)
                increment = 100;
            else if (increment >= 1000)
                increment = 1000;

            AdvancedWand.increment = (1010 - increment);

            AdvancedWand.security = json.GetAsOrDefault<bool>("security", true);
        }

    }
}
