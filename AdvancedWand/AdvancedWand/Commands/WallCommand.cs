using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class WallCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if (!chat.Trim().ToLower().StartsWith("//wall") && !chat.Trim().ToLower().StartsWith("//walls"))
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

            if (!CommandHelper.GetBlockIndex(player, splits[1], out ushort blockIndex))
                return true;

            ushort inner = BlockTypes.BuiltinBlocks.Indices.air;
            if (3 <= splits.Count)
                if (!CommandHelper.GetBlockIndex(player, splits[2], out inner))
                    return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Vector3Int start = wand.area.Corner1;
            Vector3Int end = wand.area.Corner2;

            for (int x = end.x; x >= start.x; x--)
                for (int y = end.y; y >= start.y; y--)
                    for (int z = end.z; z >= start.z; z--)
                    {
                        if (x == start.x || x == end.x || z == start.z || z == end.z)
                        {
                            Vector3Int newPos = new Vector3Int(x, y, z);
                            if (!World.TryGetTypeAt(newPos, out ushort actualType) || actualType != blockIndex)
                                AdvancedWand.AddAction(newPos, blockIndex, player);
                        }
                        else if (inner != BlockTypes.BuiltinBlocks.Indices.air)
                        {
                            Vector3Int newPos = new Vector3Int(x, y, z);
                            if (!World.TryGetTypeAt(newPos, out ushort actualType) || actualType != blockIndex)
                                AdvancedWand.AddAction(newPos, inner, player);
                        }
                    }

            Chat.Send(player, string.Format("<color=green>Wall: {0} {1}</color>", splits[1], inner));

            return true;
        }
    }
}
