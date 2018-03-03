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
            VacationAccrualViewModel vm = new VacationAccrualViewModel();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(VacationAccrualViewModel vm)
        {
            vm.SetPeriodList(vm.SelectedStartDate, vm.SelectedPeriods, vm.Accural, vm.CurrentBalance);
            return View("Index", vm);
        }

        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
