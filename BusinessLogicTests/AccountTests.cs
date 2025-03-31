using Models;
using Models.Enums;
using Models.Exceptions;

namespace DataAccessTests
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void WhenCreatingANewAccount_ShouldReturnTheNewAccount()
        {
            string accountName = "Caja de Ahorros Santander";
            Currency accountCurrency = Currency.UruguayanPeso;
            DateTime todayDate = DateTime.Now;
            int userId = 1;

            Account account = new Account(accountName, accountCurrency, todayDate);
            account.UserId = userId;

            Assert.AreEqual(accountName, account.Name);
            Assert.AreEqual(accountCurrency, account.AccountCurrency);
            Assert.AreEqual(todayDate, account.CreationDate);
            Assert.AreEqual(userId, account.UserId);

        }

        [TestMethod]
        public void WhenCreatingANewAccount_ShouldReturnTheTransactionsAssociatedToTheAccount()
        {
            string accountName = "Caja de Ahorros Santander";
            Currency accountCurrency = Currency.UruguayanPeso;
            DateTime todayDate = DateTime.Now;
            int userId = 1;

            Account account = new Account(accountName, accountCurrency, todayDate);
            account.UserId = userId;

            ExchangeRate rate = new ExchangeRate(todayDate, 40, Currency.UruguayanPeso);
            Category category = new Category("Category", todayDate, CategoryType.ExpensesCategory, StatusType.Active);
            Transaction transaction = new Transaction("Transaction", todayDate, 100.0, rate, category, account);
            ICollection <Transaction> transactions = new List<Transaction>();
            transactions.Add(transaction);

            account._transactions = transactions;

            Assert.AreEqual(transactions, account._transactions);

        }

        [TestMethod]
        [ExpectedException(typeof(AccountExceptions))]
        public void WhenCreatingANewAccountWithEmptyName_ShouldReturnAnAccountException()
        {
            string accountName = "";
            Currency accountCurrency = Currency.UruguayanPeso;
            DateTime todayDate = DateTime.Now;

            Account account = new Account(accountName, accountCurrency, todayDate);
        }
    }
}