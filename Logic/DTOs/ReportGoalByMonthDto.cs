using Models;

namespace Logic.DTOs
{
    public class ReportGoalByMonthDto
    {
        public double AmountSpent { get; set; }
        public bool Achieved { get; set; }
        public SpendingGoal SpendingGoalAssociated { get; set; }

        public DateTime Date { get; set; }


        public ReportGoalByMonthDto() { }

        public ReportGoalByMonthDto(SpendingGoal goal, double amount, bool achieved, DateTime date)
        {
            AmountSpent = amount;
            Achieved = achieved;
            SpendingGoalAssociated = goal;
            Date = date;
        }
    }
}
