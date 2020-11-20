using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;
using AdvancedWand.Persistence;

namespace AdvancedWand.Commands
{
    [ChatCommandAutoLoader]
    class SaveCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().StartsWith("//save"))
                return false;

            if(!CommandHelper.CheckCommand(player))
                return true;

            if(!CommandHelper.CheckLimit(player))
                return true;

            if(!chat.Contains(" "))
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            string blueprintName = chat.Substring(chat.IndexOf(" ") + 1).Trim();

            blueprintName = blueprintName.Replace(" ", "_");

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);
            Blueprint blueprint = (Blueprint)wand.copy; //KHANX: STRUCTURE PROBLEM

            if(blueprint == null)
            {
                Chat.Send(player, string.Format("<color=orange>There is nothing to save.</color>"));
                return true;
            }

            if(StructureManager._structures.ContainsKey(blueprintName))
            {
                Chat.Send(player, string.Format("<color=orange>A blueprint with that name already exists.</color>"));
                return true;
            }

            StructureManager.SaveStructure(blueprint, blueprintName);


            Chat.Send(player, string.Format("<color=green>Blueprint saved as: {0}</color>", blueprintName));

            return true;
        }
    }
}
