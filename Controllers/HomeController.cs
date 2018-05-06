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
        public ActionResult Forecast()
        {
            VacationAccrualViewModel vm = new VacationAccrualViewModel();
            return View(vm);
        }
        [HttpPost]
        public ActionResult Forecast(VacationAccrualViewModel vm)
        {
            vm.SetPeriodList(vm.SelectedStartDate, vm.MaxBalance, vm.SelectedPeriods, vm.Accrual, vm.Balance);
                
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
