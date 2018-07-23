using AdvancedWand.Helper;
using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand.Commands
{
    [AutoLoadCommand]
    class RotateCommand : BaseCommand
    {
        public RotateCommand()
        {
            startWith.Add("//rotate");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(!CommandHelper.CheckCommand(player))
                return true;

            if(!CommandHelper.CheckLimit(player))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Blueprint blueprint = new Blueprint(wand.area, player);

            blueprint.Rotate();

            for(int x = blueprint.xSize; x >= 0; x--)
                for(int y = blueprint.ySize; y >= 0; y--)
                    for(int z = blueprint.zSize; z >= 0; z--)
                    {
                        Vector3Int newPosition = new Vector3Int(player.Position) - blueprint.playerMod + new Vector3Int(x, y, z);
                        AdvancedWand.AddAction(newPosition, blueprint.blocks[x, y, z]);
                    }

            return true;
        }
    }
}
