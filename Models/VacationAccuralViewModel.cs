using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VacationAccrual.Models
{
    public class VacationAccrualViewModel
    {
        public List<SelectListItem> StartDateList { get; set; }
        [Required]
        public string SelectedStartDate { get; set; }
        public List<SelectListItem> Periods { get; set; }
        [Required]
        public int SelectedPeriods { get; set; }
        [Required]
        public int MaxBalance { get; set; }
        [Required]
        public float Accrual { get; set; }
        [Required]
        public float Balance { get; set; }
        public List<SelectListItem> DaysOff { get; set; }
        public int SelectedDaysOff { get; set; }
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
            periodsList.Add(new SelectListItem { Text = "4"});
            periodsList.Add(new SelectListItem { Text = "8"});
            periodsList.Add(new SelectListItem { Text = "12"});
            periodsList.Add(new SelectListItem { Text = "16"});
            periodsList.Add(new SelectListItem { Text = "20"});
            this.Periods = periodsList;
            this.SelectedPeriods = 8;
        }

        public void SetDaysOff() 
        {
            List<SelectListItem> daysOff = new List<SelectListItem>();
            daysOff.Add(new SelectListItem { Text = "0"});
            daysOff.Add(new SelectListItem { Text = "1"});
            daysOff.Add(new SelectListItem { Text = "2"});
            this.DaysOff = daysOff;
            this.SelectedDaysOff = 1;
        }

        public void SetStartDateItems()
        {
            List<SelectListItem> startDateList = new List<SelectListItem>();
            DateTime startDate = DateTime.Now;

            int diff = DayOfWeek.Sunday - startDate.DayOfWeek;
            DateTime weekBegin = startDate.AddDays(diff);

            for (int i=0; i<6; i++)
            {
                startDateList.Add(new SelectListItem { Text = weekBegin.AddDays(-i*7).ToString("MM/dd/yy") });
            }

            this.StartDateList = startDateList;

            var calendar = new GregorianCalendar();
            var weekNumber = calendar.GetWeekOfYear(weekBegin, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            int biweeklyKey = weekNumber % 2;

            if (biweeklyKey == 0)
            {
                this.SelectedStartDate = weekBegin.ToString("MM/dd/yy");
            }
            else
            {
                this.SelectedStartDate = weekBegin.AddDays(-7).ToString("MM/dd/yy");
            }
        }

        public void SetPeriodList(string selectedStartDate, float maxBalance, int periods, float accrual, float balance) 
        { 
            List<PayPeriod> periodList = new List<PayPeriod>();
            float take, forfeit = 0;

            //Calculate from previous pay period
            DateTime startDate = DateTime.Parse(selectedStartDate).AddDays(-14);

            for (int i = 0; i < periods; i++)
            {
                take = 0;
                if (balance > maxBalance)
                {
                    take = 8;
                    balance -= take;
                }
                periodList.Add(new PayPeriod(startDate.ToString("MM/dd/yy") + " - " + startDate.AddDays(13).ToString("MM/dd/yy"), accrual, take, balance, forfeit));
                startDate = startDate.AddDays(14);
                balance += accrual;
            }
            this.PeriodList = periodList;
        }
    }
}