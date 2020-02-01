using System.Collections.Generic;
using System.IO;
using AdvancedWand.Helper;
using AdvancedWand.Persistence;
using Chatting;
using Pipliz;
using Pipliz.JSON;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class TreeCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().StartsWith("//tree"))
                return false;

            if(!CommandHelper.CheckCommand(player))
                return true;

            if(0 >= splits.Count)
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if(!CommandHelper.CheckLimit(player))
                return true;

            if (!chat.Contains(" "))
            {
                Chat.Send(player, "<color=orange>Wrong Arguments: //tree _treename_</color>");
                return true;
            }

            string filename = chat.Substring(chat.IndexOf(" ") + 1).Trim();

            filename = filename.Replace(" ", "_");

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Dictionary<string, List<Vector3Int>> tree = new Dictionary<string, List<Vector3Int>>();


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

            Vector3Int start = wand.area.corner1;
            Vector3Int end = wand.area.corner2;


            Vector3Int center = new Vector3Int((int)System.Math.Ceiling((start.x + end.x)/2.0), start.y, (int)System.Math.Ceiling((start.z + end.z)/2.0));

            Chat.Send(player, "<color=blue>Center: "+ center + "</color>");

            for (int x = start.x; x <= end.x; x++)
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    for (int z = start.z; z <= end.z; z++)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
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
                                List<Vector3Int> l = new List<Vector3Int>();
                                l.Add(newPos - center);
                                tree.Add(type.Name, l);
                            }
                        }
                    }
                }
            }

            JSONNode json = new JSONNode(NodeType.Array);
            JSONNode jsonTree = new JSONNode(NodeType.Object);
            JSONNode jsonBlocks1 = new JSONNode(NodeType.Array);

            foreach(var i in tree.Keys)
            {
                if (i.Equals("air"))
                    continue;

                JSONNode jsonNodeTree2 = new JSONNode(NodeType.Object);
                JSONNode jsonBlocks = new JSONNode(NodeType.Array);
                
                foreach(var j in tree[i])
                {
                    JSONNode vector = new JSONNode(NodeType.Object);
                    vector.SetAs("x", j.x);
                    vector.SetAs("y", j.y);
                    vector.SetAs("z", j.z);

                    jsonBlocks.AddToArray(vector);
                }

                jsonNodeTree2.SetAs("blocks", jsonBlocks);
                jsonNodeTree2.SetAs("type", i);
                jsonBlocks1.AddToArray(jsonNodeTree2);
            }

            jsonTree.SetAs("blocks", jsonBlocks1);
            json.AddToArray(jsonTree);

            JSON.Serialize(StructureManager.Blueprint_FOLDER + filename + ".json", json, 2);

            Chat.Send(player, "<color=olive>Saved tree: " + filename + "</color>");

            return true;
        }
    }
}
