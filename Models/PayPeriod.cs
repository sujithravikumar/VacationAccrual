using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            for (int i = 0; i < count; i++)
            {
                periodList.Add(new PayPeriod(startDate.ToString("MM-dd-yy") + " - " + startDate.AddDays(11).ToString("MM-dd-yy"), accural, 0.0, 0.0));
                startDate = startDate.AddDays(14);
			}

            return periodList;
        }
    }
}
