using System;
namespace vacation_accrual_buddy.Models
{
    public class VacationDataModel
    {
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public decimal Accrual { get; set; }
        public decimal Take { get; set; }
        public decimal Balance { get; set; }
        public decimal Forfeit { get; set; }
    }
}
