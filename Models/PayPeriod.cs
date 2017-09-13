using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VacationAccrual.Models
{
    public class PayPeriod
    {
        public string Period { get; set; }
        public double Accural { get; set; }
        public double Take { get; set; }
        public double Balance { get; set; }
        public string StartDate { get; set;}

        public PayPeriod(string period, double accural, double take, double balance)
        {
            this.Period = period;
            this.Accural = accural;
            this.Take = take;
            this.Balance = balance;
        }

        public static List<SelectListItem> GetStartDateList()
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
            
            return startDateList;
        } 

        public static List<PayPeriod> GetPeriodList(DateTime startDate, double accural, double balance, int count)
        {
            List<PayPeriod> periodList = new List<PayPeriod>();

            DateTime weekBegin = startDate;

            for (int i = 0; i < count; i++)
            {
                periodList.Add(new PayPeriod(weekBegin.ToString("MM-dd-yy") + " - " + weekBegin.AddDays(13).ToString("MM-dd-yy"), accural, 0.0, balance));
                weekBegin = weekBegin.AddDays(14);
                balance += accural;
			}

            return periodList;
        }
    }
}
