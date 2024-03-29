﻿using Microsoft.EntityFrameworkCore;
using RoutingDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingDemo.Data {
    public class UserContext : DbContext {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options) {
        }

        public DbSet<User> User { get; set; }
    }
}
