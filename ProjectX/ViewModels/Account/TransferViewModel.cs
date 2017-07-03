using System.ComponentModel.DataAnnotations;

namespace ProjectX.ViewModels.Account
{
    public class TransferViewModel : TransactionViewModel
    {
        public TransferViewModel()
        {
            Type = TransactionType.Transfer;
        }

        [Required]
        [Display(Name = "Destination Account Number")]
        public string DestinationAccountNumber { get; set; }
    }
}
