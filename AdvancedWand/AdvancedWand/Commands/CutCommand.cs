using AdvancedWand.Helper;
using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand
{
    [AutoLoadCommand]
    public class CutCommand : BaseCommand
    {
        public CutCommand()
        {
            startWith.Add("//cut");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(!CommandHelper.CheckCommand(player))
                return true;

            string[] args = ChatCommands.CommandManager.SplitCommand(arg);

            if(0 >= args.Length)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Wrong Arguments</color>");
                return true;
            }

            if(!CommandHelper.CheckLimit(player))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            wand.copy = new Helper.Blueprint(wand.area, player);

            Vector3Int start = wand.area.corner1;
            Vector3Int end = wand.area.corner2;

            for(int x = start.x; x <= end.x; x++)
                for(int y = start.y; y <= end.y; y++)
                    for(int z = start.z; z <= end.z; z++)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        ServerManager.TryChangeBlock(newPos, BlockTypes.Builtin.BuiltinBlocks.Air);
                    }

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>Cutted the selected area</color>"));

            return true;
        }
    }
}
