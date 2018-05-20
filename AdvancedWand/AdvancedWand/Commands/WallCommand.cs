﻿using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand
{
    [AutoLoadCommand]
    public class WallCommand : BaseCommand
    {
        public WallCommand()
        {
            startWith.Add("//wall");
            startWith.Add("//walls");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(!AdvancedWandHelper.CheckCommand(player, arg, 2, out string[] args))
                return true;

            if(!AdvancedWandHelper.CheckLimit(player))
                return true;

            if(!AdvancedWandHelper.GetBlockIndex(player, args[1], out ushort blockIndex))
                return true;

            AdvancedWandHelper.GenerateCorners(player, out Vector3Int start, out Vector3Int end);

            for(int x = start.x; x <= end.x; x++)
                for(int y = start.y; y <= end.y; y++)
                    for(int z = start.z; z <= end.z; z++)
                    {
                        if(x == start.x || x == end.x || z == start.z || z == end.z)
                        {
                            Vector3Int newPos = new Vector3Int(x, y, z);
                            ServerManager.TryChangeBlock(newPos, blockIndex);
                        }
                        else
                        {
                            Vector3Int newPos = new Vector3Int(x, y, z);
                            ServerManager.TryChangeBlock(newPos, BlockTypes.Builtin.BuiltinBlocks.Air);
                        }
                    }

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>Wall: {0}</color>", args[1]));

            return true;
        }
    }
}
