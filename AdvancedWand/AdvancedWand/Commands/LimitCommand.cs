using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand.Commands
{
    [ChatCommandAutoLoader]
    public class LimitCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().StartsWith("//limit"))
                return false;

            //Player exists
            if(null == player || NetworkID.Server == player.ID)
                return true;

            //Check permissions
            if(!PermissionsManager.CheckAndWarnPermission(player, "khanx.wand"))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            //Wand is OFF
            if(!wand.active)
            {
                Chat.Send(player, "<color=orange>Wand is OFF, use //wand to activate</color>");
                return true;
            }

            if(1 == splits.Count)
            {
                Chat.Send(player, string.Format("<color=green>Limit: {0}</color>", wand.limit));
                return true;
            }

            if(2 != splits.Count)
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if(!int.TryParse(splits[1], out int newLimit))
            {
                Chat.Send(player, "<color=orange>Not number</color>");
                return true;
            }

            wand.limit = newLimit;
            Chat.Send(player, string.Format("<color=green>Limit: {0}</color>", newLimit));

            return true;
        }
    }
}
