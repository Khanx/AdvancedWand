using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;
using AdvancedWand.Persistence;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class PasteCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().StartsWith("//paste"))
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

            Structure structure = null;

            if(chat.Trim().Equals("//paste"))  //Paste from copy
            {
                structure = (Blueprint)wand.copy;   //KHANX: STRUCTURE PROBLEM

                if(structure == null)
                {
                    Chat.Send(player, string.Format("<color=orange>There is nothing copied</color>"));
                    return true;
                }
            }
            else    //Paste from loaded blueprint
            {
                string structureName = chat.Substring(chat.IndexOf(" ") + 1).Trim().ToLower();

                if(!StructureManager._structures.ContainsKey(structureName))
                {
                    Chat.Send(player, string.Format("<color=orange>There is not a bluerpint with that name.</color>"));

                    return true;
                }

                structure =  StructureManager.GetStructure(structureName);
            }

            Chat.Send(player, string.Format("<color=green>Pasting...</color>"));

            for(int x = 0; x <= structure.GetMaxX(); x++)
                for(int y = 0; y <= structure.GetMaxY(); y++)
                    for(int z = 0; z <= structure.GetMaxZ(); z++)
                    {
                        Vector3Int newPosition;
                        if(structure is Blueprint)
                            newPosition = new Vector3Int(player.Position) - ((Blueprint)structure).playerMod + new Vector3Int(x, y, z);
                        else
                            newPosition = new Vector3Int(player.Position) + new Vector3Int(x, y, z);

                        if (!World.TryGetTypeAt(newPosition, out ushort actualType) || actualType != structure.GetBlock(x, y, z))
                            AdvancedWand.AddAction(newPosition, structure.GetBlock(x,y,z), player);
                    }

            return true;
        }
    }
}
