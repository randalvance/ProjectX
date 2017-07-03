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
            modelBuilder.Entity<BankAccount>().HasAlternateKey(x => x.AccountName);
            modelBuilder.Entity<BankAccount>().HasAlternateKey(x => x.AccountNumber);
            modelBuilder.Entity<BankAccount>().Property(x => x.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<BankAccount>().Property(x => x.AccountName).IsRequired();
            modelBuilder.Entity<BankAccount>().Property(x => x.AccountNumber).IsRequired();
            modelBuilder.Entity<BankAccount>().Property(x => x.Password).IsRequired();
            modelBuilder.Entity<BankAccount>().Property(x => x.Balance).IsRequired().HasDefaultValue(0);
            modelBuilder.Entity<BankAccount>().Property(x => x.TimeStamp).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

            modelBuilder.Entity<Transaction>().HasKey(x => x.ID);
            modelBuilder.Entity<Transaction>().Property(x => x.TransactionDate).IsRequired();
            modelBuilder.Entity<Transaction>().Property(x => x.TransactionType).IsRequired();
            modelBuilder.Entity<Transaction>().Property(x => x.Amount).IsRequired();
            modelBuilder.Entity<Transaction>().Property(x => x.Balance).IsRequired();
            modelBuilder.Entity<Transaction>().HasOne(x => x.BankAccount).WithMany(x => x.Transactions);
        }
    }
}
