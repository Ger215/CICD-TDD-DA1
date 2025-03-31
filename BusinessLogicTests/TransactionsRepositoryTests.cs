using DataAccess.Context;
using DataAccess.Repository;
using Models;
using Models.Enums;

namespace DataAccessTests
{
    [TestClass]
    public class TransactionsRepositoryTests
    {
        private TransactionsRepository _repository;
        private ApplicationDbContext _context;
        private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    
        [TestInitialize]
        public void SetUp()
        {
            _context = _contextFactory.CreateDbContext();
            _repository = new TransactionsRepository(_context);
        }
    
        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
        
        [TestMethod]
        public void WhenAddingANewTransaction_ShouldAddTheNewTransactionInTransactionsTable()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle1234");
            user.Id = 1;
            Category category = new Category("Category",DateTime.Today, CategoryType.ExpensesCategory,StatusType.Active);
            category.Id = 1;
            Account account = new GeneralAccount("Caja de Ahorro", Currency.UruguayanPeso,DateTime.Today,20000);
            account.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today,40,Currency.UruguayanPeso);
            exchangeRate.Id = 1;
            Transaction transaction = new Transaction("Transaction",DateTime.Today,100.0,exchangeRate,category,account);

            _repository.AddTransaction(transaction,user.Id,account.Id,category.Id,exchangeRate.Id);

            var transactionInDb = _context.Transactions.First();
            
            Assert.AreEqual(transaction, transactionInDb);
        }
        
        [TestMethod]
        public void WhenTransactionsRepositoryListsAllTheTransactions_ShouldReturnAllTheTransactionsInTheDatabase()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle1234");
            user.Id = 1;
            Category category = new Category("Category",DateTime.Today, CategoryType.ExpensesCategory,StatusType.Active);
            category.Id = 1;
            Account account = new GeneralAccount("Caja de Ahorro", Currency.UruguayanPeso,DateTime.Today,20000);
            account.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today,40,Currency.UruguayanPeso);
            exchangeRate.Id = 1;
            Transaction transaction = new Transaction("Transaction",DateTime.Today,100.0,exchangeRate,category,account);

            _repository.AddTransaction(transaction,user.Id,account.Id,category.Id,exchangeRate.Id);
            
            var transactions = _repository.GetTransactions(user.Id);
            
            Assert.AreEqual(1, transactions.Count);
        }
        
        [TestMethod]
        public void WhenTransactionsRepositoryFindsATransactionById_ShouldReturnTheTransactionWithTheAssociatedId()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle1234");
            user.Id = 1;
            Category category = new Category("Category",DateTime.Today, CategoryType.ExpensesCategory,StatusType.Active);
            category.Id = 1;
            Account account = new GeneralAccount("Caja de Ahorro", Currency.Dollar,DateTime.Today,20000);
            account.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today,40,Currency.Dollar);
            exchangeRate.Id = 1;
            Transaction transaction = new Transaction("Transaction",DateTime.Today,100.0,exchangeRate,category,account);

            _repository.AddTransaction(transaction,user.Id,account.Id,category.Id,exchangeRate.Id);

            var transactionInDb = _repository.FindTransactionById(1);
            
            Assert.AreEqual(transaction, transactionInDb);
        }
        
        [TestMethod]
        public void WhenTransactionsRepositoryUpdatesATransaction_ShouldReturnTheUpdatedTransaction()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle1234");
            user.Id = 1;
            Category category = new Category("Category",DateTime.Today, CategoryType.ExpensesCategory,StatusType.Active);
            category.Id = 1;
            Account account = new GeneralAccount("Caja de Ahorro", Currency.Dollar,DateTime.Today,20000);
            account.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today,40,Currency.Dollar);
            exchangeRate.Id = 1;
            Transaction transaction = new Transaction("Transaction",DateTime.Today,100.0,exchangeRate,category,account);
            transaction.Id = 1;
            
            _repository.AddTransaction(transaction,user.Id,account.Id,category.Id,exchangeRate.Id);

            Transaction transactionInDb = _repository.FindTransactionById(1);
            
            ExchangeRate newExchangeRate = new ExchangeRate(DateTime.Today,50,Currency.Dollar);
            
            transactionInDb.ExchangeRateAssociated = newExchangeRate;
            transactionInDb.Amount = 10.0;
            transactionInDb.CategoryAssociated = new Category("Category2",DateTime.Today, CategoryType.ExpensesCategory,StatusType.Active);
            
            _repository.UpdateTransaction(transactionInDb);
            
            Assert.AreEqual(transaction, transactionInDb);
        }

        [TestMethod]
        public void WhenTransactionsRepositoryUpdatesATransactionExchangeRate_ShouldReturnTheUpdatedTransaction()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle1234");
            user.Id = 1;
            Category category = new Category("Category", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            category.Id = 1;
            Account account = new GeneralAccount("Caja de Ahorro", Currency.Dollar, DateTime.Today, 20000);
            account.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.Dollar);
            exchangeRate.Id = 1;
            Transaction transaction = new Transaction("Transaction", DateTime.Today, 100.0, exchangeRate, category, account);
            transaction.Id = 1;

            _repository.AddTransaction(transaction, user.Id, account.Id, category.Id, exchangeRate.Id);

            Transaction transactionInDb = _repository.FindTransactionById(1);

            ExchangeRate newExchangeRate = new ExchangeRate(DateTime.Today, 50, Currency.Dollar);

            transactionInDb.ExchangeRateAssociated = newExchangeRate;
            transactionInDb.Amount = 10.0;
            transactionInDb.CategoryAssociated = new Category("Category2", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);

            _repository.UpdateTransactionRate(transactionInDb);

            Assert.AreEqual(transaction, transactionInDb);
           
        }


        [TestMethod]
        public void WhenTransactionsRepositoryGetsTheTotalSpentInAMonth_ShouldReturnTheTotalSpentInThatMonth()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle1234");
            user.Id = 1;
            Category category = new Category("Category", DateTime.Today, CategoryType.ExpensesCategory,
                StatusType.Active);
            category.Id = 1;
            Account account = new GeneralAccount("Caja de Ahorro", Currency.Dollar, DateTime.Today, 20000);
            account.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.Dollar);
            exchangeRate.Id = 1;

            Transaction transaction1 =
                new Transaction("Transaction1", DateTime.Today, 100.0, exchangeRate, category, account);
            transaction1.Id = 1;
            Transaction transaction2 =
                new Transaction("Transaction2", DateTime.Today, 150.0, exchangeRate, category, account);
            transaction2.Id = 2;
            Transaction transaction3 =
                new Transaction("Transaction3", DateTime.Today, 200.0, exchangeRate, category, account);
            transaction3.Id = 3;

            _repository.AddTransaction(transaction1, user.Id, account.Id, category.Id, exchangeRate.Id);
            _repository.AddTransaction(transaction2, user.Id, account.Id, category.Id, exchangeRate.Id);
            _repository.AddTransaction(transaction3, user.Id, account.Id, category.Id, exchangeRate.Id);

            int totalSpent = _repository.GetTotalSpentInMonth(user.Id, DateTime.Today);

            Assert.AreEqual(450 * exchangeRate.Value, totalSpent);
        }
    }
}