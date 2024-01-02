using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlimeCore.Entity;

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
                        bool isDefault = (bool)obj.ElementAt(i).First["states"].ElementAt(s).Value<bool>("default");
                        if (!isDefault)
                            continue;

                        BlockRegistry block = new BlockRegistry()
                        { 
                            ID = int.Parse(obj.ElementAt(i).First["states"].ElementAt(s)["id"].ToString()),
                            Name = obj.ElementAt(i).Path,
                            IsDefault = isDefault
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
                        {
                            BlockToItemMap.FindAll(x => x.Name.Equals(name)).ForEach(x => x.ItemID = id);
                        }

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
    }
}
