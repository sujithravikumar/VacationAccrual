namespace vacation_accrual_buddy.Models
{
    public class UserDataModel
    {
        public bool Start_Date_Even_Ww { get; set; }
        public decimal Accrual { get; set; }
        public int Max_Balance { get; set; }
        public int Period { get; set; }
        public decimal Take_Days_Off { get; set; }
        public bool Email_Alert_Enabled { get; set; }
        public int Email_Alert_Days_Before { get; set; }
    }
}
