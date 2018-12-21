using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using vacation_accrual_buddy.Models;
using vacation_accrual_buddy.Repositories;

namespace vacation_accrual_buddy.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepository;

        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index(VacationAccrualViewModel vm)
        {
            if (_signInManager.IsSignedIn(User))
            {
                // TODO if user preferences record exists, then
                // fetch preferences values and redirect to Submit

                // else
                return RedirectToAction("Preferences");
            }
            return View(vm);
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult Submit(VacationAccrualViewModel vm)
        {
            vm.SetPeriodList(vm.StartDate, vm.MaxBalance, vm.Period, vm.Accrual, vm.Balance);
            return View("Index", vm);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Preferences(VacationAccrualViewModel vm)
        {
            // TODO if user preferences record exists, then
            // fetch preferences values

            return View(vm);
        }

        [HttpPost]
        public IActionResult SavePreferences(VacationAccrualViewModel vm)
        {
            string userId = _userManager.GetUserId(User);
            if(!_userRepository.Exists(userId))
            {
                _userRepository.Insert(
                    userId,
                    StartDateEvenWW(vm.StartDate),
                    vm.Accrual,
                    vm.MaxBalance,
                    vm.Period
                );
            }
            else
            {
                // TODO Update the record
            }
            return Content("Done.");
        }

        private bool StartDateEvenWW(string StartDate)
        {
            GregorianCalendar calendar = new GregorianCalendar();
            int weekNumber = calendar.GetWeekOfYear(
                DateTime.ParseExact(StartDate, "yyyy-MM-dd", null),
                CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            return weekNumber % 2 == 0;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
