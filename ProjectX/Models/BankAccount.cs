using System;
using System.Collections.Generic;

namespace ProjectX.Models
{
    public class BankAccount
    {
        public BankAccount()
        {
            Balance = 0;
            CreatedDate = DateTime.UtcNow;
            Transactions = new List<Transaction>();
        }

        public int ID { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string Password { get; set; }

        public decimal Balance { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<Transaction> Transactions { get; set; }
        
        public byte[] TimeStamp { get; set; }
    }
}