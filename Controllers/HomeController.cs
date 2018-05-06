using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VacationAccrual.Models;
using Microsoft.ApplicationInsights;

namespace VacationAccrual.Controllers
{
    public class HomeController : Controller
    {
        private TelemetryClient telemetry = new TelemetryClient();
        [HttpGet]
        public ActionResult Forecast(VacationAccrualViewModel vm)
        {
            return View(vm);
        }
        [HttpPost]
        [ActionName("Forecast")]
        public ActionResult ForecastCalculate(VacationAccrualViewModel vm)
        {
            vm.SetPeriodList(vm.StartDate, vm.MaxBalance, vm.Period, vm.Accrual, vm.Balance);
                
            //Azure Application Insights
            telemetry.TrackEvent("Calculated Vacation Accrual");

            return View(vm);
        }

        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
