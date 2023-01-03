using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class SetCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if (!chat.Trim().ToLower().StartsWith("//set"))
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

            ushort blockIndex = BlockTypes.BuiltinBlocks.Indices.air;  //Default: Set 0

            if (2 <= splits.Count)
                if (!CommandHelper.GetBlockIndex(player, splits[1], out blockIndex))
                    return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Vector3Int start = wand.area.Corner1;
            Vector3Int end = wand.area.Corner2;

            for (int x = end.x; x >= start.x; x--)
                for (int y = end.y; y >= start.y; y--)
                    for (int z = end.z; z >= start.z; z--)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        if (!World.TryGetTypeAt(newPos, out ushort actualType) || actualType != blockIndex)
                            AdvancedWand.AddAction(newPos, blockIndex, player);
                    }

            Chat.Send(player, string.Format("<color=green>Set: {0}</color>", blockIndex));

            return true;
        }
    }
}
