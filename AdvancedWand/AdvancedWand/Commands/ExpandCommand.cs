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
                return false;

            if(!int.TryParse(args[1], out int quantity))
            {
                Pipliz.Chatting.Chat.Send(player, "Not number");
                return false;
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

            Pipliz.Chatting.Chat.Send(player, string.Format("Expand {0} {1}", quantity, args[2]));

            return true;
        }
    }
}
