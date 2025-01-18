using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Bank.Models;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Controllers
{
    public class Service : Controller
    {
        private readonly BankContext _context;

        public Service(BankContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string password)
        {
            string result = "123123123";
            if (result == password)
            {
                return RedirectToAction("ProfileServ");
            }

            ModelState.AddModelError("", "Неверный пароль.");
            return View("Login");
        }

        [Authorize]
        public IActionResult ProfileServ()
        {
            var pendingCredits = _context.Кредиты
                .Include(k => k.Клиент)
                .Where(k => k.Статус == "На рассмотрении")
                .ToList();

            return View(pendingCredits);
        }

        public async Task<IActionResult> UpdateCreditStatus(int creditId, string status, int clientId)
        {
            // Получаем кредит клиента
            var credit = await _context.Кредиты.FindAsync(creditId);

            // Проверяем наличие кредита
            if (credit != null)
            {
                // Обновляем статус кредита
                credit.Статус = status;
                await _context.SaveChangesAsync(); // Сохраняем изменения
            }

            // Если статус кредита активный, обновляем баланс счета
            if (status == "Активный")
            {
                // Получаем кредитный счет клиента
                var creditAccount = _context.Счета
                    .FirstOrDefault(a => a.Клиент.ID_Клиента == clientId && a.ТипСчета == "Кредитный");

                // Проверяем наличие счета и обновляем его баланс
                if (creditAccount != null)
                {

                    if (credit.ТипКредита == "Долгосрочный  5 лет")
                        creditAccount.Баланс += credit.ОсновнаяСумма / Convert.ToDecimal((1.14) * (1.14) * (1.14) * (1.14) * (1.14));


                    if (credit.ТипКредита == "Среднесрочный 3 года")
                        creditAccount.Баланс += credit.ОсновнаяСумма / Convert.ToDecimal((1.18) * (1.18) * (1.18));

                    if (credit.ТипКредита == "Краткосрочный 1 год")
                        creditAccount.Баланс += credit.ОсновнаяСумма / Convert.ToDecimal(1.26);






                    // Сохраняем изменения в контексте базы данных
                    await _context.SaveChangesAsync();
                }
            }

            // Перенаправляем пользователя на профиль
            return RedirectToAction("ProfileServ");
        }




        //---
    }
}
