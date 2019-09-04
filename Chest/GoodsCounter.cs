using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chest.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chest
{
    public class GoodsCounter
    {

        public delegate void MethodContainer();
        public event MethodContainer OnAdded;

        public int GoodsCount { get; set; }
        private readonly ChestDatabaseContext _dbContext;

        public GoodsCounter(ChestDatabaseContext context)
        {
            _dbContext = context;
            GoodsCount = _dbContext.Goods.Count()+1;
        }

        public void Added()
        {
            OnAdded();
        }
    }
}
