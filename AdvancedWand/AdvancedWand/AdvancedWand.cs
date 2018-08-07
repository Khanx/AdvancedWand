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

        public bool active = false;
        public int limit = 100000;
        public SelectedArea area = new SelectedArea();
        public Blueprint copy;

        private AdvancedWand() { }

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

        private static List<TupleStruct<Vector3Int, List<TupleStruct<Vector3Int, ushort>>>> actions = new List<TupleStruct<Vector3Int, List<TupleStruct<Vector3Int, ushort>>>>();
        private static long nextUpdate = 0;
        private static long increment = 750;

        public static void AddAction(Vector3Int position, ushort type)
        {
            Vector3Int chunk = position.ToChunk();

            bool found = false;
            for(int i = 0; i < actions.Count; i++)
            {
                if(actions[i].item1 == chunk)
                {
                    var action = actions[i];
                    actions.RemoveAt(i);

                    action.item2.Add(new TupleStruct<Vector3Int, ushort>(position, type));
                    actions.Insert(0, action);

                    found = true;
                    break;
                }
            }

            if(!found)
            {
                TupleStruct<Vector3Int, List<TupleStruct<Vector3Int, ushort>>> tuple = new TupleStruct<Vector3Int, List<TupleStruct<Vector3Int, ushort>>>(chunk, new List<TupleStruct<Vector3Int, ushort>>());

                tuple.item2.Add(new TupleStruct<Vector3Int, ushort>(position, type));

                actions.Insert(0, tuple);
            }
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnLateUpdate, "Khanx.AdvancedWand.OnUpdateAction")]
        public static void OnUpdate()
        {
            if(Time.MillisecondsSinceStart < nextUpdate)
                return;

            nextUpdate = Time.MillisecondsSinceStart + increment;

            if(actions.Count <= 0)
                return;

            var chunkChanges = actions[0];
            actions.RemoveAt(0);

            var chunk = World.GetChunk(chunkChanges.item1);

            if(chunk == null || chunk.DataState != Chunk.ChunkDataState.DataFull)   //chunk NOT loaded || Not posible to modify
            {
                actions.Add(chunkChanges);  //Added again to the list
                return;
            }

            foreach(var change in chunkChanges.item2)
            {
                World.TryGetTypeAt(change.item1, out ushort type);

                if(type == change.item2)
                    continue;

                World.TrySetTypeAt(change.item1, change.item2);

                ItemTypesServer.OnChange(change.item1, type, change.item2, null);

                ServerManager.SendBlockChange(change.item1, change.item2);
            }
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

                    showWhileHoldingTypes.Add(BlockTypes.Builtin.BuiltinBlocks.BronzeAxe);
                    list.Add(new AreaJobTracker.AreaHighlight(area.corner1, area.corner2, Shared.EAreaMeshType.AutoSelect, Shared.EAreaType.BuilderArea));
                }
            }
        }
    }
}
