using ExtendedAPI.Commands;
using Pipliz;
using System.Collections.Generic;

namespace AdvancedWand.Commands
{
    [AutoLoadCommand]
    public class InfoCommand : BaseCommand
    {
        public InfoCommand()
        {
            equalsTo.Add("//info");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(!AdvancedWandHelper.CheckCommand(player))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            AdvancedWandHelper.GenerateCorners(player, out Vector3Int start, out Vector3Int end);

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

            Pipliz.Chatting.Chat.Send(player, "<color=olive>Area info:</color>");

            foreach(ushort key in info.Keys)
            {
                if(key == 0)
                    continue;
                string name = ItemTypes.IndexLookup.GetName(key);
                Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>{0}: {1}</color>", name, info[key]));
            }

            return true;
        }
    }
}
