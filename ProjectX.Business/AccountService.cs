using System;
using System.Collections.Generic;
using ProjectX.Models;

namespace ProjectX.Business
{
    public class AccountService : IAccountService
    {
        private ITransactionCreator _transactionCreator;

        public AccountService(ITransactionCreator transactionCreator)
        {
            _transactionCreator = transactionCreator;
        }

        public void Widthraw(BankAccount account, decimal amount)
        {
            var transactions = _transactionCreator.Widthraw(account.ID, amount);
        }

        public void Deposit(BankAccount account, decimal amount)
        {
            var transactions = _transactionCreator.Deposit(account.ID, amount);
        }

        public void TransferTo(BankAccount sourceAccount, BankAccount destinationAccount, decimal amount)
        {
            var transactions = _transactionCreator.TransferTo(sourceAccount.ID, destinationAccount.ID, amount);
        }

        private void ApplyTransaction(BankAccount bankAccount, Transaction transaction)
        {
            ApplyTransactions(bankAccount, new List<Transaction>() { transaction });
        }

        private void ApplyTransactions(BankAccount bankAccount, IEnumerable<Transaction> transactions)
        {
            foreach(var transaction in transactions)
            {
                if (transaction.TransactionType == TransactionType.Credit)
                {
                    bankAccount.Balance -= transaction.Amount;
                }

            }
        }
    }
}
