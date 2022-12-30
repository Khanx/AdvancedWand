using System.Collections.Generic;
using System.IO;
using AdvancedWand.Helper;
using AdvancedWand.Persistence;
using Chatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pipliz;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class TreeCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if (!chat.Trim().ToLower().StartsWith("//tree"))
                return false;

            if (!CommandHelper.CheckCommand(player))
                return true;

            if (0 >= splits.Count)
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if (!CommandHelper.CheckLimit(player))
                return true;

            if (!chat.Contains(" "))
            {
                Chat.Send(player, "<color=orange>Wrong Arguments: //tree _treename_</color>");
                return true;
            }

            string filename = chat.Substring(chat.IndexOf(" ") + 1).Trim();

            filename = filename.Replace(" ", "_");

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Dictionary<string, List<Vector3Int>> tree = new();


            if (wand.area == null)
            {
                Chat.Send(player, "<color=orange>Area not detected</color>");
                return true;
            }

            if (!wand.area.IsPos1Initialized() || !wand.area.IsPos2Initialized())
            {
                Chat.Send(player, "<color=orange>The positions of the area have not beed initialized</color>");
                return true;
            }

            Vector3Int start = wand.area.Corner1;
            Vector3Int end = wand.area.Corner2;


            Vector3Int center = new((int)System.Math.Ceiling((start.x + end.x) / 2.0), start.y, (int)System.Math.Ceiling((start.z + end.z) / 2.0));

            Chat.Send(player, "<color=blue>Center: " + center + "</color>");

            for (int x = start.x; x <= end.x; x++)
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    for (int z = start.z; z <= end.z; z++)
                    {
                        Vector3Int newPos = new(x, y, z);
                        if (World.TryGetTypeAt(newPos, out ItemTypes.ItemType type))
                        {

                            if (tree.ContainsKey(type.Name))
                            {
                                List<Vector3Int> l = tree[type.Name];
                                l.Add(newPos - center);
                                tree.Remove(type.Name);
                                tree.Add(type.Name, l);
                            }
                            else
                            {
                                List<Vector3Int> l = new();
                                l.Add(newPos - center);
                                tree.Add(type.Name, l);
                            }
                        }
                    }
                }
            }

            JObject jTree = new JObject();
            JArray jBlocksOut = new JArray();

            foreach (var typeName in tree.Keys)
            {
                if (typeName.Equals("air"))
                    continue;

                JObject jBlocks = new JObject();
                JArray jBlocksPositions = new JArray();

                foreach (var j in tree[typeName])
                {
                    JObject vector = new JObject();
                    vector.SetAs("x", j.x);
                    vector.SetAs("y", j.y);
                    vector.SetAs("z", j.z);

                    jBlocksPositions.Add(vector);
                }

                jBlocks.SetAs("blocks", jBlocksPositions);
                jBlocks.SetAs("type", typeName);
                jBlocksOut.Add(jBlocks);
            }

            jTree.Add("blocks", jBlocksOut);

            string jsonF = JsonConvert.SerializeObject(jTree, Formatting.Indented); ;

            File.WriteAllText(StructureManager.Blueprint_FOLDER + filename + ".json", jsonF);

            Chat.Send(player, "<color=green>Saved tree: " + filename + "</color>");

            return true;
        }
    }
}
