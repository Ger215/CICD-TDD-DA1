using Models.Enums;

namespace Logic.DTOs;

public class ExchangeRateDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double Value { get; set; }
    public Currency RateCurrency { get; set; }
    
    public ExchangeRateDto() { }

    public ExchangeRateDto(int id, DateTime creationDate, double value, Currency currency)
    {
        Id = id;
        Date = creationDate;
        Value = value;
        RateCurrency = currency;
    }
}