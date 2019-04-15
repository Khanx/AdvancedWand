using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class CutCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.StartsWith("//cut"))
                return false;

            if(!CommandHelper.CheckCommand(player))
                return true;

            if(0 >= splits.Count)
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if(!CommandHelper.CheckLimit(player))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            wand.copy = new Helper.Blueprint(wand.area, player);

            Vector3Int start = wand.area.corner1;
            Vector3Int end = wand.area.corner2;

            for(int x = end.x; x >= start.x; x--)
                for(int y = end.y; y >= start.y; y--)
                    for(int z = end.z; z >= start.z; z--)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        if(!World.TryGetTypeAt(newPos, out ushort actualType) || actualType != BlockTypes.BuiltinBlocks.Indices.air)
                            AdvancedWand.AddAction(newPos, BlockTypes.BuiltinBlocks.Indices.air);
                    }

            Chat.Send(player, string.Format("<color=lime>Cutted the selected area</color>"));


            return true;
        }
    }
}
