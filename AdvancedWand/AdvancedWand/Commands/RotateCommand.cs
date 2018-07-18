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

            for(int x = 0; x < blueprint.xSize; x++)
                for(int y = 0; y < blueprint.ySize; y++)
                    for(int z = 0; z < blueprint.zSize; z++)
                    {
                        Vector3Int newPosition = new Vector3Int(player.Position) - blueprint.playerMod + new Vector3Int(x, y, z);
                        ServerManager.TryChangeBlock(newPosition, blueprint.blocks[x, y, z]);
                    }

            return true;
        }
    }
}
