using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chest.Models
{
    public class ManufacturerCategory
    {

        public int ManufacturerID { get; set; }
        public int CategoryID { get; set; }

        public Manufacturer Manufacturer { get; set; }
        public Category Category { get; set; }

    }
}
