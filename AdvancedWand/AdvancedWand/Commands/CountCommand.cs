using AdvancedWand.Helper;
using ExtendedAPI.Commands;
using Pipliz;

namespace AdvancedWand.Commands
{
    [AutoLoadCommand]
    public class CountCommand : BaseCommand
    {
        public CountCommand()
        {
            startWith.Add("//count");
        }

        public override bool TryDoCommand(Players.Player player, string arg)
        {
            if(!CommandHelper.CheckCommand(player))
                return true;

            string[] args = ChatCommands.CommandManager.SplitCommand(arg);

            if(2 != args.Length)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Wrong Arguments</color>");
                return true;
            }

            AdvancedWand wand = AdvancedWand.GetAdvancedWand(player);

            Vector3Int start = wand.area.corner1;
            Vector3Int end = wand.area.corner2;

            ushort blockIndex = 0;

            try
            {
                bool isNumeric = int.TryParse(args[1], out int IDBlock);

                if(isNumeric)
                    blockIndex = (ushort)IDBlock;
                else
                    blockIndex = ItemTypes.IndexLookup.GetIndex(args[1]);
            }
            catch(System.ArgumentException)
            {
                Pipliz.Chatting.Chat.Send(player, "<color=red>Block not found</color>");
                return true;
            }

            int count = 0;

            for(int x = start.x; x <= end.x; x++)
                for(int y = start.y; y <= end.y; y++)
                    for(int z = start.z; z <= end.z; z++)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        if(World.TryGetTypeAt(newPos, out ushort actualType))
                        {
                            if(actualType == blockIndex)
                                count++;
                            else
                            {
                                ItemTypes.ItemType type = ItemTypes.GetType(actualType);

                                if(!ItemTypes.NotableTypes.Contains(type) && type.ParentItemType != null && type.ParentItemType.ItemIndex == blockIndex)
                                    count++;
                            }
                        }
                    }

            //Las camas son de dos bloques
            if(blockIndex == ItemTypes.IndexLookup.GetIndex("bed"))
                count /= 2;

            Pipliz.Chatting.Chat.Send(player, string.Format("<color=green>{0}: {1}</color>", args[1], count));

            return true;
        }
    }
}
