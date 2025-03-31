using Models;
using Models.Enums;

namespace DataAccessTests
{
    [TestClass]
    public class ExchangeRateTests
    {
        [TestMethod]
        public void WhenCreatingANewExchangeRate_ShouldReturnTheNewExchangeRate()
        {
            DateTime exchangeRateDateTime = DateTime.Now;
            double exchangeRateAmmount = 39.35;
            Currency exchangeRateCurrency = Currency.Dollar;
            int userId = 1;


            ExchangeRate exchangeRate = new ExchangeRate(exchangeRateDateTime, exchangeRateAmmount,exchangeRateCurrency);
            exchangeRate.UserId = userId;

            Assert.AreEqual(exchangeRateDateTime, exchangeRate.Date);
            Assert.AreEqual(exchangeRateAmmount, exchangeRate.Value);
            Assert.AreEqual(exchangeRateCurrency, exchangeRate.RateCurrency);
            Assert.AreEqual(userId, exchangeRate.UserId);
        }


    }
}