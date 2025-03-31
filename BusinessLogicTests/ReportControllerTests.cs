using DataAccess.Context;
using Logic;
using Logic.DTOs;
using Models;
using Models.Enums;

namespace DataAccessTests
{
    [TestClass]
    public class ReportControllerTests
    {
        private ApplicationDbContext _context;
        private ReportController _controller;
        private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    
        [TestInitialize]
        public void SetUp()
        {
            _context = _contextFactory.CreateDbContext();
            _controller = new ReportController(_context);
        }
    
        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
        
        [TestMethod]
        public void WhenReportControllerGeneratesTheReportCategoryByGoal_ShouldReturnAListOfReportGoalByCategoryDto()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German1234", "Calle 1234");
            user.Id = 1;
            Category categoria1 = new Category("Salida", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            categoria1.Id = 1;
            Category categoria2 = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            categoria2.Id = 2;
            Category categoria3 = new Category("Comida", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            categoria3.Id = 3;
            _controller._categoryRepository.AddCategory(categoria1,user.Id);
            _controller._categoryRepository.AddCategory(categoria2,user.Id);
            _controller._categoryRepository.AddCategory(categoria3,user.Id);
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.Dollar);
            exchangeRate.Id = 1;
            _controller._exchangeRatesRepository.AddExchangeRate(exchangeRate,user.Id);
            GeneralAccount generalAccount = new GeneralAccount("Cuenta",Currency.Dollar,DateTime.Today,2000);
            generalAccount.Id = 1;
            _controller._accountsRepository.AddGeneralAccount(generalAccount,user.Id);
            Transaction transaction = new Transaction("Restaurante", DateTime.Today,200,exchangeRate,categoria3,generalAccount);
            Transaction transaction2 = new Transaction("Pelicula", DateTime.Today,100,exchangeRate,categoria2,generalAccount);
            _controller._transactionsRepository.AddTransaction(transaction,user.Id,generalAccount.Id,categoria3.Id,exchangeRate.Id);
            _controller._transactionsRepository.AddTransaction(transaction2,user.Id,generalAccount.Id,categoria2.Id,exchangeRate.Id);

            List<ReportGoalByCategoryDto> expectedReport = new List<ReportGoalByCategoryDto>();
            expectedReport.Add(new ReportGoalByCategoryDto(categoria1, 0, 0));
            expectedReport.Add(new ReportGoalByCategoryDto(categoria2, 100, 33));
            expectedReport.Add(new ReportGoalByCategoryDto(categoria3, 200, 66));
            
            List<ReportGoalByCategoryDto> actualReport = _controller.GenerateReportByCategories(user.Id, DateTime.Today);
            
            Assert.AreEqual(expectedReport[0].CategoryAssociated, actualReport[0].CategoryAssociated);
            Assert.AreEqual(expectedReport[0].TotalSpent*exchangeRate.Value, actualReport[0].TotalSpent);
            Assert.AreEqual(expectedReport[0].PercentageSpent, actualReport[0].PercentageSpent);
            
            Assert.AreEqual(expectedReport[1].CategoryAssociated, actualReport[1].CategoryAssociated);
            Assert.AreEqual(expectedReport[1].TotalSpent*exchangeRate.Value, actualReport[1].TotalSpent);
            Assert.AreEqual(expectedReport[1].PercentageSpent, actualReport[1].PercentageSpent);
            
            Assert.AreEqual(expectedReport[2].CategoryAssociated, actualReport[2].CategoryAssociated);
            Assert.AreEqual(expectedReport[2].TotalSpent*exchangeRate.Value, actualReport[2].TotalSpent);
            Assert.AreEqual(expectedReport[2].PercentageSpent, actualReport[2].PercentageSpent);
        }

        [TestMethod]
        public void WhenReportControllerGeneratesTheReportGoalByMonth_ShouldReturnAListOfReportGoalByMonthDto()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German1234", "Calle 1234");
            user.Id = 1;
            ICollection<Category> categories = new List<Category>();
            Category categoria = new Category("Salida", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            categoria.Id = 1;
            categories.Add(categoria);
            Category categoria1 = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            categoria1.Id = 2;
            categories.Add(categoria1);
            Category categoria2 = new Category("Comida", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            categoria2.Id = 3;
            categories.Add(categoria2);
            Currency goalCurrency = Currency.UruguayanPeso;
            SpendingGoal spendingGoal = new SpendingGoal("Menos Noche", 2000, goalCurrency,categories);
            spendingGoal.Id = 1;
            _controller._spendingGoalsRepository.AddSpendingGoal(spendingGoal,user.Id);
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.Dollar);
            exchangeRate.Id = 1;
            _controller._exchangeRatesRepository.AddExchangeRate(exchangeRate,user.Id);GeneralAccount generalAccount = new GeneralAccount("Cuenta",Currency.Dollar,DateTime.Today,2000);
            generalAccount.Id = 1;
            _controller._accountsRepository.AddGeneralAccount(generalAccount,user.Id);
            Transaction transaction = new Transaction("Restaurante", DateTime.Today,200,exchangeRate,categoria,generalAccount);
            Transaction transaction2 = new Transaction("Pelicula", DateTime.Today,100,exchangeRate,categoria2,generalAccount);
            _controller._transactionsRepository.AddTransaction(transaction,user.Id,generalAccount.Id,categoria1.Id,exchangeRate.Id);
            _controller._transactionsRepository.AddTransaction(transaction2,user.Id,generalAccount.Id,categoria2.Id,exchangeRate.Id);

            List<ReportGoalByMonthDto> expectedReport = new List<ReportGoalByMonthDto>();
            expectedReport.Add(new ReportGoalByMonthDto(spendingGoal, 300, true, DateTime.Today));
            
            List<ReportGoalByMonthDto> actualReport = _controller.GenerateReportGoalsByMonth(user.Id, DateTime.Today);
            
            Assert.AreEqual(expectedReport[0].SpendingGoalAssociated, actualReport[0].SpendingGoalAssociated);
            Assert.AreEqual(expectedReport[0].Achieved, actualReport[0].Achieved);
            Assert.AreEqual(expectedReport[0].AmountSpent, actualReport[0].AmountSpent);
        }

        [TestMethod]
        public void WhenReportControllerGeneratesTheExpensesReport_ShouldReturnAListOfTransactionDto()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German1234", "Calle 1234");
            user.Id = 1;
            Category categoria = new Category("Salidas", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            categoria.Id = 1;
            GeneralAccount generalAccount = new GeneralAccount("Cuenta",Currency.Dollar,DateTime.Today,2000);
            generalAccount.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.Dollar);
            exchangeRate.Id = 1;
            Transaction transaction = new Transaction("Restaurante", DateTime.Today,200,exchangeRate,categoria,generalAccount);
            transaction.Id = 1;
            Transaction transaction2 = new Transaction("Pelicula", DateTime.Today,100,exchangeRate,categoria,generalAccount);
            transaction2.Id = 2;
            _controller._transactionsRepository.AddTransaction(transaction,user.Id,generalAccount.Id,categoria.Id,exchangeRate.Id);
            _controller._transactionsRepository.AddTransaction(transaction2,user.Id,generalAccount.Id,categoria.Id,exchangeRate.Id);
            
            List<TransactionDto> expectedReport = new List<TransactionDto>();
            expectedReport.Add(new TransactionDto(transaction.Id, transaction.Title, transaction.Date, transaction.Amount, transaction.ExchangeRateAssociated, transaction.CategoryAssociated, transaction.AccountAssociated));
            expectedReport.Add(new TransactionDto(transaction2.Id, transaction2.Title, transaction2.Date, transaction2.Amount, transaction2.ExchangeRateAssociated, transaction2.CategoryAssociated, transaction2.AccountAssociated));
            
            CategoryDto categoryDto = new CategoryDto(categoria.Id, categoria.Name,categoria.CreationDate, categoria.TypeOf, categoria.Status);
            AccountDto accountDto = new AccountDto(generalAccount.Id);
            List<TransactionDto> actualReport = _controller.GetTransactionsFiltered(DateTime.Today, DateTime.Today, categoryDto, accountDto, user.Id);
            
            Assert.AreEqual(expectedReport[0].Id, actualReport[0].Id);
            Assert.AreEqual(expectedReport[0].Title, actualReport[0].Title);
            Assert.AreEqual(expectedReport[0].CategoryAssociated, actualReport[0].CategoryAssociated);
            Assert.AreEqual(expectedReport[0].AccountAssociated, actualReport[0].AccountAssociated);
            Assert.AreEqual(expectedReport[0].ExchangeRateAssociated, actualReport[0].ExchangeRateAssociated);
            Assert.AreEqual(expectedReport[0].Amount, actualReport[0].Amount);
        }

        [TestMethod]
        public void WhenReportControllerGeneratesTheGeneralAccountBalanceReport_ShouldReturnAListOfReportGeneralAccountBalanceDto()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German1234", "Calle 1234");
            user.Id = 1;
            GeneralAccount generalAccount = new GeneralAccount("Cuenta",Currency.UruguayanPeso,DateTime.Today,40000);
            generalAccount.Id = 1;
            GeneralAccountDto generalAccountDto = new GeneralAccountDto(generalAccount.Id, generalAccount.Name, generalAccount.InitialAmmount, generalAccount.AccountCurrency, generalAccount.CreationDate);
            _controller._accountsRepository.AddGeneralAccount(generalAccount,user.Id);
            ExchangeRate exchangeRate = new ExchangeRate(DateTime.Today, 1, Currency.UruguayanPeso);
            exchangeRate.Id = 1;
            _controller._exchangeRatesRepository.AddExchangeRate(exchangeRate,user.Id);
            Category categoriaIngreso = new Category("Sueldo", DateTime.Today, CategoryType.IncomeCategory, StatusType.Active);
            categoriaIngreso.Id = 1;
            _controller._categoryRepository.AddCategory(categoriaIngreso,user.Id);
            Category categoriaEgreso = new Category("Salida", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            categoriaEgreso.Id = 2;
            _controller._categoryRepository.AddCategory(categoriaEgreso,user.Id);
            Transaction transaction = new Transaction("Pago", DateTime.Today,1000,exchangeRate,categoriaIngreso,generalAccount);
            transaction.Id = 1;
            Transaction transaction2 = new Transaction("Pelicula", DateTime.Today,400,exchangeRate,categoriaEgreso,generalAccount);
            transaction2.Id = 2;
            _controller._transactionsRepository.AddTransaction(transaction,user.Id,generalAccount.Id,categoriaIngreso.Id,exchangeRate.Id);
            _controller._transactionsRepository.AddTransaction(transaction2,user.Id,generalAccount.Id,categoriaEgreso.Id,exchangeRate.Id);
            
            List<ReportGeneralAccountBalanceDto> expectedReport = new List<ReportGeneralAccountBalanceDto>();
            double balance = generalAccount.InitialAmmount + transaction.Amount - transaction2.Amount;
            expectedReport.Add(new ReportGeneralAccountBalanceDto(generalAccountDto.Name, balance, generalAccountDto.AccountCurrency));
            
            List<ReportGeneralAccountBalanceDto> actualReport = _controller.GenerateReportGeneralAccountBalance(generalAccountDto,user.Id);
            
            Assert.AreEqual(expectedReport[0].GeneralAccountName, actualReport[0].GeneralAccountName);
            Assert.AreEqual(expectedReport[0].Balance, actualReport[0].Balance);
            Assert.AreEqual(expectedReport[0].AccountCurrency, actualReport[0].AccountCurrency);
        }

        [TestMethod]
        public void WhenReportControllerGeneratesTheCreditCardExpensesReport_ShouldReturnAListOfReportCreditCardExpensesDto()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German1234", "Calle 1234");
            user.Id = 1;
            CreditCardAccount creditCardAccount = new CreditCardAccount("Santander","1234",Currency.UruguayanPeso,20000,DateTime.Today,new DateTime(2023,10,10));
            creditCardAccount.Id = 1;
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(creditCardAccount.Id,creditCardAccount.IssuingBank,creditCardAccount.LastFourDigits,creditCardAccount.AccountCurrency,creditCardAccount.AvailableBalance,creditCardAccount.CreationDate,creditCardAccount.ClosingDate);
            _controller._accountsRepository.AddCreditCardAccount(creditCardAccount,user.Id);
            ExchangeRate exchangeRate = new ExchangeRate(new DateTime(2023, 11, 9), 1, Currency.UruguayanPeso);
            exchangeRate.Id = 1;
            _controller._exchangeRatesRepository.AddExchangeRate(exchangeRate,user.Id);
            Category categoria = new Category("Salida", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            categoria.Id = 1;
            _controller._categoryRepository.AddCategory(categoria,user.Id);
            DateTime date = new DateTime(2023, 11, 9);
            Transaction transaction = new Transaction("Pago",date ,1000,exchangeRate,categoria,creditCardAccount);
            transaction.Id = 1;
            Transaction transaction2 = new Transaction("Pelicula",date,400,exchangeRate,categoria,creditCardAccount);
            transaction2.Id = 2;
            _controller._transactionsRepository.AddTransaction(transaction,user.Id,creditCardAccount.Id,categoria.Id,exchangeRate.Id);
            _controller._transactionsRepository.AddTransaction(transaction2,user.Id,creditCardAccount.Id,categoria.Id,exchangeRate.Id);
            
            List<ReportCreditCardExpensesDto> expectedReport = new List<ReportCreditCardExpensesDto>();
            expectedReport.Add(new ReportCreditCardExpensesDto(creditCardAccount,1400));
            
            List<ReportCreditCardExpensesDto> actualReport = _controller.GenerateReportCreditCardExpenses(creditCardAccountDto,user.Id,DateTime.Today);
            
            Assert.AreEqual(expectedReport[0].CreditCardAccount, actualReport[0].CreditCardAccount);
            Assert.AreEqual(expectedReport[0].TotalSpent, actualReport[0].TotalSpent);
            
        }

        [TestMethod]
        public void WhenReportControllerGeneratesTheIncomeOutcomeReport_ShouldReturnAListOfReportIncomeOutcomeDto()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German1234", "Calle 1234");
            user.Id = 1;
            ExchangeRate exchangeRate = new ExchangeRate(new DateTime(2023, 11, 1), 1, Currency.UruguayanPeso);
            exchangeRate.Id = 1;
            _controller._exchangeRatesRepository.AddExchangeRate(exchangeRate,user.Id);
            Category categoriaIngreso = new Category("Sueldo", DateTime.Today, CategoryType.IncomeCategory, StatusType.Active);
            categoriaIngreso.Id = 1;
            _controller._categoryRepository.AddCategory(categoriaIngreso,user.Id);
            Category categoriaEgreso = new Category("Salida", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            categoriaEgreso.Id = 2;
            _controller._categoryRepository.AddCategory(categoriaEgreso,user.Id);
            GeneralAccount generalAccount = new GeneralAccount("Cuenta",Currency.UruguayanPeso,DateTime.Today,2000);
            generalAccount.Id = 1;
            _controller._accountsRepository.AddGeneralAccount(generalAccount,user.Id);
            DateTime date = new DateTime(2023, 11, 1);
            Transaction transaction = new Transaction("Pago", date,1000,exchangeRate,categoriaIngreso,generalAccount);
            transaction.Id = 1;
            Transaction transaction2 = new Transaction("Pelicula", date,400,exchangeRate,categoriaEgreso,generalAccount);
            transaction2.Id = 2;
            _controller._transactionsRepository.AddTransaction(transaction,user.Id,generalAccount.Id,categoriaIngreso.Id,exchangeRate.Id);
            _controller._transactionsRepository.AddTransaction(transaction2,user.Id,generalAccount.Id,categoriaEgreso.Id,exchangeRate.Id);
            
            List<ReportIncomeOutcomeDto> expectedReport = new List<ReportIncomeOutcomeDto>();
            expectedReport.Add(new ReportIncomeOutcomeDto(1,1000,400));
            
            List<ReportIncomeOutcomeDto> actualReport = _controller.GetIncomeOutcome(date,user.Id);
            
            Assert.AreEqual(expectedReport[0].Day, actualReport[0].Day);
            Assert.AreEqual(expectedReport[0].Income, actualReport[0].Income);
            Assert.AreEqual(expectedReport[0].Outcome, actualReport[0].Outcome);
            
        }
    }
}