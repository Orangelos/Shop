using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class OpenAccountViewModel
    {
        public int?  ID_Клиента { get; set; }
        public string? ТипСчета { get; set; }
        [Required]
        [Range(0, 1000000, ErrorMessage = "Введите валидную сумму")]
        public decimal НачальныйБаланс { get; set; }
    }
}
