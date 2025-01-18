using Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Bank.Controllers
{
    public class RepayController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index1()
        {
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }


        private readonly BankContext _context;

        public RepayController(BankContext context)
        {
            _context = context;
        }

        public IActionResult RepayC(int? creditId)
        {


            if (creditId == null)
            {
                return BadRequest("ID кредита обязателен.");
            }

            var account = _context.Кредиты.Find(creditId);
            if (account == null)
            {
                return NotFound("Кредит не найден.");
            }

            var clientAccounts = _context.Счета.Where(a => a.ID_Клиента == account.ID_Клиента).ToList();

            var viewModel = new Repay
            {
                SourceCreditId = account.ID_Кредита,
                ClientAccounts = new SelectList(clientAccounts, "ID_Счета", "НомерСчета")
            };
            if ((account.Статус=="Погашен досрочно")||(account.Статус == "Отклонен"))

            {
                TempData["Error"] = "Вы уже погасили этот кредит";
                return RedirectToAction("Profile","Account", new { id = account.ID_Клиента });

            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RepayC(Repay model)
        {
            if (!ModelState.IsValid)
            {
                //return RedirectToAction("Index", "Account", new { id = model.SourceAccountId });
                var sourceAccount = _context.Кредиты.FirstOrDefault(a => a.ID_Кредита == model.SourceCreditId);
                var destinationAccount = _context.Счета.FirstOrDefault(a => a.ID_Счета == model.DestinationAccountId);

                //if (model.SourceCreditId == model.DestinationAccountId)
               // {
              //      return RedirectToAction("Index3");

                //}
                if(destinationAccount.Баланс< model.Amount)
                {
                    return RedirectToAction("Index2");
                }
                if (model.Amount <= 0)
                {
                    return RedirectToAction("Index");
                }

              //  if (model.Amount > destinationAccount.Баланс && model.TransactionType == "Deposit")
               // {
                //    return RedirectToAction("Index2");

               // }
                //if (sourceAccount == null || destinationAccount == null)
               // {
                //    return NotFound("Счет не найден.");
               // }

                if (sourceAccount.ОсновнаяСумма < model.Amount)
                {
                    return RedirectToAction("Index1");              
                }


                    sourceAccount.ОсновнаяСумма -= model.Amount;
                    destinationAccount.Баланс -= model.Amount;
                

                var transaction = new ПлатежПоКредиту
                {
                    ID_Кредита = model.SourceCreditId,
                    ДатаПлатежа = DateTime.Now,
                    //ТипТранзакции = model.TransactionType,
                    СуммаПлатежа = model.Amount,
                   // Описание = model.Description
                };
                if (sourceAccount.ОсновнаяСумма == 0)
                {
                    sourceAccount.Статус = "Погашен досрочно";
                    sourceAccount.ДатаОкончания = DateTime.Now;
                }
                _context.ПлатежиПоКредитам.Add(transaction);
                await _context.SaveChangesAsync();

                return RedirectToAction("Profile", "Account", new { id = sourceAccount.ID_Клиента });
            }
            return RedirectToAction("Login", "Account");
            //Console.WriteLine("ModelState.IsValid         000000000000000000000000000000000000000000000000000000000000000\n");
            // Повторно заполняем ClientAccounts
           // var clientAccounts = _context.Счета.Where(a => a.ID_Клиента == model.SourceAccountId).ToList();
          //  model.ClientAccounts = new SelectList(clientAccounts, "ID_Счета", "НомерСчета");

            //return View(model);
        }



    }
}
