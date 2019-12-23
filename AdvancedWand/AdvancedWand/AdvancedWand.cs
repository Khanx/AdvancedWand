using AdvancedWand.Helper;
using Pipliz;
using System.Collections.Generic;

namespace AdvancedWand
{
    [ModLoader.ModManager]
    public class AdvancedWand
    {
        private static readonly Dictionary<Players.Player, AdvancedWand> advancedWands = new Dictionary<Players.Player, AdvancedWand>();
        //It's a Dictionary because in a server can be more than one player

        public static int default_limit = 100000;

        public bool active = false;
        public int limit;
        public SelectedArea area = new SelectedArea();
        public Blueprint copy;

        private AdvancedWand() { limit = default_limit; }

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

        private static Dictionary<Vector3Int, List<(Vector3Int, ushort, BlockChangeRequestOrigin)>> chunkChanges = new Dictionary<Vector3Int, List<(Vector3Int, ushort, BlockChangeRequestOrigin)>>();
        private static Stack<Vector3Int> chunkOrder = new Stack<Vector3Int>();
        private static Queue<Vector3Int> failedChunks = new Queue<Vector3Int>();

        private static long nextUpdate = 0;
        public static long increment = 250;

        public static void AddAction(Vector3Int position, ushort type, Players.Player player)
        {
            Vector3Int chunk = position.ToChunk();

            if(chunkChanges.ContainsKey(chunk))
            {
                chunkChanges[chunk].Add((position, type, new BlockChangeRequestOrigin(player)));
            }
            else
            {
                List<(Vector3Int, ushort,  BlockChangeRequestOrigin)> changes = new List<(Vector3Int, ushort, BlockChangeRequestOrigin)>();
                changes.Add((position, type, new BlockChangeRequestOrigin(player)));
                chunkChanges.Add(chunk, changes);
            }

            if(!chunkOrder.Contains(chunk))
                chunkOrder.Push(chunk);
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnLateUpdate, "Khanx.AdvancedWand.OnUpdateAction")]
        public static void OnUpdate()
        {
            if(Time.MillisecondsSinceStart < nextUpdate)
                return;

            nextUpdate = Time.MillisecondsSinceStart + increment;

            Vector3Int chunk;

            if(chunkOrder.Count > 0)
            {
                chunk = chunkOrder.Pop();
            }
            else if(failedChunks.Count > 0)
            {
                chunk = failedChunks.Dequeue();
            }
            else
                return;

            if(!chunkChanges.TryGetValue(chunk, out List<(Vector3Int, ushort, BlockChangeRequestOrigin)> changes))
                return;


            foreach(var change in changes)
            {
                if (ItemTypes.GetType(change.Item2).Name.Contains("bedend"))
                    continue;

                if(ServerManager.TryChangeBlock(change.Item1, change.Item2, change.Item3) == EServerChangeBlockResult.ChunkNotReady)
                {
                    failedChunks.Enqueue(chunk);
                    return;
                }
            }

            chunkChanges.Remove(chunk);
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnPlayerDisconnected, "Khanx.AdvancedWand.RemoveWandOnPlayerDisconnected")]
        public static void RemoveAdvancedWand(Players.Player player)
        {
            if(player != null)
                advancedWands.Remove(player);
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnSendAreaHighlights, "Khanx.AdvancedWand.ShowArea")]
        public static void OnSendAreaHighlights(Players.Player player, List<AreaJobTracker.AreaHighlight> list, List<ushort> showWhileHoldingTypes)
        {
            if(null != player)
            {
                if(advancedWands.ContainsKey(player))
                {
                    if(!advancedWands[player].active)
                        return;

                    SelectedArea area = advancedWands[player].area;

                    if(area.pos1 == Vector3Int.maximum || area.pos2 == Vector3Int.maximum)
                        return;

                    showWhileHoldingTypes.Add(BlockTypes.BuiltinBlocks.Indices.bronzeaxe);
                    list.Add(new AreaJobTracker.AreaHighlight(area.corner1, area.corner2, Shared.EAreaMeshType.AutoSelect, Shared.EServerAreaType.ConstructionArea));
                }
            }
        }
    }
}
