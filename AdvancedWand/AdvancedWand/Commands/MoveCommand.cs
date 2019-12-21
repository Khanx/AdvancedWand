using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;
using Pandaros.SchematicBuilder.NBT;

namespace AdvancedWand
{
    [ChatCommandAutoLoader]
    public class MoveCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().StartsWith("//move"))
                return false;

            if(!CommandHelper.CheckCommand(player))
                return true;

            if(1 >= splits.Count)
            {
                Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if(!CommandHelper.CheckLimit(player))
                return true;

            if(!int.TryParse(splits[1], out int quantity))
            {
                Chat.Send(player, "<color=orange>Not number</color>");
                return true;
            }

            string sdirection;

            if(2 == splits.Count)    //Default: Contract ? FORWARD
                sdirection = "forward";
            else
                sdirection = splits[2];

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            //Create a temporal copy
            Schematic tmpCopy = new Schematic(player.Name + "tmpcopy", wand.area.GetXSize(), wand.area.GetYSize(), wand.area.GetZSize(), wand.area.corner1);

            //Remove the selected area
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

            //Paste the temporal copy
            Vector3Int direction = ( CommandHelper.GetDirection(player.Forward, sdirection) * quantity );

            for (int Y = 0; Y < tmpCopy.YMax; Y++)
            {
                for (int Z = 0; Z < tmpCopy.ZMax; Z++)
                {
                    for (int X = 0; X < tmpCopy.XMax; X++)
                    {
                        Vector3Int newPosition = wand.area.corner1 + direction + new Vector3Int(X, Y, Z);
                        AdvancedWand.AddAction(newPosition, ItemTypes.IndexLookup.GetIndex(tmpCopy.Blocks[X, Y, Z].BlockID));
                    }
                }
            }

            Chat.Send(player, string.Format("<color=lime>Moved {0} {1} the selected area</color>", quantity, sdirection));

            return true;
        }
    }
}
