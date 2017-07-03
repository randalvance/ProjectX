using Microsoft.EntityFrameworkCore;
using ProjectX.Data;
using ProjectX.Models;
using ProjectX.Core;
using System;
using System.Linq;
using System.Threading;

namespace ProjectX.Business
{
    public class AccountService : IAccountService
    {
        private AppDbContext _appDbContext;

        public AccountService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Widthraw(BankAccount account, decimal amount)
        {
            var transaction = new Transaction
            {
                BankAccountID = account.ID,
                TransactionType = TransactionType.Debit,
                Amount = amount,
                Description = $"Widthrawn amount of ${amount}."
            };
            ApplyTransaction(account, transaction);
            _appDbContext.SaveChanges();
        }

        public void Deposit(BankAccount account, decimal amount)
        {
            var transaction = new Transaction
            {
                BankAccountID = account.ID,
                TransactionType = TransactionType.Credit,
                Amount = amount,
                Description = $"Deposited amount of ${amount}."
            };
            ApplyTransaction(account, transaction);
            _appDbContext.SaveChanges();
        }

        public void Transfer(BankAccount sourceAccount, BankAccount destinationAccount, decimal amount)
        {
            if (sourceAccount.ID == destinationAccount.ID)
            {
                throw new InvalidTransferException("Transfer to your own account is not allowed.");
            };

            ApplyTransaction(sourceAccount, new Transaction
            {
                BankAccountID = sourceAccount.ID,
                TransactionType = TransactionType.Debit,
                Amount = amount,
                Description = $"Transferred amount of ${amount} to account {destinationAccount.AccountNumber}."
            });
            ApplyTransaction(destinationAccount, new Transaction
            {
                BankAccountID = destinationAccount.ID,
                TransactionType = TransactionType.Credit,
                Amount = amount,
                Description = $"Received amount of ${amount} from account {sourceAccount.AccountNumber}."
            });
            _appDbContext.SaveChanges();
        }

        public void CreateAccount(BankAccount bankAccount)
        {
            if (bankAccount == null)
            {
                throw new ArgumentNullException(nameof(bankAccount));
            }

            var existingAccount = _appDbContext.BankAccounts.SingleOrDefault(x => x.AccountNumber == bankAccount.AccountNumber || x.AccountName == bankAccount.AccountName);

            if (existingAccount != null)
            {
                if (existingAccount.AccountName == bankAccount.AccountName)
                {
                    throw new AccountCreationException("Account Name already in use. Please choose another one.");
                }
                if (existingAccount.AccountNumber == bankAccount.AccountNumber)
                {
                    throw new AccountCreationException("Account Number already in use. Please choose another one.");
                }
            }

            _appDbContext.BankAccounts.Add(bankAccount);
            _appDbContext.SaveChanges();
        }

        public BankAccount RetrieveByID(int id)
        {
            return _appDbContext.BankAccounts.FirstOrDefault(x => x.ID == id);
        }

        public BankAccount RetrieveByAccountName(string accountName, bool includeTransactions = false)
        {
            return _appDbContext.BankAccounts.Include(x => x.Transactions).FirstOrDefault(x => x.AccountName == accountName);
        }

        public BankAccount RetrieveByAccountNumber(string accountNumber)
        {
            return _appDbContext.BankAccounts.FirstOrDefault(x => x.AccountNumber == accountNumber);
        }

        private void ApplyTransaction(BankAccount bankAccount, Transaction transaction)
        {
            // Uncomment to simulate a delay to prove we can handle concurrency problem (ie open 2 browser tabs and submit consequently)
            // Thread.Sleep(3000); 

            if (bankAccount == null)
            {
                throw new ArgumentNullException("Bank account is null");
            }

            if (transaction == null)
            {
                throw new ArgumentNullException("Transaction is null");
            }

            if (bankAccount.Transactions == null)
            {
                throw new InvalidOperationException("Bank account must have transactions loaded.");
            }

            bankAccount.Transactions.Add(transaction);

            if (transaction.TransactionType == TransactionType.Credit)
            {
                bankAccount.Balance += transaction.Amount;
            }
            else if (transaction.TransactionType == TransactionType.Debit)
            {
                if (bankAccount.Balance < transaction.Amount)
                {
                    throw new InsufficientFundsException();
                }

                bankAccount.Balance -= transaction.Amount;
            }

            transaction.Balance = bankAccount.Balance;
        }
    }
}
