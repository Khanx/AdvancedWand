using fNbt;
using Newtonsoft.Json;
using Pipliz;
using Pipliz.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdvancedWand.Helper;

namespace Pandaros.SchematicBuilder.NBT
{
    [ModLoader.ModManager]
    public static class SchematicReader
    {
        private const string METADATA_FILEEXT = ".metadata.json";

        public static bool TryGetSchematicMetadata(string name, out SchematicMetadata metadata)
        {
            try
            {
                if (File.Exists(GameLoader.Schematic_FOLDER + name + METADATA_FILEEXT))
                {
                    var json = JSON.Deserialize(GameLoader.Schematic_FOLDER + name + METADATA_FILEEXT);
                    metadata = JsonConvert.DeserializeObject<SchematicMetadata>(json.ToString());
                }
                else
                {
                    metadata = GenerateMetaData(name);
                }
            }
            catch (Exception ex)
            {
                metadata = null;
                Log.Write(ex + "error getting metadata for schematic {0}", name);
            }

            return metadata != null;
        }

        public static bool TryGetSchematic(string name, Vector3Int location, out Schematic schematic)
        {
            if (Directory.Exists(GameLoader.Schematic_FOLDER))
                schematic = LoadSchematic(GameLoader.Schematic_FOLDER + name + ".csschematic", location);
            else
                schematic = null;

            return schematic != null;
        }

        public static bool TryGetSchematicSize(string name, out RawSchematicSize size)
        {
            if (Directory.Exists(GameLoader.Schematic_FOLDER))
                size = LoadRawSize(new NbtFile(GameLoader.Schematic_FOLDER + name + ".csschematic"));
            else
                size = null;

            return size != null;
        }

        public static List<FileInfo> GetSchematics()
        {
            var options = new List<FileInfo>();
            var folderSchematics = GameLoader.Schematic_FOLDER;

            if (!Directory.Exists(folderSchematics))
                Directory.CreateDirectory(folderSchematics);

            foreach (var file in Directory.EnumerateFiles(folderSchematics, "*.csschematic"))
                options.Add(new FileInfo(file));

            return options.OrderBy(f => f.Name).ToList();
        }

        public static void SaveSchematic(Schematic schematic)
        {
            List<NbtTag> tags = new List<NbtTag>();

            tags.Add(new NbtInt("Width", schematic.XMax));
            tags.Add(new NbtInt("Height", schematic.YMax));
            tags.Add(new NbtInt("Length", schematic.ZMax));

            List<NbtTag> blocks = new List<NbtTag>();

            for (int Y = 0; Y < schematic.YMax; Y++)
            {
                for (int Z = 0; Z < schematic.ZMax; Z++)
                {
                    for (int X = 0; X < schematic.XMax; X++)
                    {
                        NbtCompound compTag = new NbtCompound();
                        compTag.Add(new NbtInt("x", X));
                        compTag.Add(new NbtInt("y", Y));
                        compTag.Add(new NbtInt("z", Z));
                        compTag.Add(new NbtString("id", schematic.Blocks[X,Y,Z].BlockID));
                        blocks.Add(compTag);
                    }
                }
            }

            NbtList nbtList = new NbtList("CSBlocks", blocks);
            tags.Add(nbtList);

            NbtFile nbtFile = new NbtFile(new NbtCompound("CompoundTag", tags));
            var fileSave = Path.Combine(GameLoader.Schematic_FOLDER, schematic.Name + ".csschematic");
            var metaDataSave = Path.Combine(GameLoader.Schematic_FOLDER, schematic.Name + ".csschematic.metadata.json");

            if (File.Exists(fileSave))
                File.Delete(fileSave);

            if (File.Exists(metaDataSave))
                File.Delete(metaDataSave);

            GenerateMetaData(metaDataSave, schematic.Name, schematic);
            nbtFile.SaveToFile(fileSave, NbtCompression.GZip);
        }

        public static Schematic LoadSchematic(string path, Vector3Int startPos)
        {
            NbtFile file = new NbtFile(path);
            return LoadSchematic(file, startPos);
        }

        public static SchematicMetadata GenerateMetaData(string name)
        {
                if (!Directory.Exists(GameLoader.Schematic_FOLDER))
                    Directory.CreateDirectory(GameLoader.Schematic_FOLDER);

                var metadataPath = Path.Combine(GameLoader.Schematic_FOLDER, name + METADATA_FILEEXT);
                Schematic schematic = LoadSchematic(new NbtFile(GameLoader.Schematic_FOLDER + name), Vector3Int.invalidPos);


                return GenerateMetaData(metadataPath, name.Substring(0, name.LastIndexOf('.')), schematic);
        }

        private static SchematicMetadata GenerateMetaData(string metadataPath, string name, Schematic schematic)
        {
            var metadata = new SchematicMetadata();
            metadata.Name = name;

            for (int Y = 0; Y <= schematic.YMax; Y++)
            {
                for (int Z = 0; Z <= schematic.ZMax; Z++)
                {
                    for (int X = 0; X <= schematic.XMax; X++)
                    {
                        if (schematic.Blocks.GetLength(0) > X &&
                            schematic.Blocks.GetLength(1) > Y &&
                            schematic.Blocks.GetLength(2) > Z &&
                            schematic.Blocks[X, Y, Z] != null)
                        {
                            var block = ItemTypes.IndexLookup.GetIndex(schematic.Blocks[X, Y, Z].BlockID);

                            if (block != BlockTypes.BuiltinBlocks.Indices.air)
                            {
                                var buildType = ItemTypes.GetType(block);

                                if (!buildType.Name.Contains("bedend"))
                                {
                                    var index = block;

                                    if (!string.IsNullOrWhiteSpace(buildType.ParentType) && !buildType.Name.Contains("grass") && !buildType.Name.Contains("leaves"))
                                        index = ItemTypes.GetType(buildType.ParentType).ItemIndex;

                                    if (metadata.Blocks.TryGetValue(index, out var blockMeta))
                                    {
                                        blockMeta.Count++;
                                    }
                                    else
                                    {
                                        blockMeta = new SchematicBlockMetadata();
                                        blockMeta.Count++;
                                        blockMeta.ItemId = index;

                                        metadata.Blocks.Add(blockMeta.ItemId, blockMeta);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            metadata.MaxX = schematic.XMax;
            metadata.MaxY = schematic.YMax;
            metadata.MaxZ = schematic.ZMax;

            JSON.Serialize(metadataPath, metadata.JsonSerialize());

            return metadata;
        }

        public static JSONNode JsonSerialize<T>(this T obj)
        {
            var objStr = JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            var json = JSON.DeserializeString(objStr);
            return json;
        }

        public static Schematic LoadSchematic(NbtFile nbtFile, Vector3Int startPos)
        {
            RawSchematic raw = LoadRaw(nbtFile);
            SchematicBlock[,,] blocks = default(SchematicBlock[,,]);

            if (raw.CSBlocks != null && raw.CSBlocks.Length > 0)
                blocks = raw.CSBlocks;

            string name = Path.GetFileNameWithoutExtension(nbtFile.FileName);
            Schematic schematic = new Schematic(name, raw.XMax, raw.YMax, raw.ZMax, blocks, startPos);

            return schematic;
        }

        public static RawSchematicSize LoadRawSize(NbtFile nbtFile)
        {
            RawSchematicSize raw = new RawSchematicSize();
            var rootTag = nbtFile.RootTag;

            foreach (NbtTag tag in rootTag.Tags)
            {
                switch (tag.Name)
                {
                    case "Width": //Short
                        raw.XMax = tag.IntValue;
                        break;
                    case "Height": //Short
                        raw.YMax = tag.IntValue;
                        break;
                    case "Length": //Short
                        raw.ZMax = tag.IntValue;
                        break;
                    default:
                        break;
                }
            }
            return raw;
        }

        public static RawSchematic LoadRaw(NbtFile nbtFile)
        {
            RawSchematic raw = new RawSchematic();
            var rootTag = nbtFile.RootTag;

            foreach (NbtTag tag in rootTag.Tags)
            {
                switch (tag.Name)
                {
                    case "Width": //Short
                        raw.XMax = tag.IntValue + 1;
                        break;
                    case "Height": //Short
                        raw.YMax = tag.IntValue + 1;
                        break;
                    case "Length": //Short
                        raw.ZMax = tag.IntValue + 1;
                        break;
                    case "Materials": //String
                        raw.Materials = tag.StringValue;
                        break;
                    case "Blocks": //ByteArray
                        raw.Blocks = tag.ByteArrayValue;
                        break;
                    case "Data": //ByteArray
                        raw.Data = tag.ByteArrayValue;
                        break;
                    case "Entities": //List
                        break; //Ignore
                    case "Icon": //Compound
                        break; //Ignore
                    case "CSBlocks":
                        raw.CSBlocks = GetCSBlocks(raw, tag, new SchematicBlock[raw.XMax + 1, raw.YMax + 1, raw.ZMax + 1]);
                        break;
                    case "SchematicaMapping": //Compound
                        tag.ToString();
                        break; //Ignore
                    default:
                        break;
                }
            }
            return raw;
        }

        public static SchematicBlock[,,] GetCSBlocks(RawSchematic raw, NbtTag csBlockTag, SchematicBlock[,,] list)
        {
            NbtList csBlocks = csBlockTag as NbtList;

            if (csBlocks != null)
            {
                foreach (NbtCompound compTag in csBlocks)
                {
                    NbtTag xTag = compTag["x"];
                    NbtTag yTag = compTag["y"];
                    NbtTag zTag = compTag["z"];
                    NbtTag idTag = compTag["id"];
                    SchematicBlock block = new SchematicBlock()
                    {
                        X = xTag.IntValue,
                        Y = yTag.IntValue,
                        Z = zTag.IntValue,
                        BlockID = idTag.StringValue
                    };

                    if (string.IsNullOrEmpty(block.BlockID))
                        block.BlockID = "0";

                    list[xTag.IntValue, yTag.IntValue, zTag.IntValue] = block;
                }
            }

            for (int Y = 0; Y <= raw.YMax; Y++)
            {
                for (int Z = 0; Z <= raw.ZMax; Z++)
                {
                    for (int X = 0; X <= raw.XMax; X++)
                    {
                        if (list[X, Y, Z] == null)
                            list[X, Y, Z] = new SchematicBlock()
                            {
                                X = X,
                                Y = Y,
                                Z = Z,
                                BlockID = BlockTypes.BuiltinBlocks.Types.air.Name
                            };
                    }
                }
            }

            return list;
        }
    }
}
