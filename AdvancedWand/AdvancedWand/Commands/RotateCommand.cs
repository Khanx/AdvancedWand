﻿using System.Collections.Generic;
using AdvancedWand.Helper;
using Pipliz;
using Chatting;
using Pandaros.SchematicBuilder.NBT;

namespace AdvancedWand.Commands
{
    [ChatCommandAutoLoader]
    class RotateCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if(!chat.Trim().ToLower().StartsWith("//rotate"))
                return false;

            if(!CommandHelper.CheckCommand(player))
                return true;

            if(!CommandHelper.CheckLimit(player))
                return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Schematic tmpCopy = new Schematic(player.Name + "tmpcopy", wand.area.GetXSize(), wand.area.GetYSize(), wand.area.GetZSize(), wand.area.corner1);

            Chat.Send(player, "Size before rotating = " + tmpCopy.Blocks.GetLength(0) + ", " + tmpCopy.Blocks.GetLength(1) + ", " + tmpCopy.Blocks.GetLength(2));
            Chat.Send(player, "Size before rotating = " + tmpCopy.XMax + ", " + tmpCopy.YMax + ", " + tmpCopy.ZMax);

            tmpCopy.Rotate();

            Chat.Send(player, "Size after rotating = " + tmpCopy.Blocks.GetLength(0) + ", " + tmpCopy.Blocks.GetLength(1) + ", " + tmpCopy.Blocks.GetLength(2));
            Chat.Send(player, "Size after rotating = " + tmpCopy.XMax + ", " + tmpCopy.YMax + ", " + tmpCopy.ZMax);
            Chat.Send(player, "0,0,0: " + tmpCopy.Blocks[0, 0, 0].BlockID);

            Vector3Int start = wand.area.corner1;
            Vector3Int end = wand.area.corner2;

            for (int x = end.x; x >= start.x; x--)
                for (int y = end.y; y >= start.y; y--)
                    for (int z = end.z; z >= start.z; z--)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        if (!World.TryGetTypeAt(newPos, out ushort actualType) || actualType != BlockTypes.BuiltinBlocks.Indices.air)
                            AdvancedWand.AddAction(newPos, BlockTypes.BuiltinBlocks.Indices.air);
                    }

            for (int Y = 0; Y < tmpCopy.YMax; Y++)
            {
                for (int Z = 0; Z < tmpCopy.ZMax; Z++)
                {
                    for (int X = 0; X < tmpCopy.XMax; X++)
                    {
                        Vector3Int newPosition = wand.area.corner1 + new Vector3Int(X, Y, Z);
                        AdvancedWand.AddAction(newPosition, ItemTypes.IndexLookup.GetIndex(tmpCopy.Blocks[X, Y, Z].BlockID));
                    }
                }
            }

            return true;
        }
    }
}
