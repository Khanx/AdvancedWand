﻿using AdvancedWand.Helper;
using ExtendedAPI.Commands;

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
            if(!CommandHelper.CheckCommand(player))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Pipliz.Chatting.Chat.Send(player, "<color=olive>Area size:</color>");
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>x: {0}</color>", wand.area.GetXSize()));
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>y: {0}</color>", wand.area.GetYSize()));
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>z: {0}</color>", wand.area.GetZSize()));
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>total: {0}</color>", wand.area.GetSize()));

            return true;
        }
    }
}
