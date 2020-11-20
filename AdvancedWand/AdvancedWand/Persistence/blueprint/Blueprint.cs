﻿using Pipliz;
using System.Collections.Generic;
using BlockTypes;
using System.IO;

namespace AdvancedWand.Persistence
{
    public class Blueprint : Structure
    {
        private int xSize;
        private int ySize;
        private int zSize;
        private ushort[,,] blocks;
        public Vector3Int playerMod;

        public override int GetMaxX() { return xSize - 1; }

        public override int GetMaxY() { return ySize - 1; }

        public override int GetMaxZ() { return zSize - 1; }

        public override void Rotate()
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

                        string type = ItemTypes.IndexLookup.GetName(blocks[z, y, x]);
                        switch (type.Substring(type.Length - 2))
                        {
                            case "x+":
                                type = type.Replace("x+", "z+");
                                break;
                            case "x-":
                                type = type.Replace("x-", "z-");
                                break;
                            case "z+":
                                type = type.Replace("z+", "x-");
                                break;
                            case "z-":
                                type = type.Replace("z-", "x+");
                                break;
                            default:
                                break;
                        }

                        newBlocks[newZ, y, newX] = ItemTypes.GetType(type).ItemIndex;
                    }
                }
            }

            blocks = newBlocks;

            int tmpSize = xSize;
            xSize = zSize;
            zSize = tmpSize;
        }

        public override ushort GetBlock(int x, int y, int z)
        {
            return blocks[x,y,z];
        }

        public Blueprint(Helper.SelectedArea area, Players.Player player)
        {
            if (area == null)
                return;

            if (!area.IsPos1Initialized() || !area.IsPos2Initialized())
                return;

            xSize = area.GetXSize();
            ySize = area.GetYSize();
            zSize = area.GetZSize();

            blocks = new ushort[area.GetXSize() + 1, area.GetYSize() + 1, area.GetZSize() + 1];

            Vector3Int start = area.corner1;
            Vector3Int end = area.corner2;

            for (int x = start.x; x <= end.x; x++)
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    for (int z = start.z; z <= end.z; z++)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        if (World.TryGetTypeAt(newPos, out ushort type))
                        {
                            blocks[x - start.x, y - start.y, z - start.z] = type;
                        }
                    }
                }
            }

            playerMod = new Vector3Int(player.Position) - start;
        }

        public Blueprint(string file) : base(file)
        {
            byte[] binaryBlueprint = File.ReadAllBytes(file);

            using (ByteReader raw = ByteReader.Get(binaryBlueprint))
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
                            new_type_index = BuiltinBlocks.Indices.missingerror;

                        typesTransformation.Add(type_index, new_type_index);
                    } //type

                    blocks = new ushort[xSize, ySize, zSize];

                    for(int x = 0; x < xSize; x++)
                    {
                        for(int y = 0; y < ySize; y++)
                        {
                            for(int z = 0; z < zSize; z++)
                            {
                                blocks[x, y, z] = typesTransformation.GetValueOrDefault(compressed.ReadVariableUShort(), BuiltinBlocks.Indices.missingerror);
                            }
                        }
                    }

                } //ByteReader compressed 
            }
        }

        public override void Save(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}