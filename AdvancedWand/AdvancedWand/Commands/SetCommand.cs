using BlockTypes.Builtin;
using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand
{
    [AutoLoadCommand]
    public class SetCommand : BaseCommand
    {
        public SetCommand()
        {
            startWith.Add("//set");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(!AdvancedWandHelper.CheckCommand(player))
                return true;

            string[] args = ChatCommands.CommandManager.SplitCommand(arg);

            if(0 >= args.Length)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Wrong Arguments</color>");
                return true;
            }

            if(!AdvancedWandHelper.CheckLimit(player))
                return true;

            ushort blockIndex = BuiltinBlocks.Air;  //Default: Set 0

            if(2 <= args.Length)
                if(!AdvancedWandHelper.GetBlockIndex(player, args[1], out blockIndex))
                    return true;

            AdvancedWandHelper.GenerateCorners(player, out Vector3Int start, out Vector3Int end);

            for(int x = start.x; x <= end.x; x++)
                for(int y = start.y; y <= end.y; y++)
                    for(int z = start.z; z <= end.z; z++)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        ServerManager.TryChangeBlock(newPos, blockIndex);
                    }

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>Set: {0}</color>", blockIndex));

            return true;
        }
    }
}
