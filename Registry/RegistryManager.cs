using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlimeCore.Core;
using SlimeCore.Entities;
using SlimeCore.Enums;
using SlimeCore.Registry.Enums;

namespace SlimeCore.Registry
{
    public class RegistryManager
    {
        public List<BlockRegistry> BlockToItemMap { get; set; }

        private ServerManager _serverManager;

        public RegistryManager(ServerManager sm) 
        {
            this._serverManager = sm;
            this.BlockToItemMap = new List<BlockRegistry>();
        }

        public object ParseBlocks(string registryPath)
        {
            using (StreamReader r = new StreamReader(registryPath))
            {
                string json = r.ReadToEnd();
                JToken obj = JToken.Parse(json);

                //Console.WriteLine(obj.ElementAt(0).First["states"].ElementAt(9).Value<bool>("default"));

                for (int i = 0; i < obj.Count(); i++)
                {
                    for (int s = 0; s < obj.ElementAt(i).First["states"].Count(); s++)
                    {
                        JToken state = obj.ElementAt(i).First["states"].ElementAt(s);
                        JToken properties = state["properties"];
                        bool isDefault = state.Value<bool>("default");
                        string name = obj.ElementAt(i).Path;

                        /*if (!isDefault)
                            continue;*/

                        if (properties == null)
                            properties = state;

                        //Console.WriteLine(properties.Value<string>("facing"));

                        BlockRegistry block = new BlockRegistry()
                        {
                            ID = int.Parse(obj.ElementAt(i).First["states"].ElementAt(s)["id"].ToString()),
                            Name = name,
                            IsDefault = isDefault,
                            Direction = ParseDirection(properties.Value<string>("facing")),
                            Half = ParseDirection(properties.Value<string>("half")),
                            waterlogged = properties.Value<bool>("waterlogged"),
                            Shape = ParseShape(properties.Value<string>("shape")),
                            type = ParseBlockType(name)
                        };

                        this.BlockToItemMap.Add(block);

                        /*Console.Write(obj.ElementAt(i).Path);
                        Console.Write("    id: ");
                        Console.WriteLine(obj.ElementAt(i).First["states"].ElementAt(s)["id"]);*/
                    }
                }

                return obj;
            }
        }

        private Direction ParseDirection(string direction)
        {
            switch (direction)
            {
                case "top":
                    return Direction.Top;
                case "bottom":
                    return Direction.Bottom;
                case "north":
                    return Direction.North;
                case "south":
                    return Direction.South;
                case "east":
                    return Direction.East;
                case "west":
                    return Direction.West;
                default: return Direction.None;
            }
        }

        private ShapeType ParseShape(string shape)
        {
            switch (shape)
            {
                case "straight":
                    return ShapeType.straight;
                case "inner_right":
                    return ShapeType.inner_right;
                case "inner_left":
                    return ShapeType.inner_left;
                case "outer_right":
                    return ShapeType.outer_right;
                case "outer_left":
                    return ShapeType.outer_left;
                default: return ShapeType.none;
            }
        }

        private BlockType ParseBlockType(string name)
        {
            if (name.Contains("stair"))
                return BlockType.Stair;
            else if (name.Contains("slab"))
                return BlockType.Slab;
            else if (name.Contains("button"))
                return BlockType.Button;
            else if (name.Contains("door"))
                return BlockType.Door;
            else if (name.Contains("fence"))
                return BlockType.Fence;
            else if (name.Contains("pressure_plate"))
                return BlockType.Pressure_Plate;
            else if (name.Contains("sapling"))
                return BlockType.Sapling;
            else if (name.Contains("sign"))
                return BlockType.Sign;
            else if (name.Contains("trapdoor"))
                return BlockType.Trapdoor;
            else
                return BlockType.Block;
        }

        public object ParseItems(string registryPath)
        {
            using (StreamReader r = new StreamReader(registryPath))
            {
                string json = r.ReadToEnd();
                JToken obj = JToken.Parse(json);

                Console.WriteLine(obj.ElementAt(0).First["entries"].ElementAt(0).Path.Split(".")[2]);

                for (int i = 0; i < obj.Count(); i++)
                {
                    for (int s = 0; s < obj.ElementAt(i).First["entries"].Count(); s++)
                    {
                        string type = obj.ElementAt(i).Path;
                        string name = obj.ElementAt(i).First["entries"].ElementAt(s).Path.Split(".")[2];
                        int id = obj.ElementAt(i).First["entries"].ElementAt(s).First.Value<int>("protocol_id");

                        //Console.WriteLine($"{name}    id: {id}");

                        ItemRegistry item = new ItemRegistry()
                        { 
                            TypeName = type,
                            ItemName = name,
                            ID = id
                        };

                        if (type.Equals("minecraft:item"))
                            BlockToItemMap.FindAll(x => x.Name.Equals(name)).ForEach(x => x.ItemID = id);

                        /*Console.Write(obj.ElementAt(i).Path);
                        Console.Write("    id: ");
                        Console.WriteLine(obj.ElementAt(i).First["states"].ElementAt(s)["id"]);*/
                    }
                }

                return obj;
            }
        }

        private int GetBlockIDFromPos(Position position)
        {
            Block block = _serverManager.BlockPlaced.Find(x => x.BlockPosition.Equals(position));
            return block.BlockID;
        }

        private Block GetBlockFromPos(Position position)
        {
            Block block = _serverManager.BlockPlaced.Find(x => x.BlockPosition.Equals(position));
            if (block != null)
                return block;

            return new Block(position, 0, BlockType.Air, Direction.None);
        }

        public int GetBlockId(int itemID)
        { 
            return BlockToItemMap.Find(x => x.ItemID == itemID && x.IsDefault && !x.waterlogged).ID;
        }

        public int GetBlockId(int itemID, Direction direction)
        {
            return BlockToItemMap.Find(x => x.ItemID == itemID && (x.Direction.Equals(direction) || (x.Direction.Equals(Direction.None) && x.IsDefault)) && !x.waterlogged).ID;
        }

        public int GetBlockId(int itemID, Direction direction, Direction half)
        {
            return BlockToItemMap.Find(x => x.ItemID == itemID && (x.Direction.Equals(direction) || (x.Direction.Equals(Direction.None) && x.IsDefault)) && (x.Half.Equals(half) || (x.Half.Equals(Direction.None) && x.IsDefault)) && !x.waterlogged).ID;
        }

        public BlockType GetBlockType(int itemID) 
        {
            return BlockToItemMap.Find(x => x.ItemID == itemID && x.IsDefault).type;
        }

        public int GetBlockId(int itemID, BlockProperties properties)
        {
            List<BlockRegistry> blocks = BlockToItemMap.FindAll(x => x.ItemID == itemID);

            Block[] blockMap = new Block[]
                {
                    GetBlockFromPos(properties.Position + new Position(0, 1)),      //fwd
                    GetBlockFromPos(properties.Position + new Position(1, 0)),      //left
                    GetBlockFromPos(properties.Position - new Position(1, 0)),      //right
                    GetBlockFromPos(properties.Position - new Position(0, 1))       //back
                };

            bool xInverse = false;
            bool zInverse = false;

            /*Direction fwd = Direction.None;
            Direction left = Direction.None;
            Direction right = Direction.None;
            Direction bkwd = Direction.None;*/

            int fwd = 0;
            int left = 1;
            int right = 2;
            int bkwd = 3;

            if (properties.Direction.Equals(Direction.North))
            {
                //x, z inverted
                blockMap[0].Direction = InverseDirection(blockMap[0].Direction);
                blockMap[1].Direction = InverseDirection(blockMap[1].Direction);
                blockMap[2].Direction = InverseDirection(blockMap[2].Direction);
                blockMap[3].Direction = InverseDirection(blockMap[3].Direction);
                fwd = 3;
                left = 2;
                right = 1;
                bkwd = 0;
            }
            else if (properties.Direction.Equals(Direction.West))
            {
                blockMap[1].Direction = InverseDirection(blockMap[1].Direction);
                blockMap[2].Direction = InverseDirection(blockMap[2].Direction);
                //x inverted
                fwd = 2;
                left = 0;
                right = 3;
                bkwd = 1;
            }
            else if (properties.Direction.Equals(Direction.East))
            {
                //z inverted
                blockMap[0].Direction = InverseDirection(blockMap[0].Direction);
                blockMap[3].Direction = InverseDirection(blockMap[3].Direction);
                fwd = 1;
                left = 3;
                right = 0;
                bkwd = 2;
            }

            /*if (properties.Direction.Equals(Direction.South))
            {
                blockMap[0] = GetBlockFromPos(properties.Position + new Position(0, 1));
                blockMap[1] = GetBlockFromPos(properties.Position + new Position(1, 0));
                blockMap[2] = GetBlockFromPos(properties.Position - new Position(1, 0));
                blockMap[3] = GetBlockFromPos(properties.Position - new Position(0, 1));
            }
            else if (properties.Direction.Equals(Direction.North))
            {
                zInverse = true;
                blockMap[0] = GetBlockFromPos(properties.Position - new Position(0, 1));
                blockMap[1] = GetBlockFromPos(properties.Position - new Position(1, 0));
                blockMap[2] = GetBlockFromPos(properties.Position + new Position(1, 0));
                blockMap[3] = GetBlockFromPos(properties.Position + new Position(0, 1));
            }
            else if (properties.Direction.Equals(Direction.East))
            {
                blockMap[0] = GetBlockFromPos(properties.Position + new Position(1, 0));
                blockMap[1] = GetBlockFromPos(properties.Position - new Position(0, 1));
                blockMap[2] = GetBlockFromPos(properties.Position + new Position(0, 1));
                blockMap[3] = GetBlockFromPos(properties.Position - new Position(1, 0));
            }
            else if (properties.Direction.Equals(Direction.West))
            {
                xInverse = true;
                blockMap[0] = GetBlockFromPos(properties.Position - new Position(1, 0));
                blockMap[1] = GetBlockFromPos(properties.Position + new Position(0, 1));
                blockMap[2] = GetBlockFromPos(properties.Position - new Position(0, 1));
                blockMap[3] = GetBlockFromPos(properties.Position + new Position(1, 0));
            }*/

            for (int i = 0; i < blocks.Count; i++)
            {
                BlockRegistry block = blocks[i];

                switch (block.type)
                { 
                    case BlockType.Stair:
                        ShapeType shapeType = ShapeType.straight;
                        Direction dir = Direction.South;
                        //if (blockMap.ToList().FindAll(x => x.Type.Equals(BlockType.Stair)).Count > 0)
                        if (blockMap.Count(x => x.Type.Equals(BlockType.Stair)) > 1)
                        {
                            bool hasFwd = blockMap[0].Type.Equals(BlockType.Stair);
                            Direction fwdDir = blockMap[fwd].Direction;
                            bool hasLeft = blockMap[1].Type.Equals(BlockType.Stair);
                            Direction leftDir = blockMap[left].Direction;
                            bool hasRight = blockMap[2].Type.Equals(BlockType.Stair);
                            Direction rightDir = blockMap[right].Direction;
                            bool hasBkwd = blockMap[3].Type.Equals(BlockType.Stair);
                            Direction bkwdDir = blockMap[bkwd].Direction;

                            /*if (fwdDir.Equals(Direction.West) && rightDir.Equals(Direction.South))
                                shapeType = ShapeType.outer_right;
                            else if (fwdDir.Equals(Direction.East) && leftDir.Equals(Direction.South))
                                shapeType = ShapeType.outer_left;
                            else if (fwdDir.Equals(Direction.East) && leftDir.Equals(Direction.North))
                                shapeType = ShapeType.inner_right;
                            else if (fwdDir.Equals(Direction.East) && rightDir.Equals(Direction.North))
                                shapeType = ShapeType.outer_right;
                            else if (fwdDir.Equals(Direction.West) && rightDir.Equals(Direction.North))
                                shapeType = ShapeType.inner_right;
                            else if (fwdDir.Equals(Direction.West) && leftDir.Equals(Direction.North))
                                shapeType = ShapeType.outer_left;
                            else if (bkwdDir.Equals(Direction.East) && rightDir.Equals(Direction.South))
                                shapeType = ShapeType.inner_right;
                            else if (bkwdDir.Equals(Direction.West) && leftDir.Equals(Direction.South))
                                shapeType = ShapeType.inner_left;
                            else if (bkwdDir.Equals(Direction.West) && rightDir.Equals(Direction.North))
                                shapeType = ShapeType.outer_left;
                            else if (bkwdDir.Equals(Direction.East) && leftDir.Equals(Direction.North))
                                shapeType = ShapeType.outer_right;*/

                            shapeType = FindShapeType(new ShapeDirections(fwdDir, leftDir, rightDir, bkwdDir));
                        }
                        //
                        if ((block.Direction.Equals(properties.Direction) || block.Direction.Equals(Direction.None)) && (block.Half.Equals(properties.Half) || block.Half.Equals(Direction.None)) && block.Shape.Equals(shapeType) && !block.waterlogged)
                            return block.ID;
                        break;
                    default:
                        if ((block.Direction.Equals(properties.Direction) || block.Direction.Equals(Direction.None)) && (block.Half.Equals(properties.Half) || block.Half.Equals(Direction.None)) && block.IsDefault && !block.waterlogged)
                            return block.ID;
                        break;
                }
            }
            //
            return 0;
            //return BlockToItemMap.Find(x => x.ItemID == itemID && x.IsDefault && !x.waterlogged).ID;
        }

        private Direction InverseDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.East: return Direction.West;
                case Direction.North: return Direction.South;
                case Direction.West: return Direction.East;
                case Direction.South: return Direction.North;
                default: return direction;
            }
        }

        public record ShapeDirections(Direction fwd, Direction left, Direction right, Direction bkwd);
        public ShapeType FindShapeType(ShapeDirections directions) =>
            directions switch
            {
                { fwd: Direction.East, left: Direction.South } => ShapeType.outer_left,
                { fwd: Direction.West, right: Direction.South } => ShapeType.outer_right,
                { bkwd: Direction.East, left: Direction.North } => ShapeType.inner_left,
                { bkwd: Direction.West, left: Direction.South } => ShapeType.inner_right,
                { bkwd: Direction.West, right: Direction.North } => ShapeType.inner_right,
                { bkwd: Direction.East, right: Direction.South } => ShapeType.inner_left,
                _ => ShapeType.straight,
            };
    }
}
