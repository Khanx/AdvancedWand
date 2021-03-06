﻿using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class CopyCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().StartsWith("//copy"))
                return false;

            if(!CommandHelper.CheckCommand(player))
                return true;

            if(0 >= splits.Count)
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if(!CommandHelper.CheckLimit(player))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            wand.copy = new Blueprint(wand.area, player);

            Chat.Send(player, "<color=olive>Copied area:</color>");
            Chat.Send(player, string.Format("<color=lime>X: {0}</color>", wand.copy.xSize));
            Chat.Send(player, string.Format("<color=lime>Y: {0}</color>", wand.copy.ySize));
            Chat.Send(player, string.Format("<color=lime>Z: {0}</color>", wand.copy.zSize));
            Chat.Send(player, string.Format("<color=lime>Total: {0}</color>", wand.copy.xSize * wand.copy.ySize * wand.copy.zSize));

            Chat.Send(player, string.Format("<color=lime>Copied the selected area</color>"));

            return true;
        }
    }
}
