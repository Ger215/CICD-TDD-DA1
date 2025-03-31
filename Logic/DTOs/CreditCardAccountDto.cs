using Models.Enums;

namespace Logic.DTOs;

public class CreditCardAccountDto
{
    public DateTime ClosingDate { get; set; }

    public double AvailableBalance { get; set; }

    public string LastFourDigits { get; set; }

    public string IssuingBank { get; set; }

    public int Id { get; set; }

    public Currency AccountCurrency { get; set; }

    public DateTime CreationDate { get; set; }
    public CreditCardAccountDto() { }
    
    public CreditCardAccountDto(int id, string issuingBank, string lastFourDigits, Currency accountCurrency, double availableBalance,DateTime creationDate,DateTime closingDate)
    {
        Id = id;
        IssuingBank = issuingBank;
        LastFourDigits = lastFourDigits;
        AccountCurrency = accountCurrency;
        AvailableBalance = availableBalance;
        CreationDate = creationDate;
        ClosingDate = closingDate;
    }

}