﻿using AdvancedWand.Helper;
using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand
{
    [AutoLoadCommand]
    public class MoveCommand : BaseCommand
    {
        public MoveCommand()
        {
            startWith.Add("//move");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(!CommandHelper.CheckCommand(player))
                return true;

            string[] args = ChatCommands.CommandManager.SplitCommand(arg);

            if(1 >= args.Length)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Wrong Arguments</color>");
                return true;
            }

            if(!CommandHelper.CheckLimit(player))
                return true;

            if(!int.TryParse(args[1], out int quantity))
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Not number</color>");
                return true;
            }

            string sdirection;

            if(2 == args.Length)    //Default: Contract ? FORWARD
                sdirection = "forward";
            else
                sdirection = args[2];

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            //Create a temporal copy
            Helper.Blueprint tmpCopy = new Helper.Blueprint(wand.area, player);

            //Remove the selected area
            Vector3Int start = wand.area.corner1;
            Vector3Int end = wand.area.corner2;

            for(int x = start.x; x <= end.x; x++)
                for(int y = start.y; y <= end.y; y++)
                    for(int z = start.z; z <= end.z; z++)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        ServerManager.TryChangeBlock(newPos, BlockTypes.Builtin.BuiltinBlocks.Air);
                    }

            //Paste the temporal copy
            Vector3Int direction = ( CommandHelper.GetDirection(player.Forward, sdirection) * quantity );

            for(int x = 0; x < tmpCopy.xSize; x++)
                for(int y = 0; y < tmpCopy.ySize; y++)
                    for(int z = 0; z < tmpCopy.zSize; z++)
                    {
                        Vector3Int newPosition = new Vector3Int(player.Position) - tmpCopy.playerMod + direction + new Vector3Int(x, y, z);
                        ServerManager.TryChangeBlock(newPosition, tmpCopy.blocks[x, y, z]);
                    }

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>Moved {0} {1} the selected area</color>", quantity, sdirection));

            return true;
        }
    }
}