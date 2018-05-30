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
                Pipliz.Chatting.Chat.Send(player, "<color=red>Wrong Arguments</color>");
                return true;
            }

            if(!CommandHelper.CheckLimit(player))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            wand.copy = new Helper.Blueprint(wand.area, player);

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>Copied the selected area</color>"));

            return true;
        }
    }
}
