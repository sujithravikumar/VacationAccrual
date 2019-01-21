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
        public string DaysOff { get; set; }
        public List<PayPeriod> PeriodList { get; set; }
        public bool ReceiveEmailReminder { get; set; }
        public List<SelectListItem> ReceiveDaysBeforeList { get; set; }
        public int ReceiveDaysBefore { get; set; }

        public VacationAccrualViewModel()
        {
            SetStartDateItems();
            SetPeriods();
            SetDaysOff();
            SetReceiveDaysBefore();
            MaxBalance = 120;
            Accrual = 6;
            Balance = 100;
        }

        public void SetPeriods()
        {
            List<SelectListItem> periodsList = new List<SelectListItem>();
            periodsList.Add(new SelectListItem { Text = "4" });
            periodsList.Add(new SelectListItem { Text = "8" });
            periodsList.Add(new SelectListItem { Text = "12" });
            periodsList.Add(new SelectListItem { Text = "16" });
            periodsList.Add(new SelectListItem { Text = "20" });
            Periods = periodsList;
            Period = 8;
        }

        public void SetDaysOff()
        {
            List<SelectListItem> daysOffList = new List<SelectListItem>();
            daysOffList.Add(new SelectListItem { Text = "0" });
            daysOffList.Add(new SelectListItem { Text = "0.5" });
            daysOffList.Add(new SelectListItem { Text = "1" });
            daysOffList.Add(new SelectListItem { Text = "2" });
            DaysOffList = daysOffList;
            DaysOff = "1";
        }

        public void SetReceiveDaysBefore()
        {
            List<SelectListItem> receiveDaysBeforeList = new List<SelectListItem>();
            for (int i=1; i<14; i++)
            {
                receiveDaysBeforeList.Add(new SelectListItem { Text = $"{i}" });
            }
            ReceiveDaysBeforeList = receiveDaysBeforeList;
            ReceiveDaysBefore = 1;
        }

        public void SetStartDateItems()
        {
            List<SelectListItem> startDateList = new List<SelectListItem>();
            DateTime startDate = DateTime.Now;

            int diff = DayOfWeek.Sunday - startDate.DayOfWeek;
            DateTime weekBegin = startDate.AddDays(diff);

            var calendar = new GregorianCalendar();
            var weekNumber = calendar.GetWeekOfYear(weekBegin, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            int biweeklyKey = weekNumber % 2;

            if (biweeklyKey == 0)
            {
                startDateList.Add(new SelectListItem { Text = weekBegin.ToString("yyyy-MM-dd") });
            }
            else
            {
                startDateList.Add(new SelectListItem { Text = weekBegin.AddDays(-7).ToString("yyyy-MM-dd") });
            }
            StartDateList = startDateList;
        }

        public void AppendPeriodList(List<PayPeriod> periodList, string selectedStartDate, int maxBalance, int periods, decimal accrual, decimal balance, decimal daysOff = 1, bool ignorePreviousPeriod = false)
        {
            if (periodList == null)
            {
                throw new Exception("ERROR: PeriodList cannot be null");
            }
            decimal take = 0, forfeit = 0;
            DateTime startDate;

            if (ignorePreviousPeriod)
            {
                startDate = DateTime.Parse(selectedStartDate);
                balance += accrual;
            }
            else
            {
                //Calculate from previous pay period
                startDate = DateTime.Parse(selectedStartDate).AddDays(-14);
                if (balance > maxBalance)
                {
                    forfeit = balance - maxBalance;
                    balance = maxBalance;
                }
                periodList.Add(new PayPeriod(startDate.ToString("yyyy-MM-dd") + " - " + startDate.AddDays(13).ToString("yyyy-MM-dd"), accrual, take, balance, forfeit));
                startDate = startDate.AddDays(14);
                balance += accrual;
            }

            for (int i = 0; i < periods; i++)
            {
                take = 0;
                forfeit = 0;
                if (balance > maxBalance)
                {
                    take = 8 * daysOff;
                    // to get rid of decimal point .0 if it is a whole number
                    take = take % 1 == 0 ?
                            Convert.ToInt32(take) :
                            take;
                    balance -= take;
                    if (balance > maxBalance)
                    {
                        forfeit = balance - maxBalance;
                        balance = maxBalance;
                    }
                }
                periodList.Add(new PayPeriod(startDate.ToString("yyyy-MM-dd") + " - " + startDate.AddDays(13).ToString("yyyy-MM-dd"), accrual, take, balance, forfeit));
                startDate = startDate.AddDays(14);
                balance += accrual;
            }
            PeriodList = periodList;
        }
    }
}
