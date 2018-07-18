using AdvancedWand.Helper;
using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand.Commands
{
    [AutoLoadCommand]
    class SaveCommand : BaseCommand
    {
        public SaveCommand()
        {
            startWith.Add("//save");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(!CommandHelper.CheckCommand(player))
                return true;

            if(!CommandHelper.CheckLimit(player))
                return true;

            if(!arg.Contains(" "))
            {
                Pipliz.Chatting.Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            string blueprintName = arg.Substring(arg.IndexOf(" ") + 1).Trim();

            blueprintName = blueprintName.Replace(" ", "_");

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);
            Blueprint blueprint = wand.copy;

            if(blueprint == null)
            {
                Pipliz.Chatting.Chat.Send(player, string.Format("<color=orange>There is nothing to save.</color>"));
                return true;
            }

            if(BlueprintManager._blueprints.ContainsKey(blueprintName))
            {
                Pipliz.Chatting.Chat.Send(player, string.Format("<color=orange>A blueprint with that name already exists.</color>"));
                return true;
            }

            blueprint.saveBlueprint(blueprintName);

            BlueprintManager._blueprints.Add(blueprintName, blueprint);

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Blueprint saved as: {0}</color>", blueprintName));

            return true;
        }
    }
}
