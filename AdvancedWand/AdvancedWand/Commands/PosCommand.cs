using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand.Commands
{
    [ChatCommandAutoLoader]
    public class PosCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().Equals("//pos1") && !chat.Trim().ToLower().Equals("//pos2"))
                return false;

            //Player exists
            if(null == player || NetworkID.Server == player.ID)
                return false;

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

            Vector3Int newPos = new Vector3Int(player.Position);

            if(splits[0].Equals("//pos1"))
            {
                wand.area.SetCorner1(newPos, player);
                Chat.Send(player, string.Format("<color=lime>Pos 1: {0}</color>", newPos));
            }
            else //pos2
            {
                wand.area.SetCorner2(newPos, player);
                Chat.Send(player, string.Format("<color=lime>Pos 2: {0}</color>", newPos));
            }

            return true;
        }
    }
}
