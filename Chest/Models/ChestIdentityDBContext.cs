﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chest.Models
{
    public class ChestIdentityDBContext : IdentityDbContext<User>
    {
        public ChestIdentityDBContext(DbContextOptions options) : base(options) { }
    }
}
