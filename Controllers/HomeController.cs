using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vacation_accrual_buddy.Models;

namespace vacation_accrual_buddy.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index(VacationAccrualViewModel vm)
        {
            return View(vm);
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult Submit(VacationAccrualViewModel vm)
        {
            vm.SetPeriodList(vm.StartDate, vm.MaxBalance, vm.Period, vm.Accrual, vm.Balance);
            return View("Index", vm);
        }

        [Authorize]
        public IActionResult Preferences()
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
