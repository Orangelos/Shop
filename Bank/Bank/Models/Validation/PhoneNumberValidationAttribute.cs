using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class PhoneNumberValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var phone = value as string;
        if (string.IsNullOrEmpty(phone))
        {
            return new ValidationResult("Телефон обязателен для ввода!");
        }

        if (!phone.StartsWith("+7"))
        {
            return new ValidationResult("Телефон должен начинаться с +7");
        }

        
        if (phone.Length < 12)
        {
            return new ValidationResult("Телефон должен содержать 12 символов!");
        }

       
        var digitsOnly = phone.Substring(2); 
        if (!Regex.IsMatch(digitsOnly, @"^\d+$"))
        {
            return new ValidationResult("Телефон должен содержать только цифры!");
        }

        return ValidationResult.Success;
    }
}