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
            if(!AdvancedWandHelper.CheckCommand(player, arg, 3, out string[] args))
                return true;

            if(!AdvancedWandHelper.CheckLimit(player))
                return true;

            if(!AdvancedWandHelper.GetBlockIndex(player, args[1], out ushort oldBlock))
                return true;

            if(!AdvancedWandHelper.GetBlockIndex(player, args[2], out ushort newBlock))
                return true;


            AdvancedWandHelper.GenerateCorners(player, out Vector3Int start, out Vector3Int end);

            for(int x = start.x; x <= end.x; x++)
                for(int y = start.y; y <= end.y; y++)
                    for(int z = start.z; z <= end.z; z++)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        if(World.TryGetTypeAt(newPos, out ushort actualType) && actualType == oldBlock)
                            ServerManager.TryChangeBlock(newPos, newBlock);
                    }

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>{0} -> {1}</color>", args[1], args[2]));

            return true;
        }
    }
}
