using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Chest.Models
{
    [DataContract]
    public class Category : ICategory
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }

        public List<Goods> Goods { get; set; }
        public List<ManufacturerCategory> ManufacturerCategories { get; set; }

    }
}
