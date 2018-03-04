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
        public ActionResult Index(VacationAccrualViewModel vm)
        {
            
            if(vm.SelectedStartDate != default(DateTime))
            {
                vm.SetPeriodList(vm.SelectedStartDate, vm.SelectedPeriods, vm.Accural, vm.Balance);
                
                //Azure Application Insights
                telemetry.TrackEvent("Calculated Vacation Accural");
            }
            return View(vm);
        }

        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
