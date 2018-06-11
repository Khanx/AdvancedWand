using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand.Commands
{
    [AutoLoadCommand]
    public class PosCommand : BaseCommand
    {
        public PosCommand()
        {
            equalsTo.Add("//pos1");
            equalsTo.Add("//pos2");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            //Player exists
            if(null == player || NetworkID.Server == player.ID)
                return false;

            //Check permissions
            if(!Permissions.PermissionsManager.CheckAndWarnPermission(player, "khanx.wand"))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            //Wand is OFF
            if(!wand.active)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=orange>Wand is OFF, use //wand to activate</color>");
                return false;
            }

            string[] args = ChatCommands.CommandManager.SplitCommand(arg);

            Vector3Int newPos = new Vector3Int(player.Position);

            if(args[0].Equals("//pos1"))
            {
                wand.area.SetCorner1(newPos);
                Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Pos 1: {0}</color>", newPos));
            }
            else //pos2
            {
                wand.area.SetCorner2(newPos);
                Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Pos 2: {0}</color>", newPos));
            }

            return true;
        }
    }
}
