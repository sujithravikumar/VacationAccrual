namespace vacation_accrual_buddy.Models
{
    public class PayPeriod
    {
        public string Period { get; set; }
        public decimal Accrual { get; set; }
        public decimal Take { get; set; }
        public string Balance { get; set; }
        public string Forfeit { get; set; }

        public PayPeriod() { }

        public PayPeriod(string period, decimal accrual, decimal take, decimal balance, decimal forfeit)
        {
            this.Period = period;
            this.Accrual = accrual;
            this.Take = take;
            this.Balance = balance.ToString("0.00");
            this.Forfeit = forfeit.ToString("0.00");
        }
    }
}
