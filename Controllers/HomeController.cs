using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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
        private readonly IVacationRepository _vacationRepository;

        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUserRepository userRepository,
            IVacationRepository vacationRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _vacationRepository = vacationRepository;
        }

        [HttpGet]
        public IActionResult Index(VacationAccrualViewModel vm)
        {
            if (_signInManager.IsSignedIn(User))
            {
                string userId = _userManager.GetUserId(User);
                if (_userRepository.Exists(userId))
                {
                    UserDataModel userData = _userRepository.Get(userId);
                    vm.StartDate = DecodeStartDateEvenWW(userData.Start_Date_Even_Ww);
                    vm.Accrual = userData.Accrual;
                    vm.MaxBalance = userData.Max_Balance;
                    vm.Period = userData.Period + 1; // to account for the previous period extra row
                    vm.DaysOff = userData.Take_Days_Off % 1 == 0 ?
                                Convert.ToInt32(userData.Take_Days_Off).ToString() :
                                userData.Take_Days_Off.ToString();

                    vm.PeriodList = _vacationRepository.Get(
                        userId,
                        DateTime.Parse(vm.StartDate).AddDays(-14),
                        vm.Period
                    );

                    if (vm.PeriodList.Count < vm.Period)
                    {
                        string startDate = DateTime.Parse(vm.StartDate).AddDays(14 * (vm.PeriodList.Count - 1)).ToString();
                        decimal balance = Convert.ToDecimal(vm.PeriodList.Last().Balance);
                        decimal daysOff = Convert.ToDecimal(vm.DaysOff);
                        vm.AppendPeriodList(vm.PeriodList, startDate, vm.MaxBalance, vm.Period - vm.PeriodList.Count, vm.Accrual, balance, daysOff, true);
                    }
                    return View(vm);
                }
                return RedirectToAction("Preferences");
            }
            return View(vm);
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult Submit(VacationAccrualViewModel vm)
        {
            vm.AppendPeriodList(new List<PayPeriod>(), vm.StartDate, vm.MaxBalance, vm.Period, vm.Accrual, vm.Balance);
            return View("Index", vm);
        }

        [HttpPost]
        public IActionResult SaveForecastData(VacationAccrualViewModel vm)
        {
            string userId = _userManager.GetUserId(User);
            DateTime startDate, endDate;
            decimal accrual, take, balance, forfeit;

            _vacationRepository.Delete(userId);

            for (int i = 0; i < vm.PeriodList.Count; i++)
            {
                startDate = DateTime.Parse(vm.PeriodList[i].Period
                    .Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim());
                endDate = DateTime.Parse(vm.PeriodList[i].Period
                    .Split(new string[] { " - " }, StringSplitOptions.None)[1].Trim());
                accrual = vm.PeriodList[i].Accrual;
                take = vm.PeriodList[i].Take;
                balance = Convert.ToDecimal(vm.PeriodList[i].Balance);
                forfeit = Convert.ToDecimal(vm.PeriodList[i].Forfeit);

                if (!_vacationRepository.Exists(
                        userId,
                        startDate,
                        endDate))
                {
                    _vacationRepository.Insert(
                        userId,
                        startDate,
                        endDate,
                        accrual,
                        take,
                        balance,
                        forfeit
                    );
                }
                else
                {
                    _vacationRepository.Update(
                        userId,
                        startDate,
                        endDate,
                        take,
                        balance,
                        forfeit
                    );
                }
            }
            return Content("Saved!");
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
                vm.DaysOff = userData.Take_Days_Off % 1 == 0 ?
                            Convert.ToInt32(userData.Take_Days_Off).ToString() :
                            userData.Take_Days_Off.ToString();

                List<PayPeriod> vacationData = _vacationRepository.Get(
                    userId,
                    DateTime.Parse(vm.StartDate).AddDays(-14),
                    1
                );

                vm.Balance = Convert.ToDecimal(vacationData.Single().Balance);
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
                    vm.Period,
                    Convert.ToDecimal(vm.DaysOff)
                );

                _vacationRepository.Insert(
                    userId,
                    DateTime.Parse(vm.StartDate).AddDays(-14),
                    DateTime.Parse(vm.StartDate).AddDays(-1),
                    vm.Accrual,
                    0,
                    vm.Balance,
                    0
                );
            }
            else
            {
                UserDataModel userData = _userRepository.Get(userId);

                List<PayPeriod> vacationData = _vacationRepository.Get(
                    userId,
                    DateTime.Parse(vm.StartDate).AddDays(-14),
                    1
                );

                _userRepository.Update(
                    userId,
                    EncodeStartDateEvenWW(vm.StartDate),
                    vm.Accrual,
                    vm.MaxBalance,
                    vm.Period,
                    Convert.ToDecimal(vm.DaysOff)
                );

                if (userData.Start_Date_Even_Ww != EncodeStartDateEvenWW(vm.StartDate) ||
                    userData.Accrual != vm.Accrual ||
                    userData.Max_Balance != vm.MaxBalance ||
                    userData.Take_Days_Off != Convert.ToDecimal(vm.DaysOff) ||
                    Convert.ToDecimal(vacationData.Single().Balance) != vm.Balance)
                {
                    _vacationRepository.Delete(userId);

                    _vacationRepository.Insert(
                        userId,
                        DateTime.Parse(vm.StartDate).AddDays(-14),
                        DateTime.Parse(vm.StartDate).AddDays(-1),
                        vm.Accrual,
                        0,
                        vm.Balance,
                        0
                    );
                }
            }
            return Content("Saved!");
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
