using Logic.DTOs;

namespace Logic.Interfaces;

public interface IReportGeneralAccountBalance
{
    public List<ReportGeneralAccountBalanceDto> GenerateReportGeneralAccountBalance(GeneralAccountDto generalAccountDto, int userId);
}