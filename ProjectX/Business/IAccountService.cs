using ProjectX.Models;
using System.Collections.Generic;

namespace ProjectX.Business
{
    public interface IAccountService
    {
        void Deposit(BankAccount account, decimal amount);
        void Transfer(BankAccount sourceAccount, BankAccount destinationAccount, decimal amount);
        void Widthraw(BankAccount account, decimal amount);
        void CreateAccount(BankAccount bankAccount);
        BankAccount RetrieveByAccountName(string accountName, bool includeTransactions = false);
        BankAccount RetrieveByAccountNumber(string accountNumber);
        BankAccount RetrieveByID(int id);
    }
}