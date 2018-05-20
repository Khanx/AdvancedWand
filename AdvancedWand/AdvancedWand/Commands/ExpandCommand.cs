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
            if(!AdvancedWandHelper.CheckCommand(player, arg, 3, out string[] args))
                return true;

            if(!int.TryParse(args[1], out int quantity))
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Not number</color>");
                return true;
            }

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);
            Vector3Int direction = AdvancedWandHelper.GetDirection(player.Forward, args[2]);
            AdvancedWandHelper.GenerateCorners(player, out Vector3Int start, out Vector3Int end);

            if(1 == direction.x || 1 == direction.y || 1 == direction.z)
                end += ( direction * quantity );
            else
                start += ( direction * quantity );

            wand.pos1 = start;
            wand.pos2 = end;

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>Area expanded {0} block {1}</color>", quantity, args[2]));

            return true;
        }
    }
}
