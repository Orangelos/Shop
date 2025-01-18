using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Bank.Models
{
    public class OpenСreditModel
    {
        public int? ID_Клиента { get; set; }
        public string? ТипКредита { get; set; }
        public decimal СуммаКредита { get; set; }
        public decimal ПроцентнаяСтавка { get; set; }
        public string? Статус { get; set; }
        public DateTime ДатаОкончания { get; set; }
    }
}
