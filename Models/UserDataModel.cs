namespace vacation_accrual_buddy.Models
{
    public class UserDataModel
    {
        public bool Start_Date_Even_Ww { get; set; }
        public decimal Accrual { get; set; }
        public int Max_Balance { get; set; }
        public int Period { get; set; }
    }
}
