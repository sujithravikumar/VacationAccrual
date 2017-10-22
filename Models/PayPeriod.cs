using System;
using System.Collections.Generic;

namespace VacationAccrual.Models
{
    public class PayPeriod
    {
        public string Period { get; set; }
        public double Accural { get; set; }
        public double Take { get; set; }
        public double Balance { get; set; }
        public List<PayPeriod> PeriodList { get; set; }

        public PayPeriod(DateTime startDate, Double accural, Double balance) 
        { 
            List<PayPeriod> periodList = new List<PayPeriod>();

            for (int i = 0; i < 10; i++)
            {
                periodList.Add(new PayPeriod(startDate.ToString("MM-dd-yy") + " - " + startDate.AddDays(13).ToString("MM-dd-yy"), accural, 0.0, balance));
                startDate = startDate.AddDays(14);
                balance += accural;
            }
            this.PeriodList = periodList;
        }

        public PayPeriod(string period, double accural, double take, double balance)
        {
            this.Period = period;
            this.Accural = accural;
            this.Take = take;
            this.Balance = balance;
        }
    }
}
