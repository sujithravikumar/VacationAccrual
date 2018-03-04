using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VacationAccrual.Models;

namespace VacationAccrual.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(VacationAccrualViewModel vm)
        {
            if(vm.SelectedStartDate != default(DateTime))
            {
                vm.SetPeriodList(vm.SelectedStartDate, vm.SelectedPeriods, vm.Accural, vm.Balance);
            }
            return View(vm);
        }

        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
