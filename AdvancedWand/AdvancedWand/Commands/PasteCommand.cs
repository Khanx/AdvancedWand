using AdvancedWand.Helper;
using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand
{
    [AutoLoadCommand]
    public class PasteCommand : BaseCommand
    {
        public PasteCommand()
        {
            startWith.Add("//paste");
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

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);
            Blueprint b = wand.copy;

            if(b == null)
            {
                Pipliz.Chatting.Chat.Send(player, string.Format("<color=orange>There is nothing copied</color>"));
                return true;
            }

            int fail = 0;

            for(int x = 0; x < b.xSize; x++)
                for(int y = 0; y < b.ySize; y++)
                    for(int z = 0; z < b.zSize; z++)
                    {
                        Vector3Int newPosition = new Vector3Int(player.Position) - b.playerMod + new Vector3Int(x, y, z);
                        if(!ServerManager.TryChangeBlock(newPosition, b.blocks[x, y, z]))
                            fail++;
                    }

            if(fail > 0)
                Pipliz.Chatting.Chat.Send(player, string.Format("<color=orange>{0} blocks have not been paste</color>", fail));

            return true;
        }
    }
}
