using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class Karzina
    {
        [Key]
        public int Id { get; set; }
        public string BrandName { get; set; } // Название бренда
        public string ImageUrl { get; set; }  // Ссылка на изображение
        public decimal Price { get; set; }   // Цена
        public string Description { get; set; } // Описание

        public int kolv { get; set; } = 1;

    }
}
