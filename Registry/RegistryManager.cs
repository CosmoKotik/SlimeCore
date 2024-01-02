using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlimeCore.Entity;
using SlimeCore.Enums;

namespace SlimeCore.Registry
{
    public class RegistryManager
    {
        public List<BlockRegistry> BlockToItemMap { get; set; }

        public RegistryManager() 
        {
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
                        /*if (!isDefault)
                            continue;*/

                        if (properties == null)
                            properties = state;

                        //Console.WriteLine(properties.Value<string>("facing"));

                        BlockRegistry block = new BlockRegistry()
                        { 
                            ID = int.Parse(obj.ElementAt(i).First["states"].ElementAt(s)["id"].ToString()),
                            Name = obj.ElementAt(i).Path,
                            IsDefault = isDefault,
                            Direction = ParseDirection(properties.Value<string>("facing")),
                            Half = ParseDirection(properties.Value<string>("half")),
                            waterlogged = properties.Value<bool>("waterlogged")
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

        public int GetBlockId(int itemID)
        { 
            return BlockToItemMap.Find(x => x.ItemID == itemID).ID;
        }

        public int GetBlockId(int itemID, Direction direction)
        {
            return BlockToItemMap.Find(x => x.ItemID == itemID && (x.Direction.Equals(direction) || (x.Direction.Equals(Direction.None) && x.IsDefault))).ID;
        }
    }
}
