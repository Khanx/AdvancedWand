using Chatting;
using static Shared.PlayerClickedData;

namespace AdvancedWand
{
    [ModLoader.ModManager]
    public static class AdvancedWandType
    {
        public static ItemTypes.ItemType wandType = BlockTypes.BuiltinBlocks.Types.bronzetools;

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnPlayerClicked, "Khanx.AdvancedWand.OnPlayerClicked")]
        public static void OnPlayerClicked(Players.Player player, Shared.PlayerClickedData playerClickedData)
        {
            if (playerClickedData.TypeSelected != wandType.ItemIndex)
                return;

            if (playerClickedData.ClickType == EClickType.Left)
                OnLeftClickWith(player, playerClickedData);
            else if (playerClickedData.ClickType == EClickType.Right)
                OnRightClickWith(player, playerClickedData);
        }


        public static void OnLeftClickWith(Players.Player player, Shared.PlayerClickedData boxedData)
        {
            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            if (!wand.active || boxedData.HitType != EHitType.Block || boxedData.GetVoxelHit().TypeHit == BlockTypes.BuiltinBlocks.Indices.air)
                return;

            wand.area.SetCorner1(boxedData.GetVoxelHit().BlockHit, player);

            Chat.Send(player, string.Format("<color=green>Pos 1: {0}</color>", boxedData.GetVoxelHit().BlockHit));
        }

        public static void OnRightClickWith(Players.Player player, Shared.PlayerClickedData boxedData)
        {
            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            if (!wand.active || boxedData.HitType != EHitType.Block || boxedData.GetVoxelHit().TypeHit == BlockTypes.BuiltinBlocks.Indices.air)
                return;

            wand.area.SetCorner2(boxedData.GetVoxelHit().BlockHit, player);

            Chat.Send(player, string.Format("<color=green>Pos 2: {0}</color>", boxedData.GetVoxelHit().BlockHit));
        }
    }
}
