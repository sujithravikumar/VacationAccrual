using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace VacationAccrual.Models
{
    public class PayPeriod
    {
        public string Period { get; set; }
        public double Accural { get; set; }
        public double Take { get; set; }
        public double Balance { get; set; }

        public PayPeriod(string period, double accural, double take, double balance)
        {
            this.Period = period;
            this.Accural = accural;
            this.Take = take;
            this.Balance = balance;
        }

        PayPeriod() { }

        public static List<PayPeriod> GetPeriodList(DateTime startDate, double accural, int count)
        {
            List<PayPeriod> periodList = new List<PayPeriod>();

            int diff = DayOfWeek.Sunday - startDate.DayOfWeek;
            DateTime weekBegin = startDate.AddDays(diff);

            var calendar = new GregorianCalendar();
            var weekNumber = calendar.GetWeekOfYear(weekBegin, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            int biweeklyKey = weekNumber % 2;

            if (biweeklyKey == 0)
            {
                weekBegin = weekBegin.AddDays(-7);
            }

            for (int i = 0; i < count; i++)
            {
                periodList.Add(new PayPeriod(weekBegin.ToString("MM-dd-yy") + " - " + weekBegin.AddDays(13).ToString("MM-dd-yy"), accural, 0.0, 0.0));
                weekBegin = weekBegin.AddDays(14);
			}

            return periodList;
        }
    }
}
