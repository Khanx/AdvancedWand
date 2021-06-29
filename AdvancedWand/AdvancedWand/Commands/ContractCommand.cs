using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand.Commands
{
    [ChatCommandAutoLoader]
    public class ContractCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if (!chat.Trim().ToLower().StartsWith("//contract"))
                return false;

            if (!CommandHelper.CheckCommand(player))
                return true;

            if (1 >= splits.Count)
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if (!int.TryParse(splits[1], out int quantity))
            {
                Chat.Send(player, "<color=orange>Not number</color>");
                return true;
            }

            string sdirection;

            if (2 == splits.Count)    //Default: Contract ? FORWARD
                sdirection = "forward";
            else
                sdirection = splits[2];

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);
            Vector3Int direction = CommandHelper.GetDirection(player.Forward, sdirection);

            Vector3Int start = wand.area.Corner1;
            Vector3Int end = wand.area.Corner2;

            if (1 == direction.x || 1 == direction.y || 1 == direction.z)
                end -= (direction * quantity);
            else
                start -= (direction * quantity);

            wand.area.SetCorner1(start, player);
            wand.area.SetCorner2(end, player);

            Chat.Send(player, string.Format("<color=green>Area contracted {0} block {1}</color>", quantity, sdirection));

            return true;
        }
    }
}
