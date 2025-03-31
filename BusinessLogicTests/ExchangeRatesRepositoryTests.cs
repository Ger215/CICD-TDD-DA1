using DataAccess.Context;
using DataAccess.Repository;
using Models;
using Models.Enums;
using Models.Exceptions;

namespace DataAccessTests
{
    [TestClass]
    public class ExchangeRatesRepositoryTests
    {
        private ExchangeRatesRepository _repository;
        private ApplicationDbContext _context;
        private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    
        [TestInitialize]
        public void SetUp()
        {
            _context = _contextFactory.CreateDbContext();
            _repository = new ExchangeRatesRepository(_context);
        }
    
        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
        
        [TestMethod]
        public void WhenAddingANewExchangeRate_ShouldAddTheNewExchangeRateInExchangeRateTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456");
            user.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.Dollar);
            
            _repository.AddExchangeRate(exchangeRate, user.Id);
            
            ExchangeRate exchangeRateInDb = _context.ExchangeRates.FirstOrDefault();
            
            Assert.AreEqual(exchangeRate, exchangeRateInDb);
        }
        
        [TestMethod]
        [ExpectedException(typeof(RepositoryExceptions))]
        public void WhenAddingAnExistingExchangeRate_ShouldReturnARepositoryException()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456");
            user.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.Dollar);
            
            _repository.AddExchangeRate(exchangeRate, user.Id);
            
            _repository.AddExchangeRate(exchangeRate, user.Id);
        }
        
        [TestMethod]
        public void WhenExchangeRatesRepositoryFindsAnExchangeRateById_ShouldReturnTheExchangeRateWithTheAssociatedId()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456");
            user.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.Dollar);
            exchangeRate.Id = 1;
            
            _repository.AddExchangeRate(exchangeRate, user.Id);
            
            ExchangeRate expectedExchangeRate = _repository.FindExchangeRateById(1);
            
            Assert.AreEqual(exchangeRate, expectedExchangeRate);
        }
        
        [TestMethod]
        public void WhenExchangeRatesRepositoryListsAllTheExchangeRates_ShouldReturnAllTheExchangeRatesInTheDatabase()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456");
            user.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.Dollar);
            exchangeRate.Id = 1;
            
            _repository.AddExchangeRate(exchangeRate, user.Id);
            
            List<ExchangeRate> exchangeRates = _repository.GetExchangeRates(user.Id);
            
            Assert.AreEqual(1, exchangeRates.Count);
        }
        
        [TestMethod]
        public void WhenAccountsRepositoryUpdatesACreditCardAccount_ShouldReturnTheUpdatedCreditCardAccount()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456");
            user.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.Dollar);
            exchangeRate.Id = 1;
            
            _repository.AddExchangeRate(exchangeRate, user.Id);

            ExchangeRate exchangeRateInDb = _repository.FindExchangeRateById(1);
            
            exchangeRateInDb.Value = 50;
            
            _repository.UpdateExchangeRate(exchangeRateInDb);
            
            ExchangeRate updatedExchangeRate = _repository.FindExchangeRateById(1);
            
            Assert.AreEqual(50, updatedExchangeRate.Value);
        }
        
        [TestMethod]
        public void WhenExchangeRatesRepositoryDeletesAnExchangeRate_ShouldDeleteTheExchangeRateFromTheExchangeRatesTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456");
            user.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.Dollar);
            exchangeRate.Id = 1;
            
            _repository.AddExchangeRate(exchangeRate, user.Id);
            
            _repository.DeleteExchangeRate(exchangeRate.Id);
            
            List<ExchangeRate> exchangeRates = _repository.GetExchangeRates(user.Id);
            
            Assert.AreEqual(0, exchangeRates.Count);
        }
    }
}