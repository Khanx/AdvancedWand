using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;
using Pandaros.SchematicBuilder.NBT;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class CopyCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().StartsWith("//copy"))
                return false;

            if(!CommandHelper.CheckCommand(player))
                return true;

            if(0 >= splits.Count)
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if(!CommandHelper.CheckLimit(player))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            wand.copy = new Schematic(player.Name + "tmpcopy", wand.area.GetXSize(), wand.area.GetYSize(), wand.area.GetZSize(), wand.area.corner1);

            /*
            Chat.Send(player, "<color=olive>Copied area:</color>");
            
            Chat.Send(player, string.Format("<color=lime>X: {0}</color>", wand.copy.XMax));
            Chat.Send(player, string.Format("<color=lime>Y: {0}</color>", wand.copy.YMax));
            Chat.Send(player, string.Format("<color=lime>Z: {0}</color>", wand.copy.ZMax));
            Chat.Send(player, string.Format("<color=lime>Total: {0}</color>", wand.copy.XMax* wand.copy.YMax* wand.copy.ZMax));
            */
            Chat.Send(player, string.Format("<color=lime>Copied the selected area</color>"));

            return true;
        }
    }
}
