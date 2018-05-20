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
        public Vector3Int pos1;
        public Vector3Int pos2;


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
    }
}
