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
                string userId = _userManager.GetUserId(User);
                if (_userRepository.Exists(userId))
                {

                }
                else
                {
                    return RedirectToAction("Preferences");
                }
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
            string userId = _userManager.GetUserId(User);
            if (_userRepository.Exists(userId))
            {
                UserDataModel userData = _userRepository.Get(userId);
                vm.StartDate = DecodeStartDateEvenWW(userData.Start_Date_Even_Ww);
                vm.Accrual = userData.Accrual;
                vm.MaxBalance = userData.Max_Balance;
                vm.Period = userData.Period;
            }
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
                    EncodeStartDateEvenWW(vm.StartDate),
                    vm.Accrual,
                    vm.MaxBalance,
                    vm.Period
                );
            }
            else
            {
                _userRepository.Update(
                    userId,
                    EncodeStartDateEvenWW(vm.StartDate),
                    vm.Accrual,
                    vm.MaxBalance,
                    vm.Period
                );
            }
            return Content("Done.");
        }

        private bool EncodeStartDateEvenWW(string StartDate)
        {
            GregorianCalendar calendar = new GregorianCalendar();
            int weekNumber = calendar.GetWeekOfYear(
                DateTime.ParseExact(StartDate, "yyyy-MM-dd", null),
                CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            return weekNumber % 2 == 0;
        }

        private string DecodeStartDateEvenWW(bool startDateEvenWW)
        {
            DateTime startDate = DateTime.Now;
            int diff = DayOfWeek.Sunday - startDate.DayOfWeek;
            DateTime weekBegin = startDate.AddDays(diff);

            var calendar = new GregorianCalendar();
            var weekNumber = calendar.GetWeekOfYear(weekBegin, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            int biweeklyKey = weekNumber % 2;

            if (startDateEvenWW)
            {
                if (biweeklyKey == 0)
                {
                    return weekBegin.ToString("yyyy-MM-dd");
                }
                return weekBegin.AddDays(-7).ToString("yyyy-MM-dd");
            }
            else
            {
                if (biweeklyKey == 0)
                {
                    return weekBegin.AddDays(-7).ToString("yyyy-MM-dd");
                }
                return weekBegin.ToString("yyyy-MM-dd");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
