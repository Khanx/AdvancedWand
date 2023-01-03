using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand.Commands
{
    [ChatCommandAutoLoader]
    public class CountCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if (!chat.Trim().ToLower().StartsWith("//count"))
                return false;

            if (!CommandHelper.CheckCommand(player))
                return true;

            if (2 != splits.Count)
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Vector3Int start = wand.area.Corner1;
            Vector3Int end = wand.area.Corner2;

            ushort blockIndex;
            try
            {
                bool isNumeric = int.TryParse(splits[1], out int IDBlock);

                if (isNumeric)
                    blockIndex = (ushort)IDBlock;
                else
                    blockIndex = ItemTypes.IndexLookup.GetIndex(splits[1]);
            }
            catch (System.ArgumentException)
            {
                Chat.Send(player, "<color=orange>Block not found</color>");
                return true;
            }

            int count = 0;

            for (int x = start.x; x <= end.x; x++)
                for (int y = start.y; y <= end.y; y++)
                    for (int z = start.z; z <= end.z; z++)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        if (World.TryGetTypeAt(newPos, out ushort actualType))
                        {
                            if (actualType == blockIndex)
                                count++;
                            else
                            {
                                ItemTypes.ItemType type = ItemTypes.GetType(actualType);

                                if (type.ParentItemType != null && type.ParentItemType.ItemIndex == blockIndex)
                                    count++;
                            }
                        }
                    }

            //Las camas son de dos bloques
            if (blockIndex == ItemTypes.IndexLookup.GetIndex("bed"))
                count /= 2;

            Chat.Send(player, string.Format("<color=green>{0}({1}): {2}</color>", splits[1], blockIndex, count));

            return true;
        }
    }
}
