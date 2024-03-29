﻿using System.Collections.Generic;
using AdvancedWand.Helper;
using Chatting;
using AdvancedWand.Persistence;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class CopyCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if (!chat.Trim().ToLower().StartsWith("//copy"))
                return false;

            if (!CommandHelper.CheckCommand(player))
                return true;

            if (0 >= splits.Count)
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if (!CommandHelper.CheckLimit(player))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            wand.copy = new Blueprint(wand.area, player);

            Chat.Send(player, "<color=green>Copied area:</color>");
            Chat.Send(player, string.Format("<color=green>X: {0}</color>", wand.copy.GetMaxX() + 1));
            Chat.Send(player, string.Format("<color=green>Y: {0}</color>", wand.copy.GetMaxY() + 1));
            Chat.Send(player, string.Format("<color=green>Z: {0}</color>", wand.copy.GetMaxZ() + 1));
            Chat.Send(player, string.Format("<color=green>Total: {0}</color>", (wand.copy.GetMaxX() + 1) * wand.copy.GetMaxY() + 1 * wand.copy.GetMaxZ() + 1));

            Chat.Send(player, string.Format("<color=green>Copied the selected area</color>"));

            return true;
        }
    }
}
