using ExtendedAPI.Types;
using Pipliz;
using Shared;

namespace AdvancedWand
    {

    [AutoLoadType]
    public class AdvancedWandType : BaseType
        {
        public AdvancedWandType() { key = "bronzeaxe"; }

        public override void OnLeftClickWith(Players.Player player, Box<PlayerClickedData> boxedData)
            {
            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            if(!wand.active)
                return;

            wand.pos1 = boxedData.item1.VoxelHit;

            Pipliz.Chatting.Chat.Send(player, string.Format("Pos 1: {0}", wand.pos1));
            }

        public override void OnRightClickWith(Players.Player player, Box<PlayerClickedData> boxedData)
            {
            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            if(!wand.active)
                return;

            wand.pos2 = boxedData.item1.VoxelHit;

            Pipliz.Chatting.Chat.Send(player, string.Format("Pos 2: {0}", wand.pos2));
            }
        }
    }
