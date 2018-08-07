using AdvancedWand.Helper;
using BlockTypes.Builtin;
using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand
{
    [AutoLoadCommand]
    public class ReplaceCommand : BaseCommand
    {
        public ReplaceCommand()
        {
            startWith.Add("//replace");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(!CommandHelper.CheckCommand(player))
                return true;

            string[] args = ChatCommands.CommandManager.SplitCommand(arg);

            if(1 >= args.Length)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=orange>Wrong Arguments</color>");
                return true;
            }

            if(!CommandHelper.CheckLimit(player))
                return true;

            if(2 == args.Length) //Default replace ALL ?
            {
                if(!CommandHelper.GetBlockIndex(player, args[1], out ushort newBlock))
                    return true;

                AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

                Vector3Int start = wand.area.corner1;
                Vector3Int end = wand.area.corner2;

                for(int x = end.x; x >= start.x; x--)
                    for(int y = end.y; y >= start.y; y--)
                        for(int z = end.z; z >= start.z; z--)
                        {
                            Vector3Int newPos = new Vector3Int(x, y, z);
                            if(!World.TryGetTypeAt(newPos, out ushort actualType) || ( actualType != BuiltinBlocks.Air && ItemTypes.NotableTypes.Contains(ItemTypes.GetType(actualType)) ))
                                AdvancedWand.AddAction(newPos, newBlock);

                        }

                Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>ALL -> {0}</color>", args[1]));
            }
            else
            {
                if(!CommandHelper.GetBlockIndex(player, args[1], out ushort oldBlock))
                    return true;

                if(!CommandHelper.GetBlockIndex(player, args[2], out ushort newBlock))
                    return true;

                AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

                Vector3Int start = wand.area.corner1;
                Vector3Int end = wand.area.corner2;

                for(int x = end.x; x >= start.x; x--)
                    for(int y = end.y; y >= start.y; y--)
                        for(int z = end.z; z >= start.z; z--)
                        {
                            Vector3Int newPos = new Vector3Int(x, y, z);
                            if(!World.TryGetTypeAt(newPos, out ushort actualType) || actualType == oldBlock)
                                AdvancedWand.AddAction(newPos, newBlock);
                        }

                Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>{0} -> {1}</color>", args[1], args[2]));
            }

            return true;
        }
    }
}
