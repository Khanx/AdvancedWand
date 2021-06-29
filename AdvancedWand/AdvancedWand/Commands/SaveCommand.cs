using System.Collections.Generic;
using AdvancedWand.Helper;
using Chatting;
using AdvancedWand.Persistence;

namespace AdvancedWand.Commands
{
    [ChatCommandAutoLoader]
    class SaveCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if (!chat.Trim().ToLower().StartsWith("//save"))
                return false;

            if (!CommandHelper.CheckCommand(player))
                return true;

            if (!CommandHelper.CheckLimit(player))
                return true;

            //Save blueprintname [b/s]
            if (splits.Count < 2 || splits.Count > 3)
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                Chat.Send(player, "<color=orange>Count:" + splits.Count + "</color>");
                return true;
            }

            string structureName = splits[1].Trim();
            char stype = (splits.Count == 3) ? splits[2][0] : 'b';

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);
            Structure structure;
            if (stype.Equals('b'))
                structure = (Blueprint)wand.copy;
            else
                structure = new Schematic((Blueprint)wand.copy);

            if (structure == null)
            {
                Chat.Send(player, string.Format("<color=orange>There is nothing to save.</color>"));
                return true;
            }

            if (StructureManager._structures.ContainsKey(structureName))
            {
                Chat.Send(player, string.Format("<color=orange>A blueprint with that name already exists.</color>"));
                return true;
            }

            StructureManager.SaveStructure(structure, structureName);

            if (stype.Equals('b'))
                Chat.Send(player, string.Format("<color=green>Blueprint saved as: {0}</color>", structureName));
            else
                Chat.Send(player, string.Format("<color=green>Schematic saved as: {0}</color>", structureName));

            return true;
        }
    }
}
