using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand.Commands
{
    [AutoLoadCommand]
    public class PosCommand : BaseCommand
    {
        public PosCommand()
        {
            startWith.Add("//pos1");
            startWith.Add("//pos2");
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
                Pipliz.Chatting.Chat.Send(player, "Wand is OFF, use //wand to activate");
                return false;
            }

            string[] args = ChatCommands.CommandManager.SplitCommand(arg);

            if(args[0].Equals("//pos1"))
            {
                wand.pos1 = new Vector3Int(player.Position);
                Pipliz.Chatting.Chat.Send(player, string.Format("Pos 1: {0}", wand.pos1));
            }
            else //pos2
            {
                wand.pos2 = new Vector3Int(player.Position);
                Pipliz.Chatting.Chat.Send(player, string.Format("Pos 2: {0}", wand.pos2));
            }

            return true;
        }
    }
}
