using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chest.Models
{
    interface IGoods
    {

        int ID { get; set; }
        string Name { get; set; }
        int Price { get; set; }

    }
}
