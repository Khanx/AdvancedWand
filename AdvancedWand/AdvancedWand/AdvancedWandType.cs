using Pipliz;
using Chatting;
using static Shared.PlayerClickedData;

namespace AdvancedWand
{
    [ModLoader.ModManager]
    public static class AdvancedWandType
    {
        public static ItemTypes.ItemType wandType = BlockTypes.BuiltinBlocks.Types.bronzeaxe;

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnPlayerClicked, "Khanx.ExtendedAPI.OnPlayerClickedType")]
        public static void OnPlayerClicked(Players.Player player, Box<Shared.PlayerClickedData> playerClickedData)
        {
            ItemTypes.ItemType typeWith = null;

            if(playerClickedData.item1.typeSelected != wandType.ItemIndex)
                return;

                if(playerClickedData.item1.clickType == ClickType.Left)
                    OnLeftClickWith(player, playerClickedData);
                else if(playerClickedData.item1.clickType == ClickType.Right)
                    OnRightClickWith(player, playerClickedData);
        }
    

    public static void OnLeftClickWith(Players.Player player, Box<Shared.PlayerClickedData> boxedData)
        {
            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            if(!wand.active || boxedData.item1.typeHit == BlockTypes.BuiltinBlocks.Indices.air)
                return;

            wand.area.SetCorner1(boxedData.item1.VoxelHit, player);

            Chat.Send(player, string.Format("<color=lime>Pos 1: {0}</color>", boxedData.item1.VoxelHit));
        }

        public static void OnRightClickWith(Players.Player player, Box<Shared.PlayerClickedData> boxedData)
        {
            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            if(!wand.active || boxedData.item1.typeHit == BlockTypes.BuiltinBlocks.Indices.air)
                return;

            wand.area.SetCorner2(boxedData.item1.VoxelHit, player);

            Chat.Send(player, string.Format("<color=lime>Pos 2: {0}</color>", boxedData.item1.VoxelHit));
        }
    }
}
