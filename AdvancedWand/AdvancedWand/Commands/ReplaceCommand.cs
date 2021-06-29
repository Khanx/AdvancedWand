using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class ReplaceCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if (!chat.Trim().ToLower().StartsWith("//replace"))
                return false;

            if (!CommandHelper.CheckCommand(player))
                return true;

            if (1 >= splits.Count)
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if (!CommandHelper.CheckLimit(player))
                return true;

            if (2 == splits.Count) //Default replace ALL ?
            {
                if (!CommandHelper.GetBlockIndex(player, splits[1], out ushort newBlock))
                    return true;

                AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

                Vector3Int start = wand.area.Corner1;
                Vector3Int end = wand.area.Corner2;

                for (int x = end.x; x >= start.x; x--)
                    for (int y = end.y; y >= start.y; y--)
                        for (int z = end.z; z >= start.z; z--)
                        {
                            Vector3Int newPos = new(x, y, z);
                            if (!World.TryGetTypeAt(newPos, out ushort actualType) || (actualType != BlockTypes.BuiltinBlocks.Indices.air))
                                AdvancedWand.AddAction(newPos, newBlock, player);

                        }

                Chat.Send(player, string.Format("<color=green>ALL -> {0}</color>", splits[1]));
            }
            else
            {
                if (!CommandHelper.GetBlockIndex(player, splits[1], out ushort oldBlock))
                    return true;

                if (!CommandHelper.GetBlockIndex(player, splits[2], out ushort newBlock))
                    return true;

                AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

                Vector3Int start = wand.area.Corner1;
                Vector3Int end = wand.area.Corner2;

                for (int x = end.x; x >= start.x; x--)
                    for (int y = end.y; y >= start.y; y--)
                        for (int z = end.z; z >= start.z; z--)
                        {
                            Vector3Int newPos = new(x, y, z);
                            if (!World.TryGetTypeAt(newPos, out ushort actualType) || actualType == oldBlock)
                                AdvancedWand.AddAction(newPos, newBlock, player);
                        }

                Chat.Send(player, string.Format("<color=green>{0} -> {1}</color>", splits[1], splits[2]));
            }

            return true;
        }
    }
}
