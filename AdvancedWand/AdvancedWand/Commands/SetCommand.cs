using AdvancedWand.Helper;
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
            if(!CommandHelper.CheckCommand(player))
                return true;

            string[] args = ChatCommands.CommandManager.SplitCommand(arg);

            if(0 >= args.Length)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if(!CommandHelper.CheckLimit(player))
                return true;

            ushort blockIndex = BuiltinBlocks.Air;  //Default: Set 0

            if(2 <= args.Length)
                if(!CommandHelper.GetBlockIndex(player, args[1], out blockIndex))
                    return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Vector3Int start = wand.area.corner1;
            Vector3Int end = wand.area.corner2;

            for(int x = end.x; x >= start.x; x--)
                for(int y = end.y; y >= start.y; y--)
                    for(int z = end.z; z >= start.z; z--)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        AdvancedWand.AddAction(newPos, blockIndex);
                    }

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Set: {0}</color>", blockIndex));

            return true;
        }
    }
}
