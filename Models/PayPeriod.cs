using System;
using System.Collections.Generic;

namespace VacationAccrual.Models
{
    public class PayPeriod
    {
        public string Period { get; set; }
        public float Accural { get; set; }
        public float Take { get; set; }
        public string Balance { get; set; }

        public PayPeriod(string period, float accural, float take, float balance)
        {
            this.Period = period;
            this.Accural = accural;
            this.Take = take;
            this.Balance = balance.ToString("0.00");
        }
    }
}
