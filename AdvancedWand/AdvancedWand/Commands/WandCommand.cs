using ExtendedAPI.Commands;

namespace AdvancedWand
    {

    [AutoLoadCommand]
    public class WandCommand : BaseCommand
        {

        public WandCommand()
            {
            //startWith.Add("//w");
            startWith.Add("//wand");
            }

        public override bool TryDoCommand(Players.Player player, string chat)
            {
            //Player exists
            if(null == player || NetworkID.Server == player.ID)
                return true;

            //Check permissions
            if(!Permissions.PermissionsManager.CheckAndWarnPermission(player, "khanx.wand"))
                return true;


            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            //Change the status of the wand
            wand.active = !wand.active;

            if(wand.active)
                Pipliz.Chatting.Chat.Send(player, "Wand ON");
            else
                {
                Pipliz.Chatting.Chat.Send(player, "Wand OFF");
                AdvancedWand.RemoveAdvancedWand(player);
                }

            return true;
            }

        }

    }
