using AdvancedWand.Helper;
using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand
{
    [AutoLoadCommand]
    public class PasteCommand : BaseCommand
    {
        public PasteCommand()
        {
            startWith.Add("//paste");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(null == player || NetworkID.Server == player.ID)
                return false;

            //Player has permission
            if(!Permissions.PermissionsManager.CheckAndWarnPermission(player, "khanx.wand"))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            //Wand is OFF
            if(!wand.active)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=orange>Wand is OFF, use //wand to activate</color>");
                return false;
            }

            Blueprint blueprint = null;

            if(arg.Trim().Equals("//paste"))  //Paste from copy
            {
                blueprint = wand.copy;

                if(blueprint == null)
                {
                    Pipliz.Chatting.Chat.Send(player, string.Format("<color=orange>There is nothing copied</color>"));
                    return true;
                }
            }
            else    //Paste from loaded blueprint
            {
                string blueprintName = arg.Substring(arg.IndexOf(" ") + 1).Trim();

                if(!BlueprintManager._blueprints.TryGetValue(blueprintName, out blueprint))
                {
                    Pipliz.Chatting.Chat.Send(player, string.Format("<color=orange>There is not a bluerpint with that name.</color>"));
                    return true;
                }
            }


            int fail = 0;

            for(int x = 0; x < blueprint.xSize; x++)
                for(int y = 0; y < blueprint.ySize; y++)
                    for(int z = 0; z < blueprint.zSize; z++)
                    {
                        Vector3Int newPosition = new Vector3Int(player.Position) - blueprint.playerMod + new Vector3Int(x, y, z);
                        if(!ServerManager.TryChangeBlock(newPosition, blueprint.blocks[x, y, z]))
                            fail++;
                    }

            if(fail > 0)
                Pipliz.Chatting.Chat.Send(player, string.Format("<color=orange>{0} blocks have not been paste</color>", fail));

            return true;
        }
    }
}
