using Bank.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace Bank.Controllers
{
   
        public class ContactsController : Controller
        {
            private readonly ILogger<ContactsController> _logger;

            public ContactsController(ILogger<ContactsController> logger)
            {
                _logger = logger;
            }

            public IActionResult Index()
            {
                return View();
            }

        [HttpPost]
        public IActionResult Check(Contact contact)
        {
            if (ModelState.IsValid)
            {









                string smtpServer = "smtp.mail.ru";
                int smtpPort = 587; // Обычно используется порт 587 для TLS
                string smtpUsername = "arina_andrey2004@mail.ru";
                string smtpPassword = "WdR03VPBFFgBeBG66HPu"; // замените на ваш реальный пароль

                // Создаем объект клиента SMTP
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    // Настройка аутентификации
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(smtpUsername);
                        mailMessage.To.Add("venesken@mail.ru"); // Укажите адрес получателя
                        mailMessage.Subject = "Обратная связь";
                        mailMessage.Body = $"Имя: {contact.Name} \r\n Фамилия: {contact.Surname}\r\n Почта: {contact.Email}\r\n Телефон: {contact.Phone}\r\n Адрес: {contact.Address}\r\n Сообщение: {contact.Message}  ";

                        try
                        {
                            // Отправляем сообщение
                            smtpClient.Send(mailMessage);
                            Console.WriteLine("Сообщение успешно отправлено.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка отправки сообщения: {ex.Message}");
                        }
                    }
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(smtpUsername);
                        mailMessage.To.Add($"{contact.Email}"); // Укажите адрес получателя
                        mailMessage.Subject = "Обратная связь";
                        mailMessage.Body = $"Здравствуйте {contact.Name}, спасибо, что обратились в службу поддержки, ваше сообщение получено,\n ближайший свободный оператор ответит вам, а пока проверьте правильность введенных данных\n Имя: {contact.Name} \r\n Фамилия: {contact.Surname}\r\n Почта: {contact.Email}\r\n Телефон: {contact.Phone}\r\n Адрес: {contact.Address}";

                        try
                        {
                            // Отправляем сообщение
                            smtpClient.Send(mailMessage);
                            Console.WriteLine("Сообщение успешно отправлено.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка отправки сообщения: {ex.Message}");
                        }
                    }
                }

                return RedirectToAction("ThankYou");
            }
            return View("Index");
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }

