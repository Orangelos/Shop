using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class Кредит
    {
        [Key]
        public int ID_Кредита { get; set; }

        [ForeignKey("Клиент")]
        public int ID_Клиента { get; set; }
        public virtual Клиент? Клиент { get; set; }

        public string? ТипКредита { get; set; }
        public decimal ОсновнаяСумма { get; set; }
        public decimal ПроцентнаяСтавка { get; set; }

        public string? Статус { get; set; }
        public DateTime ДатаНачала { get; set; }
        public DateTime ДатаОкончания { get; set; }

        
        public virtual ICollection<ПлатежПоКредиту> Платежи { get; set; }
    }
}
