using Pipliz;

namespace AdvancedWand.Helper
{
    public class SelectedArea
    {
        public Vector3Int pos1 = Vector3Int.maximum;
        public Vector3Int pos2 = Vector3Int.maximum;
        public Vector3Int corner1 { get; internal set; }
        public Vector3Int corner2 { get; internal set; }

        public bool IsPos1Initialized() { return pos1 != Vector3Int.maximum; }
        public bool IsPos2Initialized() { return pos2 != Vector3Int.maximum; }

        public void SetCorner1(Vector3Int newPos, Players.Player player)
        {
            pos1 = newPos;
            corner1 = Vector3Int.Min(pos1, pos2);
            corner2 = Vector3Int.Max(pos1, pos2);

            AreaJobTracker.SendData(player);
        }

        public void SetCorner2(Vector3Int newPos, Players.Player player)
        {
            pos2 = newPos;
            corner1 = Vector3Int.Min(pos1, pos2);
            corner2 = Vector3Int.Max(pos1, pos2);

            AreaJobTracker.SendData(player);
        }

        public int GetXSize() { return Math.Abs(pos1.x - pos2.x) + 1; }
        public int GetYSize() { return Math.Abs(pos1.y - pos2.y) + 1; }
        public int GetZSize() { return Math.Abs(pos1.z - pos2.z) + 1; }
        public int GetSize() { return GetXSize() * GetYSize() * GetZSize(); }
    }
}
