using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand.Commands
{
    [ChatCommandAutoLoader]
    public class SizeCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().StartsWith("//size"))
                return false;

            if(!CommandHelper.CheckCommand(player))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Chat.Send(player, "<color=olive>Area size:</color>");
            Chat.Send(player, string.Format("<color=green>x: {0}</color>", wand.area.GetXSize()));
            Chat.Send(player, string.Format("<color=green>y: {0}</color>", wand.area.GetYSize()));
            Chat.Send(player, string.Format("<color=green>z: {0}</color>", wand.area.GetZSize()));
            Chat.Send(player, string.Format("<color=green>total: {0}</color>", wand.area.GetSize()));

            return true;
        }
    }
}
