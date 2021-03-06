﻿using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class WandCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().StartsWith("//wand"))
                return false;

            //Player exists
            if(null == player || NetworkID.Server == player.ID)
                return true;

            //Check permissions
            if(!PermissionsManager.CheckAndWarnPermission(player, "khanx.wand"))
                return true;


            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            //Change the status of the wand
            wand.active = !wand.active;

            if(wand.active)
                Chat.Send(player, "<color=lime>Wand ON</color>");
            else
            {
                Chat.Send(player, "<color=lime>Wand OFF</color>");
                AdvancedWand.RemoveAdvancedWand(player);
            }

            return true;
        }
    }
}
