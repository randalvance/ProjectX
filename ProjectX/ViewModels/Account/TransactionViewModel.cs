using System.ComponentModel.DataAnnotations;

namespace ProjectX.ViewModels.Account
{
    public class TransactionViewModel
    {
        public TransactionType Type { get; set; }

        public string AccountNumber { get; set; }

        public decimal Balance { get; set; }

        [Required]
        [Range(minimum: 100, maximum: 1000000)]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; } = 0;

        public enum TransactionType
        {
            Deposit = 0,
            Widthraw = 1,
            Transfer = 2
        }
    }
}
