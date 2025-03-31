using DataAccess.Context;
using DataAccess.Repository;
using Models;
using Models.Enums;
using Models.Exceptions;

namespace DataAccessTests
{
    [TestClass]
    public class AccountsRepositoryTests
    {
        private AccountsRepository _repository;
        private ApplicationDbContext _context;
        private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    
        [TestInitialize]
        public void SetUp()
        {
            _context = _contextFactory.CreateDbContext();
            _repository = new AccountsRepository(_context);
        }
    
        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
        
        [TestMethod]
        public void WhenAddingANewGeneralAccount_ShouldAddTheNewGeneralAccountInAccountsTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            GeneralAccount generalAccount = new GeneralAccount("Banco Santander",Currency.UruguayanPeso,DateTime.Today, 25000);

            _repository.AddGeneralAccount(generalAccount,1);

            var categoryInDb = _context.Accounts.First();
            
            Assert.AreEqual(generalAccount, categoryInDb);
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryExceptions))]
        public void WhenAddingAGeneralAccountThatAlreadyExists_ShouldReturnARepositoryException() 
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            GeneralAccount generalAccount = new GeneralAccount("Banco Santander",Currency.UruguayanPeso,DateTime.Today, 25000);
            GeneralAccount generalAccount2 = new GeneralAccount("Banco Santander",Currency.UruguayanPeso,DateTime.Today, 25000);

            _repository.AddGeneralAccount(generalAccount, 1);
            _repository.AddGeneralAccount(generalAccount2, 1);
            
        }
        
        [TestMethod]
        public void WhenAccountsRepositoryListsAllTheGeneralAccounts_ShouldReturnAllTheGeneralAccountsInTheDatabase()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            GeneralAccount generalAccount = new GeneralAccount("Banco Santander",Currency.UruguayanPeso,DateTime.Today, 25000);

            _repository.AddGeneralAccount(generalAccount, 1);
            
            List<GeneralAccount> generalAccounts = _repository.GetGeneralAccounts(1);
            
            Assert.AreEqual(1,generalAccounts.Count);
        }
        
        [TestMethod]
        public void WhenAccountsRepositoryFindsAGeneralAccountById_ShouldReturnTheGeneralAccountWithTheAssociatedId()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            GeneralAccount generalAccount = new GeneralAccount("Banco Santander",Currency.UruguayanPeso,DateTime.Today, 25000);
            generalAccount.Id = 1;
            
            _repository.AddGeneralAccount(generalAccount, 1);

            GeneralAccount expectedGeneralAccount = _repository.FindGeneralAccountById(1);
            
            Assert.AreEqual(generalAccount,expectedGeneralAccount);
        }
        
        [TestMethod]
        
        public void WhenAccountsRepositoryUpdatesAGeneralAccount_ShouldReturnTheUpdatedGeneralAccount()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            GeneralAccount generalAccount = new GeneralAccount("Banco Santander",Currency.UruguayanPeso,DateTime.Today, 25000);
            generalAccount.Id = 1;
            _repository.AddGeneralAccount(generalAccount, 1);

            GeneralAccount generalAccountInDb = _repository.FindGeneralAccountById(1);
            
            generalAccountInDb.Name = "Itau";
            
            _repository.UpdateGeneralAccount(generalAccountInDb);
            
            Assert.AreEqual(generalAccountInDb.Name,generalAccount.Name);
        }
        
        [TestMethod]
        public void WhenAccountsRepositoryDeletesAGeneralAccount_ShouldDeleteTheGeneralAccountFromTheAccountsTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            GeneralAccount generalAccount = new GeneralAccount("Banco Santander",Currency.UruguayanPeso,DateTime.Today, 25000);
            _repository.AddGeneralAccount(generalAccount, 1);
            
            _repository.DeleteGeneralAccount(generalAccount.Name,1);
            
            Assert.IsFalse(_repository.GeneralAccountAlreadyExists(generalAccount, 1));
        }
        
        [TestMethod]
        public void WhenAddingANewCreditCardAccount_ShouldAddTheNewCreditCardAccountInAccountsTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creditCardAccountCreationDate = DateTime.Today;
            DateTime creditCardAccountClosingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccount creditCardAccount = new CreditCardAccount("Santander","1234",creditCardAccountCurrency,25000,
                                                                        creditCardAccountCreationDate,creditCardAccountClosingDate);
            _repository.AddCreditCardAccount(creditCardAccount,user.Id);

            var categoryInDb = _context.Accounts.First();
            
            Assert.AreEqual(creditCardAccount, categoryInDb);
        }
        
        [TestMethod]
        [ExpectedException(typeof(RepositoryExceptions))]
        public void WhenAddingACreditCardAccountThatAlreadyExists_ShouldReturnARepositoryException()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creditCardAccountCreationDate = DateTime.Today;
            DateTime creditCardAccountClosingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccount creditCardAccount = new CreditCardAccount("Santander","1234",creditCardAccountCurrency,25000,
                creditCardAccountCreationDate,creditCardAccountClosingDate);
            creditCardAccount.Id = 1;
            CreditCardAccount creditCardAccount2 = new CreditCardAccount("Santander","1234",creditCardAccountCurrency,25000,
                creditCardAccountCreationDate,creditCardAccountClosingDate);
            creditCardAccount2.Id = 1;
            
            _repository.AddCreditCardAccount(creditCardAccount,user.Id);
            _repository.AddCreditCardAccount(creditCardAccount2,user.Id);
            
        }
        
        [TestMethod]
        public void WhenAccountsRepositoryListsAllTheAccounts_ShouldReturnAllTheAccountsInTheDatabase()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Account account = new Account("Santander",Currency.UruguayanPeso,DateTime.Today);
            Account account2 = new Account("Itau",Currency.UruguayanPeso,DateTime.Today);
            
            _repository.AddAccount(account,user.Id);
            _repository.AddAccount(account2,user.Id);
            
            List<Account> accounts = _repository.GetAccounts(user.Id);
            
            Assert.AreEqual(2,accounts.Count);
        }
        
        [TestMethod]
        public void WhenAccountsRepositoryListsAllTheCreditCardAccounts_ShouldReturnAllTheCreditCardAccountsInTheDatabase()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creditCardAccountCreationDate = DateTime.Today;
            DateTime creditCardAccountClosingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccount creditCardAccount = new CreditCardAccount("Santander","1234",creditCardAccountCurrency,25000,
                creditCardAccountCreationDate,creditCardAccountClosingDate);
            
            _repository.AddCreditCardAccount(creditCardAccount,user.Id);
            
            List<CreditCardAccount> creditCardAccounts = _repository.GetCreditCardAccounts(user.Id);
            
            Assert.AreEqual(1,creditCardAccounts.Count);
        }
        
        [TestMethod]
        public void WhenAccountsRepositoryFindsACreditCardAccountById_ShouldReturnTheCreditCardAccountWithTheAssociatedId()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creditCardAccountCreationDate = DateTime.Today;
            DateTime creditCardAccountClosingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccount creditCardAccount = new CreditCardAccount("Santander","1234",creditCardAccountCurrency,25000,
            creditCardAccountCreationDate,creditCardAccountClosingDate);
            creditCardAccount.Id = 1;
            
            _repository.AddCreditCardAccount(creditCardAccount,user.Id);

            CreditCardAccount expectedCreditCardAccount = _repository.FindCreditCardAccountById(1);
            
            Assert.AreEqual(creditCardAccount,expectedCreditCardAccount);
        }

        [TestMethod]
        public void WhenAccountsRepositoryFindsAnAccountById_ShouldReturnAccountWithTheAssociatedId()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            Account account = new Account("Santander",creditCardAccountCurrency , DateTime.Now);
            account.Id = 1;

            _repository.AddAccount(account, user.Id);

            Account expectedAccount = _repository.FindAccountById(1);

            Assert.AreEqual(account, expectedAccount);
        }


        [TestMethod]
        public void WhenAccountsRepositoryUpdatesACreditCardAccount_ShouldReturnTheUpdatedCreditCardAccount()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creditCardAccountCreationDate = DateTime.Today;
            DateTime creditCardAccountClosingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccount creditCardAccount = new CreditCardAccount("Santander","1234",creditCardAccountCurrency,25000,
                creditCardAccountCreationDate,creditCardAccountClosingDate);
            creditCardAccount.Id = 1;
            
            _repository.AddCreditCardAccount(creditCardAccount,user.Id);

            CreditCardAccount creditCardAccountInDb = _repository.FindCreditCardAccountById(1);

            creditCardAccountInDb.IssuingBank = "Itau";
            creditCardAccountInDb.LastFourDigits = "9876";
            creditCardAccountInDb.AccountCurrency = Currency.Dollar;
            creditCardAccountInDb.ClosingDate = new DateTime(2025, 10, 10);
            
            _repository.UpdateCreditCardAccount(creditCardAccountInDb, user.Id);
            
            Assert.AreEqual(creditCardAccountInDb.IssuingBank,creditCardAccount.IssuingBank);
            Assert.AreEqual(creditCardAccountInDb.LastFourDigits,creditCardAccount.LastFourDigits);
        }
        
        [TestMethod]
        public void WhenAccountsRepositoryDeletesACreditCardAccount_ShouldDeleteTheCreditCardAccountFromTheAccountsTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creditCardAccountCreationDate = DateTime.Today;
            DateTime creditCardAccountClosingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccount creditCardAccount = new CreditCardAccount("Santander","1234",creditCardAccountCurrency,25000,
                creditCardAccountCreationDate,creditCardAccountClosingDate);
            creditCardAccount.Id = 1;
            
            _repository.AddCreditCardAccount(creditCardAccount,user.Id);
            
            _repository.DeleteCreditCardAccount(creditCardAccount.Id,user.Id);
            
            Assert.IsFalse(_repository.CreditCardAccountAlreadyExists(creditCardAccount,user.Id));
        }
    }
}