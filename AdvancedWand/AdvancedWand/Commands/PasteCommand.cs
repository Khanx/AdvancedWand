using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;
using Pandaros.SchematicBuilder.NBT;

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

             Schematic schematic = null;

            if(chat.Trim().Equals("//paste"))  //Paste from copy
            {
                schematic = wand.copy;

                if(schematic == null)
                {
                    Chat.Send(player, string.Format("<color=orangewww>There is nothing copied</color>"));
                    return true;
                }
            }
            else    //Paste from loaded blueprint
            {
                string blueprintName = chat.Substring(chat.IndexOf(" ") + 1).Trim();

                if (SchematicReader.TryGetSchematic(blueprintName, Vector3Int.zero, out Schematic sch))
                    schematic = sch;
                else
                {
                    Chat.Send(player, string.Format("<color=orange>There is not a bluerpint with that name.</color>"));
                    return true;
                }
            }

            Chat.Send(player, string.Format("<color=lime>Pasting...</color>"));

            for (int Y = 0; Y <= schematic.YMax; Y++)
            {
                for (int Z = 0; Z <= schematic.ZMax; Z++)
                {
                    for (int X = 0; X <= schematic.XMax; X++)
                    {
                        Vector3Int newPosition = new Vector3Int(player.Position) + new Vector3Int(X,Y,Z);
                        AdvancedWand.AddAction(newPosition, ItemTypes.IndexLookup.GetIndex(schematic.Blocks[X,Y,Z].BlockID));
                    }
                }
            }


            return true;
        }
    }
}
