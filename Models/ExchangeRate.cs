using Models.Enums;

namespace Models
{
    public class ExchangeRate
    {
        public int? UserId { get; set; }
        public ICollection<Transaction> Transactions { get; set;}
        public Currency RateCurrency { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public int Id { get; set; }

        public ExchangeRate() { }
        public ExchangeRate(DateTime exchangeRateDate, double exchangeRateValue, Currency exchangeRateCurrency)
        {
            Date = exchangeRateDate;
            Value = exchangeRateValue;
            RateCurrency = exchangeRateCurrency;
        }
    }
}
