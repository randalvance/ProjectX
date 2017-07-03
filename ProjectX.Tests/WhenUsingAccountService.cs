using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ProjectX.Data;
using ProjectX.Business;
using ProjectX.Models;
using System.Linq;

namespace ProjectX.Tests
{
    [TestClass]
    public class WhenUsingAccountService
    {
        [TestMethod]
        public void ShouldBeAbleToDepositFunds()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var bankAccount = new BankAccount
            {
                AccountName = "Test",
                AccountNumber = "1",
                Balance = 100,
                Password = "password"
            };

            BankAccount accountFromDb;

            // Act
            using (var context = GetDbContextForTestingTransactionOperation(dbName, new BankAccount[] { bankAccount }))
            {
                var accountService = new AccountService(context);
                accountService.Deposit(bankAccount, 100);
                accountFromDb = context.BankAccounts.Include(x => x.Transactions).SingleOrDefault(x => x.ID == bankAccount.ID);
            }

            // Assert
            Assert.AreEqual(200, accountFromDb.Balance);
            Assert.AreEqual(1, accountFromDb.Transactions.Count);
            Assert.AreEqual(100, accountFromDb.Transactions.First().Amount);
            Assert.AreEqual(200, accountFromDb.Transactions.First().Balance);
            Assert.AreEqual(TransactionType.Credit, accountFromDb.Transactions.First().TransactionType);
        }

        [TestMethod]
        public void ShouldBeAbleToWidthrawFunds()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var bankAccount = new BankAccount
            {
                AccountName = "Test",
                AccountNumber = "1",
                Balance = 1000,
                Password = "password"
            };

            BankAccount accountFromDb;

            // Act
            using (var context = GetDbContextForTestingTransactionOperation(dbName, new BankAccount[] { bankAccount }))
            {
                var accountService = new AccountService(context);
                accountService.Widthraw(bankAccount, 100);
                accountFromDb = context.BankAccounts.Include(x => x.Transactions).SingleOrDefault(x => x.ID == bankAccount.ID);
            }

            // Assert
            Assert.AreEqual(900, accountFromDb.Balance);
            Assert.AreEqual(1, accountFromDb.Transactions.Count);
            Assert.AreEqual(100, accountFromDb.Transactions.First().Amount);
            Assert.AreEqual(900, accountFromDb.Transactions.First().Balance);
            Assert.AreEqual(TransactionType.Debit, accountFromDb.Transactions.First().TransactionType);
        }

        [TestMethod]
        public void ShouldBeAbleToTransferFunds()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var sourceAccount = new BankAccount
            {
                AccountName = "Test 1",
                AccountNumber = "1",
                Balance = 1000,
                Password = "password"
            };
            var destinationAccount = new BankAccount
            {
                AccountName = "Test 2",
                AccountNumber = "2",
                Balance = 1000,
                Password = "password"
            };

            BankAccount sourceAccountFromDb, destAccountFromDb;

            // Act
            using (var context = GetDbContextForTestingTransactionOperation(dbName, new BankAccount[] { sourceAccount, destinationAccount }))
            {
                var accountService = new AccountService(context);
                accountService.Transfer(sourceAccount, destinationAccount, 100);

                sourceAccountFromDb = context.BankAccounts.Include(x => x.Transactions).SingleOrDefault(x => x.ID == sourceAccount.ID);
                destAccountFromDb = context.BankAccounts.Include(x => x.Transactions).SingleOrDefault(x => x.ID == destinationAccount.ID);
            }

            // Assert
            Assert.AreEqual(900, sourceAccountFromDb.Balance);
            Assert.AreEqual(1, sourceAccountFromDb.Transactions.Count);
            Assert.AreEqual(100, sourceAccountFromDb.Transactions.First().Amount);
            Assert.AreEqual(900, sourceAccountFromDb.Transactions.First().Balance);
            Assert.AreEqual(TransactionType.Debit, sourceAccountFromDb.Transactions.First().TransactionType);

            Assert.AreEqual(1100, destinationAccount.Balance);
            Assert.AreEqual(1, destinationAccount.Transactions.Count);
            Assert.AreEqual(100, destinationAccount.Transactions.First().Amount);
            Assert.AreEqual(1100, destinationAccount.Transactions.First().Balance);
            Assert.AreEqual(TransactionType.Credit, destinationAccount.Transactions.First().TransactionType);
        }
        
        [TestMethod]
        public void ShouldBeAbleToCreateAnAccount()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var bankAccount = new BankAccount
            {
                AccountName = "Test",
                AccountNumber = "1",
                Balance = 1000,
                Password = "password"
            };

            BankAccount accountFromDb;
            int accountCount;

            // Act
            using (var context = GetDbContextForTestingTransactionOperation(dbName))
            {
                var accountService = new AccountService(context);
                accountService.CreateAccount(bankAccount);
            }

            using (var context = GetDbContextForTestingTransactionOperation(dbName))
            {
                accountFromDb = context.BankAccounts.SingleOrDefault(x => x.ID == bankAccount.ID);
                accountCount = context.BankAccounts.Count();
            }

            // Assert
            Assert.AreEqual(1, accountCount);
            Assert.IsNotNull(accountFromDb);
            Assert.AreEqual(bankAccount.AccountName, accountFromDb.AccountName);
            Assert.AreEqual(bankAccount.AccountNumber, accountFromDb.AccountNumber);
            Assert.AreEqual(bankAccount.Balance, accountFromDb.Balance);
            Assert.AreEqual(bankAccount.Password, accountFromDb.Password);
            Assert.AreEqual(bankAccount.CreatedDate, accountFromDb.CreatedDate);
        }

        private AppDbContext GetDbContextForTestingTransactionOperation(string dbName, BankAccount[] seedData = null)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: dbName)
                    .Options;

            var context = new AppDbContext(options);

            if (seedData != null)
            {
                context.BankAccounts.AddRange(seedData);
                context.SaveChanges();
            }

            return context;
        }
    }
}
