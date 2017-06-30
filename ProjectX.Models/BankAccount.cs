using System;
using System.Collections.Generic;

namespace ProjectX.Models
{
    public class BankAccount
    {
        public int ID { get; set; }
        public decimal Balance { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}