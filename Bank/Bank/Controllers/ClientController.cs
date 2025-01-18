using Bank.Models;
using Microsoft.AspNetCore.Identity;

//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;
namespace Bank.Controllers
{
    public class ClientController : Controller
    {
        private readonly BankContext _context;
        private readonly IPasswordHasher<Клиент> _passwordHasher;

        public ClientController(BankContext context, IPasswordHasher<Клиент> passwordHasher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public IActionResult Register()
        {
            var model = new ViewVerify
            {
                
            };

            return View(model);
        }


        int kod;

        [HttpPost]
        public IActionResult Register(ViewVerify model)
        {
           

            try
            {
                // Проверка на существование клиента с той же почтой или телефоном
                var existingClient = _context.Клиенты
                    .FirstOrDefault(c => c.ЭлектроннаяПочта == model.ЭлектроннаяПочта || c.PhoneNumber == model.PhoneNumber);

                if (existingClient != null)
                {
                    // Если почта уже используется
                    if (existingClient.ЭлектроннаяПочта == model.ЭлектроннаяПочта)
                    {
                        ModelState.AddModelError("ЭлектроннаяПочта", "Электронная почта уже используется.");
                    }

                    // Если телефон уже используется
                    if (existingClient.PhoneNumber == model.PhoneNumber)
                    {
                        ModelState.AddModelError("PhoneNumber", "Номер телефона уже используется.");
                    }

                    return View(model); // Возврат модели с ошибками
                }

                // Создание нового клиента
                var клиент = new Клиент
                {
                    Имя = model.Имя,
                    Фамилия = model.Фамилия,
                    ДатаРождения = model.ДатаРождения,
                    ЭлектроннаяПочта = model.ЭлектроннаяПочта,
                  
                    PhoneNumber = model.PhoneNumber,
                    Пароль = _passwordHasher.HashPassword(null, model.Пароль) // Хэширование пароля
                };

                // Сохранение данных в базу
                _context.Клиенты.Add(клиент);
                _context.SaveChanges();







                return RedirectToAction("EndRegister", "Client"); // Редирект в случае успешной регистрации
            }
            catch (Exception ex)
            {
                // Обработка исключений
                ModelState.AddModelError(string.Empty, $"Ошибка при добавлении клиента: {ex.Message}");
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult FeedBack()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(FeedBack feedback)
        {
            if (ModelState.IsValid)
            {
                // Если модель валидна, сохраняем данные
                var f = new FeedBack
                {
                    Имя = feedback.Имя,
                    Фамилия = feedback.Фамилия,
                    ЭлектроннаяПочта = feedback.ЭлектроннаяПочта,
                    Otziv = feedback.Otziv,
                };

                _context.FeedBack.Add(f); // Используем _context (DI)
                _context.SaveChanges();

                // Сообщение об успешной отправке
                TempData["Message"] = "Спасибо за ваш отзыв!";
                return RedirectToAction("FeedBack"); // Укажите правильное действие
            }

            // Если данные недействительны, возвращаем введённую информацию
            return View("FeedBack", feedback); // Укажите правильное View
        }
        public IActionResult Catalog(string searchQuery = null)
        {
            // Получаем список товаров из базы данных
            var products = _context.Product.AsQueryable();

            // Фильтруем по запросу
            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p => p.BrandName.Contains(searchQuery) ||
                                                p.Description.Contains(searchQuery) ||
                                                p.Kategory.Contains(searchQuery));
            }

            // Преобразуем результат
            var productList = products.Select(p => new
            {
                p.Id_Product,
                p.BrandName,
                p.ImageUrl,
                p.Price,
                p.Description,
                p.Kategory
            }).ToList();

            // Передаем параметры в представление
            ViewBag.SearchQuery = searchQuery; // передаем строку поиска
            return View(productList);
        }


        public IActionResult EndRegister()
        {
            return View();
        }








        private static List<Karzina> cart = new(); // Хранилище корзины

        public IActionResult Index2()
        {
            return RedirectToAction("Index");
        }
        // Метод: Отображение корзины
        public IActionResult Index()
        {
           
            decimal totalPrice = cart.Sum(p => p.Price);
            ViewBag.TotalPrice = totalPrice;
            return View(cart);
            

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
                item.Id = newItem.Id;
                cart.Add(item);

                // Обновляем количество предметов
                int count = cart.Count;
                return Json(new { count });
            }

            return BadRequest(new { message = "Ошибка добавления товара в корзину" });
        }

        // Метод: Удалить товар из корзины
        public IActionResult RemoveFromCart(int id)
        {
            var item = cart.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                cart.Remove(item);

            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult GetCartCount()
        {
            int count = cart.Count;
            return Json(new { count });
        }





      












    }







}




