using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class Banner
    {
        [Key]
        public int Id { get; set; }
        public string URL { get; set; } 
        public string ImageUrl { get; set; }  

    }
}
