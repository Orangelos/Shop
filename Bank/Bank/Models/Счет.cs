using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class Счет
    {
        [Key]
        public int ID_Счета { get; set; }

        [ForeignKey("Клиент")]
        public int ID_Клиента { get; set; }
        public string? НомерСчета { get; set; }
        public virtual Клиент? Клиент { get; set; }

        public string? ТипСчета { get; set; }
        [Required]
        [Range(0, 100000, ErrorMessage = "Введите валидную сумму")]
        public decimal Баланс { get; set; }
        public DateTime ДатаСоздания { get; set; }

        public virtual ICollection<Транзакция>? Транзакции { get; set; }
    }
}
