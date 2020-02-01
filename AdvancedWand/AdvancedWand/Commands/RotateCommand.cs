using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;
using AdvancedWand.Persistence;

namespace AdvancedWand.Commands
{
    [ChatCommandAutoLoader]
    class RotateCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().StartsWith("//rotate"))
                return false;

            if(!CommandHelper.CheckCommand(player))
                return true;

            if(!CommandHelper.CheckLimit(player))
                return true;

            //LEFT
            int direction = 1;

            if (2 <= splits.Count)
            {
                switch (splits[1].Trim().ToLower())
                {
                    case "l":
                    case "left":
                    case "270":
                        direction = 3;
                        break;
                    case "r":
                    case "right":
                    case "90":
                        direction = 1;
                        break;
                    case "b":
                    case "back":
                    case "f":
                    case "flip":
                    case "180":
                        direction = 2;
                        break;
                    default:
                        direction = 1;
                        break;
                }
            }


            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Blueprint blueprint = new Blueprint(wand.area, player);

            Vector3Int start = wand.area.corner1;
            Vector3Int end = wand.area.corner2;

            //Remove the current area
            for(int x = end.x; x >= start.x; x--)
                for(int y = end.y; y >= start.y; y--)
                    for(int z = end.z; z >= start.z; z--)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        if(!World.TryGetTypeAt(newPos, out ushort actualType) || actualType != BlockTypes.BuiltinBlocks.Indices.air)
                            AdvancedWand.AddAction(newPos, BlockTypes.BuiltinBlocks.Indices.air, player);
                    }

            for(int i =0;i< direction;i++)
                blueprint.Rotate();

            for(int x = 0; x <= blueprint.GetMaxX(); x++)
                for(int y = 0; y <= blueprint.GetMaxY(); y++)
                    for(int z = 0; z <= blueprint.GetMaxZ(); z++)
                    {
                        Vector3Int newPosition = new Vector3Int(player.Position) - blueprint.playerMod + new Vector3Int(x, y, z);
                        //DONT USE THIS IT CAN CAUSE PROBLEMS!!!
                        //if(!World.TryGetTypeAt(newPosition, out ushort actualType) || actualType != blueprint.blocks[x, y, z])
                        AdvancedWand.AddAction(newPosition, blueprint.GetBlock(x,y,z), player);
                    }

            return true;
        }
    }
}
