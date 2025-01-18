using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class Product
    {
        [Key]
        public int Id_Product { get; set; }
        public string BrandName { get; set; } // Название бренда
        public string ImageUrl { get; set; }  // Ссылка на изображение
        public decimal Price { get; set; }   // Цена

        public string Kategory { get; set; }   // Цена
        public string Description { get; set; } // Описание
    }
}
