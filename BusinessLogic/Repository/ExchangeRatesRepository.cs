using DataAccess.Context;
using Models;
using Models.Exceptions;

namespace DataAccess.Repository;

public class ExchangeRatesRepository
{
    private ApplicationDbContext _database;

    public ExchangeRatesRepository(ApplicationDbContext database)
    {
        _database = database;
    }
    
    public void AddExchangeRate(ExchangeRate newExchangeRate, int userId)
    {
        if(ExchangeRateAlreadyExists(newExchangeRate, userId))
        {
            ExchangeRateAlreadyExistsException();
        }

        AddNewExchangeRateToExchangeRatesTable(newExchangeRate, userId);
    }

    private void AddNewExchangeRateToExchangeRatesTable(ExchangeRate newExchangeRate, int userId)
    {
        newExchangeRate.UserId = userId;
        _database.ExchangeRates.Add(newExchangeRate);
        _database.SaveChanges();
    }

    private void ExchangeRateAlreadyExistsException()
    {
        throw new RepositoryExceptions("Exchange Rate already exists, pleas edit the existing one");
    }

    private bool ExchangeRateAlreadyExists(ExchangeRate newExchangeRate, int userId)
    {
        return _database.ExchangeRates.Any(exrate => exrate.Id == newExchangeRate.Id && exrate.UserId == userId);
    }

    public ExchangeRate FindExchangeRateById(int id)
    {
        ExchangeRate exchangeRate = _database.ExchangeRates.FirstOrDefault(acc => acc.Id == id);
        return exchangeRate;
    }

    public List<ExchangeRate> GetExchangeRates(int userId)
    {
        User user = _database.Users.FirstOrDefault(user => user.Id == userId);
        return _database.ExchangeRates.Where(exrate => exrate.UserId == userId).ToList();
    }

    public void UpdateExchangeRate(ExchangeRate exchangeRate)
    {
        var dbExchangeRate = _database.ExchangeRates.FirstOrDefault(exrate => exrate.Id == exchangeRate.Id);
        if (dbExchangeRate != null)
        {
            dbExchangeRate.Value = exchangeRate.Value;
            _database.SaveChanges();
        }
    }

    public void DeleteExchangeRate(int id)
    {
        var exchangeRate = _database.ExchangeRates.FirstOrDefault(exrate => exrate.Id == id);
        if (exchangeRate != null)
        {
            _database.ExchangeRates.Remove(exchangeRate);
            _database.SaveChanges();
        }
    }
}