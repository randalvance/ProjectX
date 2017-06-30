using System;
using System.Collections.Generic;
using ProjectX.Models;

namespace ProjectX.Business
{
    public class TransactionCreator : ITransactionCreator
    {
        public Transaction Widthraw(int accountID, decimal amount)
        {
            return new Transaction
            {
                BankAccountID = accountID,
                TransactionType = TransactionType.Debit,
                Amount = amount,
                Description = $"Widthrawn amount of ${amount}."
            };
        }

        public Transaction Deposit(int accountID, decimal amount)
        {
            return new Transaction
            {
                BankAccountID = accountID,
                TransactionType = TransactionType.Credit,
                Amount = amount,
                Description = $"Deposited amount of ${amount}."
            };
        }

        public List<Transaction> TransferTo(int sourceAccountID, int destinationAccountID, decimal amount)
        {
            return new List<Transaction>
            {
                new Transaction
                {
                    BankAccountID = sourceAccountID,
                    TransactionType = TransactionType.Credit,
                    Amount = amount,
                    Description = $"Transferred amount of ${amount} to account {destinationAccountID}."
                },
                new Transaction
                {
                    BankAccountID = sourceAccountID,
                    TransactionType = TransactionType.Credit,
                    Amount = amount,
                    Description = $"Received amount of ${amount} from account {sourceAccountID}."
                }
            };
        }
    }
}
