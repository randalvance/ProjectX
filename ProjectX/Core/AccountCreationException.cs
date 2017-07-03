using System;

namespace ProjectX.Core
{
    public class AccountCreationException : Exception
    {
        public AccountCreationException(string message) : base(message)
        {
            
        }
    }
}
