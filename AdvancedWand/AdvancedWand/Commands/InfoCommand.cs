using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand.Commands
{
    [ChatCommandAutoLoader]
    public class InfoCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().StartsWith("//info"))
                return false;

            if(!CommandHelper.CheckCommand(player))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Vector3Int start = wand.area.corner1;
            Vector3Int end = wand.area.corner2;

            Dictionary<ushort, int> info = new Dictionary<ushort, int>();


            for(int x = start.x; x <= end.x; x++)
                for(int y = start.y; y <= end.y; y++)
                    for(int z = start.z; z <= end.z; z++)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        if(World.TryGetTypeAt(newPos, out ushort actualType))
                        {

                            ItemTypes.ItemType type = ItemTypes.GetType(actualType);
                            if(!ItemTypes.NotableTypes.Contains(type))
                            {
                                while(type.ParentItemType != null)
                                {
                                    type = type.ParentItemType;
                                }
                                actualType = type.ItemIndex;
                            }

                            if(info.TryGetValue(actualType, out int value))
                                info[actualType] = value + 1;
                            else
                                info[actualType] = 1;
                        }
                    }

            //Las camas son de dos bloques
            if(info.ContainsKey(ItemTypes.IndexLookup.GetIndex("bed")))
                info[ItemTypes.IndexLookup.GetIndex("bed")] /= 2;

            Chat.Send(player, "<color=green>Area info:</color>");

            foreach(ushort key in info.Keys)
            {
                if(key == 0)
                    continue;
                string name = ItemTypes.IndexLookup.GetName(key);
                Chat.Send(player, string.Format("<color=green>{0}({1}): {2}</color>", name, key, info[key]));
            }

            return true;
        }
    }
}
