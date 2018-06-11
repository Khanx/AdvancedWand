using Pipliz;

namespace AdvancedWand.Helper
{
    public class Blueprint
    {
        public int xSize { get; internal set; }
        public int ySize { get; internal set; }
        public int zSize { get; internal set; }
        public ushort[,,] blocks { get; internal set; }
        public Vector3Int playerMod { get; internal set; }

        public Blueprint(SelectedArea area, Players.Player player)
        {
            if(area == null)
                return;

            if(!area.IsPos1Initialized() || !area.IsPos2Initialized())
                return;

            xSize = area.GetXSize();
            ySize = area.GetYSize();
            zSize = area.GetZSize();

            blocks = new ushort[area.GetXSize() + 1, area.GetYSize() + 1, area.GetZSize() + 1];

            Vector3Int start = area.corner1;
            Vector3Int end = area.corner2;

            for(int x = start.x; x <= end.x; x++)
            {
                for(int y = start.y; y <= end.y; y++)
                {
                    for(int z = start.z; z <= end.z; z++)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        if(World.TryGetTypeAt(newPos, out ushort type) && ItemTypes.NotableTypes.Contains(ItemTypes.GetType(type)))
                            blocks[x - start.x, y - start.y, z - start.z] = type;
                    }
                }
            }

            playerMod = new Vector3Int(player.Position) - start;

            Pipliz.Chatting.Chat.Send(player, "<color=olive>Copied area:</color>");
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>X: {0}</color>", xSize));
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Y: {0}</color>", ySize));
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Z: {0}</color>", zSize));
            Pipliz.Chatting.Chat.Send(player, string.Format("<color=lime>Total: {0}</color>", xSize* ySize* zSize));
        }
    }
}
