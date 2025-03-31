using Logic.DTOs;
using Models;

namespace Logic.Interfaces;

public interface IExchangeRateController
{
    public ExchangeRate CreateExchangeRate(ExchangeRateDto exchangeRateDto, int userId);
    public void AddExchangeRate(ExchangeRateDto exchangeRateDto, int userId);
    public List<ExchangeRateDto> GetExchangeRates(int userId);
    public void ChangeExchangeRateValue(ExchangeRate exchangeRate, ExchangeRateDto exchangeRateDto);
    public void UpdateExchangeRate(ExchangeRateDto exchangeRateDto);
    public ExchangeRateDto GetExchangeRateById(int id);
    public void DeleteExchangeRate(ExchangeRateDto exchangeRateDto, int id);
}