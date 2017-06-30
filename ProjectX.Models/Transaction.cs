using System;

namespace ProjectX.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        public int BankAccountID { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}