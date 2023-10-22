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

        // We can use Customers or Set<Customer>. This things is same. Doesn't matter.
        // dbContext.Set<Customer>() == dbContext.Customers

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
