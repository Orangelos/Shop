using Bank.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bank.Controllers
{
    public class AccountController : Controller
    {
        private readonly BankContext _context;
        private readonly IPasswordHasher<Клиент> _passwordHasher;

        public AccountController(BankContext context, IPasswordHasher<Клиент> passwordHasher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }
        public IActionResult Index1()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var клиент = _context.Клиенты.FirstOrDefault(k => k.ЭлектроннаяПочта == email);

            if (клиент != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(клиент, клиент.Пароль, password);

                if (result == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, клиент.ЭлектроннаяПочта),
                    new Claim("ClientId", клиент.ID_Клиента.ToString())
                };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Profile");
                }
            }

            ModelState.AddModelError("", "Неверная электронная почта или пароль.");
            return View("Login");
        }


        [HttpPost]
        public JsonResult RemoveFromCart(int id)
        {
            var item = _context.Karzina.FirstOrDefault(s => s.Id == id);

            if (item != null)
            {
                _context.Karzina.Remove(item);
                _context.SaveChanges();
            }

            // Возвращаем статус и обновленную сумму товаров
            var total = _context.Karzina.Sum(x => x.Price);
            return Json(new { success = true, totalPrice = total });
        }


        [HttpPost]
        public IActionResult AddToCart([FromBody] Karzina item)
        {
            if (item != null)
            {
                var newItem = new Karzina
                {
                    BrandName = item.BrandName,
                    ImageUrl = item.ImageUrl,
                    Price = item.Price,
                    Description = item.Description
                };

                _context.Karzina.Add(newItem);
                _context.SaveChanges();
                item.Id = newItem.Id;

                // Обновляем количество предметов
                // Подсчёт всех товаров в корзине
                var cartCount = _context.Karzina.Count();

                // Возвращаем результат в формате JSON
                return Json(new { count = cartCount });
            }

            return BadRequest(new { message = "Ошибка добавления товара в корзину" });
        }




        [Authorize]
        public IActionResult Profile()
        {
            var клиентId = int.Parse(User.Claims.First(c => c.Type == "ClientId").Value);

            var клиент = _context.Клиенты.FirstOrDefault(k => k.ID_Клиента == клиентId);
            var счета = _context.Счета.Where(s => s.ID_Клиента == клиентId).ToList();
            var кредиты = _context.Кредиты.Where(c => c.ID_Клиента == клиентId).ToList();

            var корзина = _context.Karzina.ToList();

            var модель = new ЛичныйКабинетМодель
            {
                Клиент = клиент,
                Счета = счета,
                Кредиты = кредиты,
                Корзина = корзина
            };

            if (клиент.ID_Отделения == 1)
            {
                модель.otdel = "Ботаническая улица 41к7";
            }
            else if (клиент.ID_Отделения == 2)
            {
                модель.otdel = "Строгинский бульвар 12";
            }
            else
            {
                модель.otdel = "Студенческая улица 33";
            }


            return View(модель);
        }


        [HttpGet]
        public IActionResult GetCartCount()
        {
            // Подсчёт всех товаров в корзине
            var cartCount = _context.Karzina.Count();

            // Возвращаем результат в формате JSON
            return Json(new { count = cartCount });
        }



        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }






        // Метод для отображения формы открытия счета
        [HttpGet]
        public IActionResult OpenS(int id)
        {
            // Вывод id для проверки
            Console.WriteLine($"Поступившее ID_Клиента: {id}");

            var model = new OpenAccountViewModel { ID_Клиента = id };
            return View(model);
        }

        // Метод для обработки открытия счета
        [HttpPost]
        public IActionResult OpenAccount(OpenAccountViewModel model)
        {
            if (model.ID_Клиента == null || model.ID_Клиента == 0)
            {
                ModelState.AddModelError("Login", "Не найден ID клиента.");
                return View(model);
            }

            var currentUser = _context.Клиенты.FirstOrDefault(c => c.ID_Клиента == model.ID_Клиента.Value);

            if (currentUser != null)
            {
                var currentAccounts = _context.Счета.Where(a => a.Клиент.ID_Клиента == currentUser.ID_Клиента).ToList();

                // Проверка на количество счетов
                if (currentAccounts.Count >= 3)
                {
                    ModelState.AddModelError("OpenAccount", "Вы не можете открыть больше 3 счетов.");
                    return View("OpenAccount", model);
                }

                // Проверка на количество кредитных счетов
                if (model.ТипСчета == "Кредитный" && currentAccounts.Any(a => a.ТипСчета == "Кредитный"))
                {
                    ModelState.AddModelError("OpenAccount", "Вы не можете открыть больше 1 кредитного счета.");
                    return View("Index1", model);
                }

                var account = new Счет
                {
                    Клиент = currentUser,
                    ТипСчета = model.ТипСчета,
                    Баланс = model.НачальныйБаланс,
                    ДатаСоздания = DateTime.Now,
                    НомерСчета = GenerateAccountNumber()
                };

                _context.Счета.Add(account);
                _context.SaveChanges();

                return RedirectToAction("Profile", new { id = currentUser.ID_Клиента });
            }

            ModelState.AddModelError("OpenAccount", "Не удалось найти пользователя.");
            return View("OpenAccount", model);
        }

        //---
        int tupa;

        // Метод для удаления счета
        [HttpPost]
        public IActionResult DeleteAccount(int accountId, int клиентId)
        {
            tupa = accountId;
            var accountToDelete = _context.Счета.FirstOrDefault(a => a.ID_Счета == accountId && a.Клиент.ID_Клиента == клиентId);

            if (accountToDelete != null)
            {
                var otherAccounts = _context.Счета.Where(a => a.Клиент.ID_Клиента == клиентId && a.ID_Счета != accountId).ToList();

                if (otherAccounts.Any())
                {
                    // Переводим баланс на первый из доступных счетов
                    otherAccounts.First().Баланс += accountToDelete.Баланс;

                    _context.Счета.Remove(accountToDelete);
                    _context.SaveChanges();

                    TempData["Message"] = "Счёт успешно удалён, и баланс был переведен на другой счёт.";
                }
                else
                {
                    TempData["Error"] = "Удаление невозможно, так как это единственный счёт клиента.";
                }

            }

            return RedirectToAction("Profile", new { id = клиентId });
        }

        // Метод для генерации номера счета
        private string GenerateAccountNumber()
        {
            var random = new Random();
            return new string(Enumerable.Repeat("0123456789", 20).Select(s => s[random.Next(s.Length)]).ToArray());
        }








        // Метод для отображения формы открытия 
        [HttpGet]
        public IActionResult OpenC(int id)
        {
            // Вывод id для проверки
            Console.WriteLine($"Поступившее ID_Клиента: {id}");

            var model = new OpenСreditModel { ID_Клиента = id };
            return View(model);
        }





        // Метод для обработки открытия кредита
        [HttpPost]
        public IActionResult OpenCredit(OpenСreditModel model)
        {


            if (model.ID_Клиента == null || model.ID_Клиента == 0)
            {
                ModelState.AddModelError("Login", "Не найден ID клиента.");
                return View(model);
            }

            var currentUser = _context.Клиенты.FirstOrDefault(c => c.ID_Клиента == model.ID_Клиента.Value);


            if (currentUser != null)
            {
                var currentAccounts = _context.Кредиты.Where(a => a.Клиент.ID_Клиента == currentUser.ID_Клиента).ToList();






                var account = new Кредит
                {
                    Клиент = currentUser,
                    ТипКредита = model.ТипКредита,
                    ОсновнаяСумма = model.СуммаКредита,
                    ДатаНачала = DateTime.Now,
                    ДатаОкончания = model.ДатаОкончания,
                    ПроцентнаяСтавка = model.ПроцентнаяСтавка,
                    Статус = "На рассмотрении"
                    // НомерСчета = GenerateAccountNumber()
                };

                if (currentAccounts.Count > 5)
                {
                    model.Статус = "Отменен";
                    account.Статус = "Отменен";
                    _context.Кредиты.Add(account);

                    ModelState.AddModelError("OpenCredit", "Вы не можете взять больше 2 кредитов.");
                    return View("OpenCredit", model);

                }
                var otherAccounts = _context.Счета
    .Where(a => a.Клиент.ID_Клиента == model.ID_Клиента && a.ID_Счета != tupa && a.ТипСчета == "Кредитный")
    .ToList();

                if (otherAccounts.Any())
                {
                    // Переводим баланс на первый из "Кредитных" счетов
                    // otherAccounts.First().Баланс += model.СуммаКредита;

                    TempData["Message"] = "Кредит оформлен и находится на рассмотрении";
                }
                else
                {
                    model.Статус = "Отменен";
                    account.Статус = "Отменен";
                    _context.Кредиты.Add(account);

                    TempData["Error"] = "Взять кредит не удалось, нет подходящего кредитного счета.";
                }

                if (model.ТипКредита == "Долгосрочный  5 лет")
                {
                    model.ПроцентнаяСтавка = 14;
                    account.ПроцентнаяСтавка = model.ПроцентнаяСтавка;
                    model.ДатаОкончания = DateTime.Now;
                    model.ДатаОкончания = model.ДатаОкончания.AddMonths(60);
                    account.ДатаОкончания = model.ДатаОкончания;
                    model.СуммаКредита = model.СуммаКредита * Convert.ToDecimal((1.14) * (1.14) * (1.14) * (1.14) * (1.14));
                    account.ОсновнаяСумма = model.СуммаКредита;
                }
                else
                    if (model.ТипКредита == "Среднесрочный 3 года")
                {
                    model.ПроцентнаяСтавка = 18;
                    account.ПроцентнаяСтавка = model.ПроцентнаяСтавка;
                    model.ДатаОкончания = DateTime.Now;
                    model.ДатаОкончания = model.ДатаОкончания.AddMonths(48);
                    account.ДатаОкончания = model.ДатаОкончания;
                    model.СуммаКредита = model.СуммаКредита * Convert.ToDecimal(1.18);
                    model.СуммаКредита = model.СуммаКредита * Convert.ToDecimal(1.18);
                    model.СуммаКредита = model.СуммаКредита * Convert.ToDecimal(1.18);
                    account.ОсновнаяСумма = model.СуммаКредита;
                }
                else
                {
                    model.ПроцентнаяСтавка = 26;
                    account.ПроцентнаяСтавка = model.ПроцентнаяСтавка;
                    model.ДатаОкончания = DateTime.Now;
                    model.ДатаОкончания = model.ДатаОкончания.AddMonths(12);
                    account.ДатаОкончания = model.ДатаОкончания;
                    model.СуммаКредита = model.СуммаКредита * Convert.ToDecimal(1.26);
                    account.ОсновнаяСумма = model.СуммаКредита;
                }

                _context.Кредиты.Add(account);
                _context.SaveChanges();
                return RedirectToAction("Profile", new { id = currentUser.ID_Клиента });
            }

            //ModelState.AddModelError("OpenAccount", "Не удалось найти пользователя.");
            return View("OpenCredit", model);
        }


        public IActionResult DeleteCredit(int creditId, int клиентId)
        {
            // tupa = creditId;

            var accountToDelete = _context.Кредиты.FirstOrDefault(a => a.ID_Кредита == creditId && a.Клиент.ID_Клиента == клиентId);

            if (accountToDelete != null)
            {
                //var otherAccounts = _context.Кредиты.Where(a => a.Клиент.ID_Клиента == клиентId && a.ID_Счета != creditId).ToList();

                _context.Кредиты.Remove(accountToDelete);
                _context.SaveChanges();

            }

            return RedirectToAction("Profile", new { id = клиентId });
        }


        [HttpPost]
        public IActionResult UpdateQuantity(int id, int kolvo)
        {
            // Проверяем, что количество положительное
            if ((kolvo <= 0))
            {
                return Json(new { success = false, message = "Количество должно быть больше 0." });
            }

            // Ищем товар в корзине
            var item = _context.Karzina.FirstOrDefault(p => p.Id == id);

            // Проверяем, что товар существует
            if (item == null)
            {
                return Json(new { success = false, message = "Товар не найден." });
            }

            // Обновляем количество
            item.kolv = kolvo;

            // Сохраняем изменения в базе
            _context.SaveChanges();

            // Пересчитываем общую стоимость корзины
            var totalPrice = _context.Karzina.Sum(x => x.Price * x.kolv);

            // Возвращаем успех и обновленную общую стоимость
            return Json(new { success = true, totalPrice });
        }

    }
}

