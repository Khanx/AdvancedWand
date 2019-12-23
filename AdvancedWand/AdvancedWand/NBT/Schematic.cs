using Pipliz;

namespace Pandaros.SchematicBuilder.NBT
{
    public class Schematic
    {
        public enum Rotation
        {
            Front,
            Right,
            Back,
            Left
        }

        public string Name { get; set; }
        public int XMax { get; set; }
        public int YMax { get; set; }
        public int ZMax { get; set; }
        /// <summary>Contains all usual blocks</summary>
        public SchematicBlock[,,] Blocks { get; set; }
        /// <summary>Contains TileEntities such as hoppers and chests</summary>
        public Vector3Int StartPos { get; set; }

        public Schematic()
        {
   
        }

        public Schematic(string name) : this()
        {
            Name = name;
        }

        public Schematic(string name, int x, int y, int z) : this(name)
        {
            XMax = x;
            YMax = y;
            ZMax = z;
        }

        public Schematic(string name, int x, int y, int z, Vector3Int corner1) : this(name, x, y, z)
        {
            Blocks = new SchematicBlock[x+1,y+1,z+1];

            for (int Y = 0; Y <= YMax; Y++)
            {
                for (int Z = 0; Z <= ZMax; Z++)
                {
                    for (int X = 0; X <= XMax; X++)
                    {
                        string blockid;
                        if (World.TryGetTypeAt(corner1 + new Vector3Int(X, Y, Z), out ItemTypes.ItemType type))
                            blockid = type.Name;
                        else
                            blockid = "air";

                        Blocks[X, Y, Z] = new SchematicBlock()
                        {
                            X = X,
                            Y = Y,
                            Z = Z,
                            BlockID = blockid
                        };
                    }
                }
            }
        }

        public Schematic(string name, int x, int y, int z, SchematicBlock[,,] blocks, Vector3Int startPos) : this(name, x, y, z)
        {
            Blocks = blocks;
            StartPos = startPos;
        }

        public SchematicBlock GetBlock(int X, int Y, int Z)
        {
            SchematicBlock block = default(SchematicBlock);

            if (Y < YMax &&
                X < XMax &&
                Z < ZMax)
                block = Blocks[X, Y, Z];

            if (block == default(SchematicBlock))
                block = SchematicBlock.Air;

            return block;
        }

        public void Rotate()
        {
            SchematicBlock[,,] newBlocks = new SchematicBlock[ZMax+1, YMax+1, XMax+1];

            for (int y = 0; y <= YMax; y++)
            {
                for (int z = 0; z <= ZMax; z++)
                {
                    for (int x = 0; x <= XMax; x++)
                    {
                        int newX = x;
                        int newZ = (ZMax+1) - (z + 1);

                        if (Blocks[x, y, z].BlockID.Contains("z+"))
                        {
                            Blocks[x, y, z].BlockID = Blocks[x, y, z].BlockID.Replace("z+", "x-");
                        }
                        else if (Blocks[x, y, z].BlockID.Contains("z-"))
                        {
                            Blocks[x, y, z].BlockID = Blocks[x, y, z].BlockID.Replace("z-", "x+");
                        }
                        else if (Blocks[x, y, z].BlockID.Contains("x+"))
                        {
                            Blocks[x, y, z].BlockID = Blocks[x, y, z].BlockID.Replace("x+", "z+");
                        }
                        else if (Blocks[x, y, z].BlockID.Contains("x-"))
                        {
                            Blocks[x, y, z].BlockID = Blocks[x, y, z].BlockID.Replace("x-", "z-");
                        }

                        newBlocks[newZ, y, newX] = Blocks[x, y, z];
                    }
                }
            }

            Blocks = newBlocks;

            int tmpSize = XMax;
            XMax = ZMax;
            ZMax = tmpSize;
        }

        public override string ToString()
        {
            return $"Name: {Name}  Max Bounds: [{XMax}, {YMax}, {ZMax}]";
        }
    }
}
