using System;

namespace ProjectX.Core
{
    public class InvalidTransferException : Exception
    {
        public InvalidTransferException(string message) : base(message)
        {

        }
    }
}
