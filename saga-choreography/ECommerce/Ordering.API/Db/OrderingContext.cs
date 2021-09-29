using Microsoft.EntityFrameworkCore;
using Ordering.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Db
{
    public class OrderingContext : DbContext
    {
        public OrderingContext(DbContextOptions<OrderingContext> options) : base(options)
        {

        }

        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
