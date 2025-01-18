using Bank.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Bank.Controllers
{
    public class BranchController : Controller
    {
        private readonly BankContext _context;

        public BranchController(BankContext context)
        {
            _context = context;
        }

        public IActionResult Employees(int branchId)
        {
            var employees = _context.Сотрудники
                .Where(e => e.ID_Отделения == branchId)
                .ToList();

            return View(employees);
        }
    }
}

