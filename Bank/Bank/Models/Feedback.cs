using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class FeedBack
    {

        [Key]
        public int Id { get; set; }
        public string? Имя { get; set; }// = "Иван";


        [Required(ErrorMessage = "Фамилия обязательна для ввода")]
        [StringLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов")]
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Фамилия должна содержать только буквы")]
        public string? Фамилия { get; set; } //= "Иванов";

        [Required(ErrorMessage = "Обязательно введите почту")]
        [EmailAddress(ErrorMessage = "Неверный формат почты")]
        public string? ЭлектроннаяПочта { get; set; } //= "example@mail.com";

        [Required(ErrorMessage = "Ваш отзыв...")]
        public string? Otziv { get; set; }
    }
}