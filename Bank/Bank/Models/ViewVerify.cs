using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class ViewVerify
    {
        

            public int ID_Клиента { get; set; }

            [Required(ErrorMessage = "Имя обязательно для ввода")]
            [StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
            [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Имя должно содержать только буквы")]
            public string? Имя { get; set; }// = "Иван";


            [Required(ErrorMessage = "Фамилия обязательна для ввода")]
            [StringLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов")]
            [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Фамилия должна содержать только буквы")]
            public string? Фамилия { get; set; } //= "Иванов";


            [Required(ErrorMessage = "Дата рождения обязательна для ввода")]
            [DataType(DataType.Date, ErrorMessage = "Введите корректную дату")]
            [Display(Name = "Дата рождения")]
            public DateTime ДатаРождения { get; set; } //= DateTime.Today.AddYears(-10);


            [Required]
            [Display(Name = "Номер телефона")]
            [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Номер телефона должен содержать от 10 до 15 цифр и может начинаться с '+'.")]
            public string? PhoneNumber { get; set; }// = "+79991234567";


            [Required(ErrorMessage = "Обязательно введите почту")]
            [EmailAddress(ErrorMessage = "Неверный формат почты")]
            public string? ЭлектроннаяПочта { get; set; } //= "example@mail.com";


            [Required(ErrorMessage = "Пароль обязателен для заполнения.")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "Пароль должен быть не менее 6 символов.")]
            [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "Пароль должен содержать хотя бы одну букву и одну цифру.")]
            public string? Пароль { get; set; }

        [Required(ErrorMessage = "Код обязателен для заполнения.")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Код должен состоять ровно из 4 цифр.")]
        public int? Verify { get; set; }

        [ForeignKey("Отделение")]
            [Required]
            public int ID_Отделения { get; set; }
            // public virtual ОтделениеБанка Отделение { get; set; }
            //
            // public virtual ICollection<Счет> Счета { get; set; }
            // public virtual ICollection<Кредит> Кредиты { get; set; }
        
    }
}
