using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;
using Pandaros.SchematicBuilder.NBT;
using System.IO;

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

            if (wand.copy == null)
            {
                Chat.Send(player, string.Format("<color=orange>There is nothing to save.</color>"));
                return true;
            }

            if (File.Exists(GameLoader.Schematic_FOLDER + blueprintName + ".csschematic"))
            {
                Chat.Send(player, string.Format("<color=orange>A blueprint with that name already exists.</color>"));
                return true;
            }


            Schematic schematic = wand.copy;
            schematic.Name = blueprintName;
            SchematicReader.SaveSchematic(schematic);

            Chat.Send(player, string.Format("<color=lime>Blueprint saved as: {0}</color>", blueprintName));

            return true;
        }
    }
}
