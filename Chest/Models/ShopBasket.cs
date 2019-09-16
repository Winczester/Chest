using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chest.Models
{
    public class ShopBasket
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        public int GoodsPrice { get; set; }
    }
}
