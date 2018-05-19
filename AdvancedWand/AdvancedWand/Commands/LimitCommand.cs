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
                return false;

            //Check permissions
            if(!Permissions.PermissionsManager.CheckAndWarnPermission(player, "khanx.wand"))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            //Wand is OFF
            if(!wand.active)
                {
                Pipliz.Chatting.Chat.Send(player, "Wand is OFF, use //wand to activate");
                return false;
                }

            String[] args = ChatCommands.CommandManager.SplitCommand(arg);

            if(1 == args.Length)
                {
                Pipliz.Chatting.Chat.Send(player, string.Format("Limit: {0}", wand.limit));
                return true;
                }

            if(2 != args.Length)
                {
                Pipliz.Chatting.Chat.Send(player, "Wrong Arguments");
                return false;
                }

            if(!int.TryParse(args[1], out int newLimit))
                {
                Pipliz.Chatting.Chat.Send(player, "Not number");
                return false;
                }

            wand.limit = newLimit;
            Pipliz.Chatting.Chat.Send(player, string.Format("Limit: {0}", newLimit));

            return true;
            }

        }

    }
