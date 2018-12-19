namespace vacation_accrual_buddy.Models
{
    public class PayPeriod
    {
        public string Period { get; set; }
        public float Accrual { get; set; }
        public float Take { get; set; }
        public string Balance { get; set; }
        public string Forfeit { get; set; }

        public PayPeriod() { }

        public PayPeriod(string period, float accrual, float take, float balance, float forfeit)
        {
            this.Period = period;
            this.Accrual = accrual;
            this.Take = take;
            this.Balance = balance.ToString("0.00");
            this.Forfeit = forfeit.ToString("0.00");
        }
    }
}
