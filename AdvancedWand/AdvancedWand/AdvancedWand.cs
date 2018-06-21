using AdvancedWand.Helper;
using Pipliz;
using System.Collections.Generic;

namespace AdvancedWand
{
    [ModLoader.ModManager]
    public class AdvancedWand
    {
        private static readonly Dictionary<Players.Player, AdvancedWand> advancedWands = new Dictionary<Players.Player, AdvancedWand>();
        //It's a Dictionary because in a server can be more than one player

        public bool active = false;
        public int limit = 100000;
        public SelectedArea area = new SelectedArea();
        public Blueprint copy;

        private AdvancedWand() { }

        public static AdvancedWand GetAdvancedWand(Players.Player player)
        {
            if(player != null)
            {
                if(!advancedWands.ContainsKey(player))
                    advancedWands.Add(player, new AdvancedWand());

                return advancedWands[player];
            }

            return null;
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnPlayerDisconnected, "Khanx.AdvancedWand.RemoveWandOnPlayerDisconnected")]
        public static void RemoveAdvancedWand(Players.Player player)
        {
            if(player != null)
                advancedWands.Remove(player);
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnSendAreaHighlights, "Khanx.AdvancedWand.ShowArea")]
        public static void OnSendAreaHighlights(Players.Player player, List<AreaJobTracker.AreaHighlight> list, List<ushort> showWhileHoldingTypes)
        {
            if(null != player)
            {
                if(advancedWands.ContainsKey(player))
                {
                    if(!advancedWands[player].active)
                        return;

                    SelectedArea area = advancedWands[player].area;

                    if(area.pos1 == Vector3Int.maximum || area.pos2 == Vector3Int.maximum)
                        return;

                    showWhileHoldingTypes.Add(BlockTypes.Builtin.BuiltinBlocks.BronzeAxe);
                    list.Add(new AreaJobTracker.AreaHighlight(area.corner1, area.corner2, Shared.EAreaMeshType.AutoSelect, Shared.EAreaType.BuilderArea));
                }
            }
        }
    }
}
