using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using ProjectX.Models;

namespace ProjectX.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>().HasKey(x => x.ID);
        }
    }
}
