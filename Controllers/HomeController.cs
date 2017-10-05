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
        public ActionResult Index(PayPeriodList payPeriod)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Command, string StartDate, string Accural, string Balance, PayPeriodList payPeriod)
        {
            DateTime startDate = DateTime.Parse(StartDate);
            Double accural = String.IsNullOrWhiteSpace(Accural) ? 0 : Double.Parse(Accural);
            Double balance = String.IsNullOrWhiteSpace(Balance) ? 0 : Double.Parse(Balance);
            
            List<PayPeriod> periodList = new List<PayPeriod>();

            for (int i = 0; i < 10; i++)
            {
                periodList.Add(new PayPeriod(startDate.ToString("MM-dd-yy") + " - " + startDate.AddDays(13).ToString("MM-dd-yy"), accural, 0.0, balance));
                startDate = startDate.AddDays(14);
                balance += accural;
			}

            PayPeriodList objPayPeriod = new PayPeriodList();
            objPayPeriod.PayPeriods = periodList;

            return View("Index", objPayPeriod);
        }

        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
