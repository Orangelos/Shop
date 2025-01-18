using Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank;
namespace Bank.Controllers
{


    public class HomeController : Controller
    {

        private readonly BankContext _context;

        public HomeController(BankContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Получаем список баннеров из БД
            var banners = _context.Banner.ToList();

            // Передаем их в представление
            return View(banners);
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }
        public IActionResult A1()
        {
            return View();
        }
        public IActionResult A2()
        {
            return View();
        }
        public IActionResult A3()
        {
            return View();
        }

        public IActionResult Uslov()
        {
            return View();
        }



    }










    // private readonly ILogger<HomeController> _logger;
    //
    // public HomeController(ILogger<HomeController> logger)
    // {
    //     _logger = logger;
    // }
    //
    // public IActionResult Index()
    // {
    //     
    //
    //     return View();
    // }
    //
}

      

