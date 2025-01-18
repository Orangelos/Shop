using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class Contact
    {
        [Display(Name = "Введите имя")]
        [Required(ErrorMessage = "Обязательно введите имя")]
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Имя должно содержать только буквы")]
        public string Name { get; set; }

        [Display(Name = "Введите фамилию")]
        [Required(ErrorMessage = "Обязательно введите фамилию")]
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Фамилия должна содержать только буквы")]
        public string Surname { get; set; }

        [Display(Name = "Введите почту")]
        [Required(ErrorMessage = "Обязательно введите почту")]
        [EmailAddress(ErrorMessage = "Неверный формат почты")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Введите телефон")]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Номер телефона должен содержать от 10 до 15 цифр и может начинаться с '+'.")]
        public string Phone { get; set; }


        [Display(Name = "Введите адрес")]
        [Required(ErrorMessage = "Обязательно введите адрес")]
        [MinLength(4, ErrorMessage = "Адрес некоректен")]
        public string Address { get; set; }
       
       
        [Display(Name = "Что вы хотите спросить? Напишите :)")]
        [Required(ErrorMessage = "Обязательно введите вопрос")]
        [StringLength(6000, ErrorMessage="Превышено количество символов >6000")]
        [MinLength(6, ErrorMessage = "Сообщение слишком короткое")]
        public string Message {  get; set; }

    }
}
