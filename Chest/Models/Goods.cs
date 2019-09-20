using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Chest.Models
{
    [DataContract]
    public class Goods : IGoods
    {
        [DataMember]
        public int ID { get; set; } // Primary key
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Price { get; set; }

        [DataMember]
        public int CategoryID { get; set; } // Foreign key to Category
        [DataMember]
        public int ManufacturerID { get; set; } // Foreign key to Manufacturer

        [DataMember]
        public Category Category { get; set; }
        [DataMember]
        public Manufacturer Manufacturer { get; set; }

    }
}
