using System;
using System.Collections.Generic;
using ProjectX.Models;

namespace ProjectX.Business
{
    public interface IAccountService
    {
        void Widthraw(BankAccount account, decimal amount);
        void Deposit(BankAccount account, decimal amount);
        void TransferTo(BankAccount sourceAccount, BankAccount destinationAccount, decimal amount);
    }
}
