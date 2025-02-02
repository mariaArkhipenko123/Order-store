using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Lab.Infrastructure.Context
{
    public class PostgresDbContext :IdentityDbContext<User, Role, Guid>
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<UserAccess> UserAccesses { get; set; }

    }
}