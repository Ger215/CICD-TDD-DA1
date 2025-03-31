using Logic.DTOs;

namespace Logic.Interfaces;

public interface IReportByCategory
{
    public List<ReportGoalByCategoryDto> GenerateReportByCategories(int userId, DateTime month);
}