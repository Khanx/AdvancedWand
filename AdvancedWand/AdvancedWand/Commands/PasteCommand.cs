using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class PasteCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.StartsWith("//paste"))
                return false;

            if(null == player || NetworkID.Server == player.ID)
                return false;

            //Player has permission
            if(!PermissionsManager.CheckAndWarnPermission(player, "khanx.wand"))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            //Wand is OFF
            if(!wand.active)
            {
                Chat.Send(player, "<color=orange>Wand is OFF, use //wand to activate</color>");
                return true;
            }

            Blueprint blueprint = null;

            if(chat.Trim().Equals("//paste"))  //Paste from copy
            {
                blueprint = wand.copy;

                if(blueprint == null)
                {
                    Chat.Send(player, string.Format("<color=orange>There is nothing copied</color>"));
                    return true;
                }
            }
            else    //Paste from loaded blueprint
            {
                string blueprintName = chat.Substring(chat.IndexOf(" ") + 1).Trim();

                if(!BlueprintManager._blueprints.TryGetValue(blueprintName, out blueprint))
                {
                    Chat.Send(player, string.Format("<color=orange>There is not a bluerpint with that name.</color>"));
                    return true;
                }
            }

            Chat.Send(player, string.Format("<color=lime>Pasting...</color>"));

            for(int x = 0; x < blueprint.xSize; x++)
                for(int y = 0; y < blueprint.ySize; y++)
                    for(int z = 0; z < blueprint.zSize; z++)
                    {
                        Vector3Int newPosition = new Vector3Int(player.Position) - blueprint.playerMod + new Vector3Int(x, y, z);
                        if(!World.TryGetTypeAt(newPosition, out ushort actualType) || actualType != blueprint.blocks[x, y, z])
                            AdvancedWand.AddAction(newPosition, blueprint.blocks[x, y, z]);
                    }

            return true;
        }
    }
}
