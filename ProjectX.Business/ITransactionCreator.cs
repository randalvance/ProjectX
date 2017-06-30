using System;
using System.Collections.Generic;
using ProjectX.Models;

namespace ProjectX.Business
{
    public interface ITransactionCreator
    {
        Transaction Widthraw(int accountID, decimal amount);
        Transaction Deposit(int accountID, decimal amount);
        List<Transaction> TransferTo(int sourceAccountID, int destinationAccountID, decimal amount);
    }
}
