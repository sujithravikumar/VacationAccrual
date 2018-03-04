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
        public DateTime SelectedStartDate { get; set; }
        public List<SelectListItem> Periods { get; set; }
        [Required]
        public int SelectedPeriods { get; set; }
        [Required]
        public int MaxBalance { get; set; }
        [Required]
        public float Accural { get; set; }
        [Required]
        public float Balance { get; set; }
        public List<PayPeriod> PeriodList { get; set; }

        public VacationAccrualViewModel()
        {
            SetStartDateItems();
            SetPeriods();
            this.MaxBalance = 120;
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
                startDateList.Add(new SelectListItem { Text = weekBegin.ToString("MM-dd-yy") });
                startDateList.Add(new SelectListItem { Text = weekBegin.AddDays(-7).ToString("MM-dd-yy") });
            }
            else
            {
                startDateList.Add(new SelectListItem { Text = weekBegin.AddDays(-7).ToString("MM-dd-yy") });
                startDateList.Add(new SelectListItem { Text = weekBegin.ToString("MM-dd-yy") });
            }

            this.StartDateList = startDateList;
        }

        public void SetPeriodList(DateTime startDate, int periods, float accural, float balance) 
        { 
            List<PayPeriod> periodList = new List<PayPeriod>();

            //Calculate from previous pay period
            startDate = startDate.AddDays(-14);

            for (int i = 0; i < periods; i++)
            {
                periodList.Add(new PayPeriod(startDate.ToString("MM-dd-yy") + " - " + startDate.AddDays(13).ToString("MM-dd-yy"), accural, 0, balance));
                startDate = startDate.AddDays(14);
                balance += accural;
            }
            this.PeriodList = periodList;
        }
    }
}