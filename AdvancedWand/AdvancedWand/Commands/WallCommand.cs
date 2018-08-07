using AdvancedWand.Helper;
using BlockTypes.Builtin;
using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand
{
    [AutoLoadCommand]
    public class WallCommand : BaseCommand
    {
        public WallCommand()
        {
            startWith.Add("//wall");
            startWith.Add("//walls");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(!CommandHelper.CheckCommand(player))
                return true;

            string[] args = ChatCommands.CommandManager.SplitCommand(arg);

            if(0 >= args.Length)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if(!CommandHelper.CheckLimit(player))
                return true;

            if(!CommandHelper.GetBlockIndex(player, args[1], out ushort blockIndex))
                return true;

            ushort inner = BuiltinBlocks.Air;
            if(3 <= args.Length)
                if(!CommandHelper.GetBlockIndex(player, args[2], out inner))
                    return true;

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Vector3Int start = wand.area.corner1;
            Vector3Int end = wand.area.corner2;

            for(int x = end.x; x >= start.x; x--)
                for(int y = end.y; y >= start.y; y--)
                    for(int z = end.z; z >= start.z; z--)
                    {
                        if(x == start.x || x == end.x || z == start.z || z == end.z)
                        {
                            Vector3Int newPos = new Vector3Int(x, y, z);
                            if(!World.TryGetTypeAt(newPos, out ushort actualType) || actualType != blockIndex)
                                AdvancedWand.AddAction(newPos, blockIndex);
                        }
                        else if(inner != BuiltinBlocks.Air)
                        {
                            Vector3Int newPos = new Vector3Int(x, y, z);
                            if(!World.TryGetTypeAt(newPos, out ushort actualType) || actualType != blockIndex)
                                AdvancedWand.AddAction(newPos, inner);
                        }
                    }

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Wall: {0} {1}</color>", args[1], inner));

            return true;
        }
    }
}
