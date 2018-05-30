using AdvancedWand.Helper;
using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand
{
    [AutoLoadCommand]
    public class ExpandCommand : BaseCommand
    {
        public ExpandCommand()
        {
            startWith.Add("//expand");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(!CommandHelper.CheckCommand(player))
                return true;

            string[] args = ChatCommands.CommandManager.SplitCommand(arg);

            if(1 >= args.Length)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Wrong Arguments</color>");
                return true;
            }

            if(!int.TryParse(args[1], out int quantity))
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Not number</color>");
                return true;
            }

            string sdirection;

            if(2 == args.Length)    //Default: Expand ? FORWARD
                sdirection = "forward";
            else
                sdirection = args[2];

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);
            Vector3Int direction = CommandHelper.GetDirection(player.Forward, sdirection);

            Vector3Int start = wand.area.corner1;
            Vector3Int end = wand.area.corner2;

            if(1 == direction.x || 1 == direction.y || 1 == direction.z)
                end += ( direction * quantity );
            else
                start += ( direction * quantity );

            wand.area.SetCorner1(start);
            wand.area.SetCorner2(end);

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>Area expanded {0} block {1}</color>", quantity, sdirection));

            return true;
        }
    }
}
