namespace Logic.DTOs
{
    public class ReportIncomeOutcomeDto
    {
        public int Day { get; set; }

        public double Income { get; set; }

        public double Outcome { get; set; }


        public ReportIncomeOutcomeDto(int day, double income, double outcome)
        {
            Day = day;
            Income = income;
            Outcome = outcome;
        }


    }
}
