using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand.Commands
{
    [AutoLoadCommand]
    public class SizeCommand : BaseCommand
    {
        public SizeCommand()
        {
            equalsTo.Add("//size");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(!AdvancedWandHelper.CheckCommand(player, arg, 1, out string[] args))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            int x = Math.Abs(wand.pos1.x - wand.pos2.x) + 1; //+1 is to take into account the start block
            int y = Math.Abs(wand.pos1.y - wand.pos2.y) + 1;
            int z = Math.Abs(wand.pos1.z - wand.pos2.z) + 1;

            int blocks_in_selected_area = x * y * z;

            Pipliz.Chatting.Chat.Send(player, "<color=olive>Area size:</color>");
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>x: {0}</color>", x));
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>y: {0}</color>", y));
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>z: {0}</color>", z));
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>total: {0}</color>", blocks_in_selected_area));

            return true;
        }
    }
}
