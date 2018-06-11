using ExtendedAPI.Commands;
using System;

namespace AdvancedWand.Commands
{
    [AutoLoadCommand]
    public class LimitCommand : BaseCommand
    {
        public LimitCommand()
        {
            startWith.Add("//limit");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            //Player exists
            if(null == player || NetworkID.Server == player.ID)
                return true;

            //Check permissions
            if(!Permissions.PermissionsManager.CheckAndWarnPermission(player, "khanx.wand"))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            //Wand is OFF
            if(!wand.active)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=orange>Wand is OFF, use //wand to activate</color>");
                return true;
            }

            String[] args = ChatCommands.CommandManager.SplitCommand(arg);

            if(1 == args.Length)
            {
                Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Limit: {0}</color>", wand.limit));
                return true;
            }

            if(2 != args.Length)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if(!int.TryParse(args[1], out int newLimit))
            {
                Pipliz.Chatting.Chat.Send(player, "<color=orange>Not number</color>");
                return true;
            }

            wand.limit = newLimit;
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Limit: {0}</color>", newLimit));

            return true;
        }
    }
}
