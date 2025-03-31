using Logic.DTOs;

namespace Logic.Interfaces
{
    public interface IReportIncomeOutcome
    {
        public List<ReportIncomeOutcomeDto> GetIncomeOutcome(DateTime date, int userId);
    }
}
