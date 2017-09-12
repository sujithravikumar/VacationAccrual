using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationAccrual.Models;

namespace VacationAccrual.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ListofPeriods = PayPeriod.GetPeriodList(DateTime.Parse("08/15/2017"), 4.62, 10);
            return View();
        }

        [HttpPost]
        public IActionResult Result()
        {
            ViewBag.ListofPeriods = PayPeriod.GetPeriodList(DateTime.Parse("09/11/2017"), 5.55, 5);
            return View("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
