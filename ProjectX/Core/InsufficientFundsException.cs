using System;

namespace ProjectX.Core
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base("Insufficient funds.")
        {
        }
    }
}
