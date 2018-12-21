using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vacation_accrual_buddy.Models
{
    public class VacationAccrualViewModel
    {
        public List<SelectListItem> StartDateList { get; set; }
        [Required]
        public string StartDate { get; set; }
        public List<SelectListItem> Periods { get; set; }
        [Required]
        public int Period { get; set; }
        [Required]
        public int MaxBalance { get; set; }
        [Required]
        public decimal Accrual { get; set; }
        [Required]
        public decimal Balance { get; set; }
        public List<SelectListItem> DaysOffList { get; set; }
        public int DaysOff { get; set; }
        public List<PayPeriod> PeriodList { get; set; }

        public VacationAccrualViewModel()
        {
            SetStartDateItems();
            SetPeriods();
            SetDaysOff();
            this.MaxBalance = 120;
            this.Accrual = 6;
            this.Balance = 100;
        }

        public void SetPeriods()
        {
            List<SelectListItem> periodsList = new List<SelectListItem>();
            periodsList.Add(new SelectListItem { Text = "4" });
            periodsList.Add(new SelectListItem { Text = "8" });
            periodsList.Add(new SelectListItem { Text = "12" });
            periodsList.Add(new SelectListItem { Text = "16" });
            periodsList.Add(new SelectListItem { Text = "20" });
            this.Periods = periodsList;
            this.Period = 8;
        }

        public void SetDaysOff()
        {
            List<SelectListItem> daysOffList = new List<SelectListItem>();
            daysOffList.Add(new SelectListItem { Text = "0" });
            daysOffList.Add(new SelectListItem { Text = "0.5" });
            daysOffList.Add(new SelectListItem { Text = "1" });
            daysOffList.Add(new SelectListItem { Text = "2" });
            this.DaysOffList = daysOffList;
            this.DaysOff = 1;
        }

        public void SetStartDateItems()
        {
            List<SelectListItem> startDateList = new List<SelectListItem>();
            DateTime startDate = DateTime.Now;

            int diff = DayOfWeek.Sunday - startDate.DayOfWeek;
            DateTime weekBegin = startDate.AddDays(diff);

            for (int i = 1; i >= 0; i--)
            {
                startDateList.Add(new SelectListItem { Text = weekBegin.AddDays(-i * 7).ToString("yyyy-MM-dd") });
            }

            this.StartDateList = startDateList;

            var calendar = new GregorianCalendar();
            var weekNumber = calendar.GetWeekOfYear(weekBegin, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            int biweeklyKey = weekNumber % 2;

            if (biweeklyKey == 0)
            {
                this.StartDate = weekBegin.ToString("yyyy-MM-dd");
            }
            else
            {
                this.StartDate = weekBegin.AddDays(-7).ToString("yyyy-MM-dd");
            }
        }

        public void SetPeriodList(string selectedStartDate, int maxBalance, int periods, decimal accrual, decimal balance)
        {
            List<PayPeriod> periodList = new List<PayPeriod>();
            decimal take, forfeit = 0;

            //Calculate from previous pay period
            DateTime startDate = DateTime.Parse(selectedStartDate).AddDays(-14);

            for (int i = 0; i < periods; i++)
            {
                take = 0;
                if (balance > maxBalance)
                {
                    take = (8 * this.DaysOff);
                    balance -= take;
                    if (balance > maxBalance)
                    {
                        forfeit += balance - maxBalance;
                        balance = maxBalance;
                    }
                }
                periodList.Add(new PayPeriod(startDate.ToString("yyyy-MM-dd") + " - " + startDate.AddDays(13).ToString("yyyy-MM-dd"), accrual, take, balance, forfeit));
                startDate = startDate.AddDays(14);
                balance += accrual;
            }
            this.PeriodList = periodList;
        }
    }
}
