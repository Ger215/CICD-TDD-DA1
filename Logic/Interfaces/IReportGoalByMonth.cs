using Logic.DTOs;

namespace Logic.Interfaces
{
    public interface IReportGoalByMonth
    {
        public List<ReportGoalByMonthDto> GenerateReportGoalsByMonth(int userId, DateTime date);
    }
}
