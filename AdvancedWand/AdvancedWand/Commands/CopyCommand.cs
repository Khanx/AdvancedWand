using AdvancedWand.Helper;
using ExtendedAPI.Commands;

namespace AdvancedWand
{
    [AutoLoadCommand]
    public class CopyCommand : BaseCommand
    {
        public CopyCommand()
        {
            startWith.Add("//copy");
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

            wand.copy = new Blueprint(wand.area, player);

            Pipliz.Chatting.Chat.Send(player, "<color=olive>Copied area:</color>");
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>X: {0}</color>", wand.copy.xSize));
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Y: {0}</color>", wand.copy.ySize));
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Z: {0}</color>", wand.copy.zSize));
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Total: {0}</color>", wand.copy.xSize * wand.copy.ySize * wand.copy.zSize));

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Copied the selected area</color>"));

            return true;
        }
    }
}
