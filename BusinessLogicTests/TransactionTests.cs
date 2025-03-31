using Models;
using Models.Enums;
using Models.Exceptions;

namespace DataAccessTests
{
    [TestClass]
    public class TransactionTests
    {
        [TestMethod]

        public void WhenCreatingANewEmptyTransaction_ShouldReturnAnEmptyTransaction()
        {
            Transaction transaction = new Transaction();

            Assert.AreEqual("", transaction.Title);
            Assert.AreEqual(0.0, transaction.Amount);
            Assert.AreEqual(null, transaction.AccountAssociated);
            Assert.AreEqual(null, transaction.CategoryAssociated);
        }
        
        [TestMethod]
        public void WhenCreatingANewTransaction_ShouldReturnTheNewTransaction()
        {
            string expectedTitle = "Trabajo";
            double expectedAmount = 100.0;
            DateTime todayDate = new DateTime(2023, 09, 18);
            Currency transactionCurrency = Currency.UruguayanPeso;
            Category category = new Category("Trabajo",DateTime.Today,CategoryType.IncomeCategory,StatusType.Active);
            Account ac = new Account("Banco Santander", Currency.UruguayanPeso, todayDate);
            ExchangeRate exchangeRate = new ExchangeRate(todayDate, 40, Currency.UruguayanPeso);
            Transaction transaction = new Transaction(expectedTitle, todayDate, expectedAmount,exchangeRate, category,ac);

            Assert.AreEqual(expectedTitle, transaction.Title);
            Assert.AreEqual(todayDate, transaction.Date);
            Assert.AreEqual(expectedAmount, transaction.Amount);
            Assert.AreEqual(ac, transaction.AccountAssociated);
            Assert.AreEqual(transaction.CategoryAssociated,category);
        }

        
        [TestMethod]
        [ExpectedException(typeof(TransactionExceptions))]
        public void WhenCreatingANewTransactionWithEmptyTitle_ShouldReturnATransactionException()
        {
            double expectedAmount = 100.0;
            DateTime todayDate = new DateTime(2023, 09, 18);
            Account account = new Account("Banco Santander", Currency.UruguayanPeso, todayDate);
            Currency transactionCurrency = Currency.UruguayanPeso;
            Category category = new Category("Trabajo",DateTime.Today,CategoryType.IncomeCategory,StatusType.Active);
            ExchangeRate exchangeRate = new ExchangeRate(todayDate, 40, Currency.Dollar);
            Transaction transaction = new Transaction("", todayDate, expectedAmount,exchangeRate, category,account);
        }

        [TestMethod]
        [ExpectedException(typeof(TransactionExceptions))]
        public void WhenCreatingANewTransactionWithAmountLowerThanZero_ShouldReturnATransactionAmountException()
        {
            DateTime todayDate = new DateTime(2023, 09, 18);
            Currency transactionCurrency = Currency.UruguayanPeso;
            Category category = new Category("Trabajo",DateTime.Today,CategoryType.IncomeCategory,StatusType.Active);
            Account account = new Account("Banco Santander", Currency.UruguayanPeso, todayDate);
            ExchangeRate exchangeRate = new ExchangeRate(todayDate, 40, Currency.Dollar);
            Transaction transaction = new Transaction("Viaje De Egresados", todayDate, -1.0,exchangeRate, category,account);
        }

        [TestMethod]

        public void WhenCreatingANewTransactionWithCreditCardAccount_ShouldReturnTheNewTransactionWithTheCreditCardAccount()
        {
            string expectedTitle = "Trabajo";
            double expectedAmount = 100.0;
            DateTime todayDate = new DateTime(2023, 09, 18);
            DateTime closingDate = new DateTime(2025, 12, 18);
            Currency transactionCurrency = Currency.UruguayanPeso;
            Category category = new Category("Trabajo",DateTime.Today,CategoryType.IncomeCategory,StatusType.Active);
            ExchangeRate exchangeRate = new ExchangeRate(todayDate, 40, Currency.Dollar);
            CreditCardAccount creditCardAccount = new CreditCardAccount("Banco Santander", Currency.Dollar, todayDate, "Santander", "1234", 1000.0, closingDate);
            Transaction transaction = new Transaction(expectedTitle, todayDate, expectedAmount,exchangeRate, category, creditCardAccount);

            Assert.AreEqual(creditCardAccount, transaction.AccountAssociated);
        }

        [TestMethod]

        public void WhenCreatingANewTransactionWithGeneralAccount_ShouldReturnTheNewTransactionWithTheGeneralAccount()
        {
            string expectedTitle = "Trabajo";
            double expectedAmount = 100.0;
            double initialAmmount = 1000.0;
            DateTime todayDate = new DateTime(2023, 09, 18);
            Currency transactionCurrency = Currency.UruguayanPeso;
            Category category = new Category("Trabajo",DateTime.Today,CategoryType.IncomeCategory,StatusType.Active);
            ExchangeRate exchangeRate = new ExchangeRate(todayDate, 40, Currency.Dollar);
            GeneralAccount generalAccount = new GeneralAccount("Banco Santander", Currency.Dollar, todayDate, initialAmmount);
            Transaction transaction = new Transaction(expectedTitle, todayDate, expectedAmount,exchangeRate, category, generalAccount);

            Assert.AreEqual(generalAccount, transaction.AccountAssociated);
        }
        
        [TestMethod]

        public void WhenGivenATransaction_ShouldReturnANewTransactionWithSameValue()
        {
            string expectedTitle = "Trabajo";
            double expectedAmount = 100.0;
            double initialAmmount = 50.0;
            DateTime oldDate = new DateTime(2022, 09, 18);
            Category category = new Category("Trabajo",oldDate,CategoryType.IncomeCategory,StatusType.Active);
            ExchangeRate exchangeRate = new ExchangeRate(oldDate, 40, Currency.Dollar);
            GeneralAccount generalAccount = new GeneralAccount("Banco Santander", Currency.Dollar, oldDate, initialAmmount);
            Transaction transaction = new Transaction(expectedTitle, oldDate, expectedAmount, exchangeRate, category, generalAccount);

            Transaction newTransaction = transaction.DuplicateTransaction();
            newTransaction.Date=oldDate;

            Assert.AreEqual(transaction.Title, newTransaction.Title);
            Assert.AreEqual(transaction.Amount, newTransaction.Amount);
            Assert.AreEqual(transaction.CategoryAssociated, newTransaction.CategoryAssociated);
            Assert.AreEqual(transaction.AccountAssociated, newTransaction.AccountAssociated);
            Assert.AreEqual(transaction.Date, newTransaction.Date);

        }
    }
}