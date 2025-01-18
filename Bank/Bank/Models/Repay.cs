using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class Repay
    {
        public int SourceCreditId { get; set; }

        [Required]
        [Display(Name = "Счет списания")]
        public int DestinationAccountId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Введите сумму больше нуля")]
        [Display(Name = "Сумма погашения")]
        public decimal Amount { get; set; }

        public string ТипСчета { get; set; }
        public SelectList ClientAccounts { get; set; }

    }


}
