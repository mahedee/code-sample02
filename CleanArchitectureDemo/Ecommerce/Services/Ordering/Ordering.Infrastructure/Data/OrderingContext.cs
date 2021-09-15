using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderingContext : DbContext
    {
        public OrderingContext(DbContextOptions<OrderingContext> options) : base (options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
