using Logic.DTOs;

namespace Logic.Interfaces;

public interface IReportCreditCardExpenses
{
    public List<ReportCreditCardExpensesDto> GenerateReportCreditCardExpenses(CreditCardAccountDto creditCardAccountDto, int userId, DateTime date);
}