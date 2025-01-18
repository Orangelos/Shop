using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class Транзакция
    {
        [Key]
        public int ID_Транзакции { get; set; }

        [ForeignKey("Счет")]
        public int ID_Счета { get; set; }
        public virtual Счет? Счет { get; set; }

        public DateTime ДатаТранзакции { get; set; }
        public string? ТипТранзакции { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Введите сумму больше нуля")]
        public decimal Сумма { get; set; }
        public string? Описание { get; set; }
    }
}
