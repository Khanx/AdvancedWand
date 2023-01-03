using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;
using AdvancedWand.Persistence;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class MoveCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if (!chat.Trim().ToLower().StartsWith("//move"))
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

            if (!int.TryParse(splits[1], out int quantity))
            {
                Chat.Send(player, "<color=orange>Not number</color>");
                return true;
            }

            string sdirection;

            if (2 == splits.Count)    //Default: Contract ? FORWARD
                sdirection = "forward";
            else
                sdirection = splits[2];

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            //Create a temporal copy
            Blueprint tmpCopy = new Blueprint(wand.area, player);

            //Remove the selected area
            Vector3Int start = wand.area.Corner1;
            Vector3Int end = wand.area.Corner2;

            for (int x = end.x; x >= start.x; x--)
                for (int y = end.y; y >= start.y; y--)
                    for (int z = end.z; z >= start.z; z--)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        if (!World.TryGetTypeAt(newPos, out ushort actualType) || actualType != BlockTypes.BuiltinBlocks.Indices.air)
                            AdvancedWand.AddAction(newPos, BlockTypes.BuiltinBlocks.Indices.air, player);
                    }

            //Paste the temporal copy
            Vector3Int direction = (CommandHelper.GetDirection(player.Forward, sdirection) * quantity);

            for (int x = 0; x <= tmpCopy.GetMaxX(); x++)
                for (int y = 0; y <= tmpCopy.GetMaxY(); y++)
                    for (int z = 0; z <= tmpCopy.GetMaxZ(); z++)
                    {
                        Vector3Int newPosition = new Vector3Int(player.Position) - tmpCopy.playerMod + direction + new Vector3Int(x, y, z);
                        //DONT USE THIS IF CAN CAUSE PROBLEMS!!!
                        //if(!World.TryGetTypeAt(newPosition, out ushort actualType) || actualType != tmpCopy.blocks[x, y, z])
                        AdvancedWand.AddAction(newPosition, tmpCopy.GetBlock(x, y, z), player);
                    }

            Chat.Send(player, string.Format("<color=green>Moved {0} {1} the selected area</color>", quantity, sdirection));

            return true;
        }
    }
}
