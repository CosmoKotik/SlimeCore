using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeCore.Core.Metadata
{
    public class Metadata
    {
        public string Name { get; set; }
        public MetadataType MetaType { get; set; }
        public MetadataValue MetaValue { get; set; }

        public List<Metadata> Meta { get; set; }

        public Metadata()
        {
            Meta = new List<Metadata>();
        }

        public Metadata AddMetadata(string name, MetadataType type, MetadataValue value)
        {
            Metadata metadata = new Metadata()
            { 
                Name = name,
                MetaType = type,
                MetaValue = value
            };

            Meta.Add(metadata);

            return this;
        }

        public Metadata GetMetadata(string name)
        {
            Metadata data = Meta.Find(x => x.Name.Equals(name));
            return data;
        }
        public Metadata UpdateMetadata(string name, MetadataType type)
        {
            Metadata data = Meta.Find(x => x.Name.Equals(name));
            data.MetaType = type;
            return data;
        }

        public Metadata UpdateMetadata(string name, MetadataValue value)
        {
            Metadata data = Meta.Find(x => x.Name.Equals(name));
            data.MetaValue = value;
            return data;
        }

        public Metadata UpdateMetadata(string name, MetadataType type, MetadataValue value)
        {
            Metadata data = Meta.Find(x => x.Name.Equals(name));
            data.MetaType = type;
            data.MetaValue = value;
            return data;
        }

        public Metadata[] ToArray()
        { 
            return Meta.ToArray();
        }
    }
}
