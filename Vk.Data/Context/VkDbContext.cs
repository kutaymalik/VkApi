using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vk.Data.Domain;

namespace Vk.Data.Context
{
    public class VkDbContext : DbContext
    {
        public VkDbContext(DbContextOptions<VkDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new EftTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new AccountTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new CardConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
