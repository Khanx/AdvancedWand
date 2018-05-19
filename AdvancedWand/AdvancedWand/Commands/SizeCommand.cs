using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand.Commands
    {

    [AutoLoadCommand]
    public class SizeCommand : BaseCommand
        {

        public SizeCommand()
            {
            startWith.Add("//size");
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

            Pipliz.Chatting.Chat.Send(player, "Area size:");
            Pipliz.Chatting.Chat.Send(player, string.Format("x: {0}", x));
            Pipliz.Chatting.Chat.Send(player, string.Format("y: {0}", y));
            Pipliz.Chatting.Chat.Send(player, string.Format("z: {0}", z));
            Pipliz.Chatting.Chat.Send(player, string.Format("total: {0}", blocks_in_selected_area));

            return true;
            }
        }

    }
