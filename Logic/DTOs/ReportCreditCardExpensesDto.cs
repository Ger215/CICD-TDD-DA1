using Models;

namespace Logic.DTOs
{
    public class ReportCreditCardExpensesDto
    {
        public double TotalSpent { get; set; }
        public CreditCardAccount CreditCardAccount { get; set; }
        
        public ReportCreditCardExpensesDto() { }

        public ReportCreditCardExpensesDto(CreditCardAccount account, double totalSpent)
        {
            CreditCardAccount = account;
            TotalSpent = totalSpent;
        }
    }
} 