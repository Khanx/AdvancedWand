using Pipliz;
using System.Collections.Generic;
using System.IO;

namespace AdvancedWand.Helper
{
    public class Blueprint
    {
        public int xSize { get; internal set; }
        public int ySize { get; internal set; }
        public int zSize { get; internal set; }
        public Dictionary<ushort, string> types = new Dictionary<ushort, string>();
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
                        {
                            if(!types.ContainsKey(type))
                                types.Add(type, ItemTypes.IndexLookup.GetName(type));

                            blocks[x - start.x, y - start.y, z - start.z] = type;
                        }
                    }
                }
            }

            playerMod = new Vector3Int(player.Position) - start;
        }

        public void Rotate()
        {
            ushort[,,] newBlocks = new ushort[zSize + 1, ySize + 1, xSize + 1];

            for(int y = 0; y < ySize; y++)
            {
                for(int x = 0; x < zSize; x++)
                {
                    for(int z = 0; z < xSize; z++)
                    {
                        int newX = z;
                        int newZ = zSize - ( x + 1 );

                        newBlocks[newZ, y, newX] = blocks[z, y, x];
                    }
                }
            }

            blocks = newBlocks;

            int tmpSize = xSize;
            xSize = zSize;
            zSize = tmpSize;
        }

        public Blueprint(byte[] binaryBlueprint)
        {
            using(ByteReader raw = ByteReader.Get(binaryBlueprint))
            {
                playerMod = raw.ReadVariableVector3Int();
                int typesC = raw.ReadVariableInt();

                xSize = raw.ReadVariableInt();
                ySize = raw.ReadVariableInt();
                zSize = raw.ReadVariableInt();

                //From one world to another
                Dictionary<ushort, ushort> typesTransformation = new Dictionary<ushort, ushort>();

                using(ByteReader compressed = raw.ReadCompressed())
                {
                    for(int i = 0; i < typesC; i++)
                    {
                        ushort type_index = compressed.ReadVariableUShort();
                        string type_name = compressed.ReadString();

                        ushort new_type_index;

                        if(!ItemTypes.IndexLookup.TryGetIndex(type_name, out new_type_index))
                            new_type_index = BlockTypes.BuiltinBlocks.Indices.air;

                        typesTransformation.Add(type_index, new_type_index);
                        types.Add(new_type_index, type_name);
                    }

                    blocks = new ushort[xSize, ySize, zSize];

                    for(int x = 0; x < xSize; x++)
                    {
                        for(int y = 0; y < ySize; y++)
                        {
                            for(int z = 0; z < zSize; z++)
                            {
                                blocks[x, y, z] = typesTransformation.GetValueOrDefault(compressed.ReadVariableUShort(), BlockTypes.BuiltinBlocks.Indices.air);
                            }
                        }
                    }

                }
            }
        }

        public void saveBlueprint(string filename)
        {
            using(ByteBuilder builder = ByteBuilder.Get())
            {
                builder.WriteVariable(playerMod);

                builder.WriteVariable(types.Count);

                builder.WriteVariable(xSize);
                builder.WriteVariable(ySize);
                builder.WriteVariable(zSize);

                using(ByteBuilder compressed = ByteBuilder.Get())
                {
                    foreach(var key in types.Keys)
                    {
                        compressed.WriteVariable(key);
                        compressed.Write(types[key]);
                    }

                    for(int x = 0; x < xSize; x++)
                    {
                        for(int y = 0; y < ySize; y++)
                        {
                            for(int z = 0; z < zSize; z++)
                            {
                                compressed.WriteVariable(blocks[x, y, z]);
                            }
                        }
                    }

                    builder.WriteCompressed(compressed);
                }

                //return builder.ToArray();
                File.WriteAllBytes(BlueprintManager.MODPATH + "/blueprints/" + filename + ".b", builder.ToArray());
            }
        }
    }
}
