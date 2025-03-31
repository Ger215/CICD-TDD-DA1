using Models.Enums;

namespace Logic.DTOs
{
    public class ReportGeneralAccountBalanceDto
    {
        public string GeneralAccountName { get; set; }
        
        public double Balance { get; set; }
        
        public Currency AccountCurrency { get; set; }
        
        public ReportGeneralAccountBalanceDto() { }

        public ReportGeneralAccountBalanceDto(string generalAccountName, double balance, Currency currency)
        {
            GeneralAccountName = generalAccountName;
            Balance = balance;
            AccountCurrency = currency;
        }
    }
} 