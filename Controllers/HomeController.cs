using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VacationAccrual.Models;

namespace VacationAccrual.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string StartDate, string Accural, string Balance)
        {
            DateTime startDate = DateTime.Parse(StartDate);
            Double accural = String.IsNullOrWhiteSpace(Accural) ? 0 : Double.Parse(Accural);
            Double balance = String.IsNullOrWhiteSpace(Balance) ? 0 : Double.Parse(Balance);
            
            return View("Index", new PayPeriod(startDate, accural, balance));
        }

        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
