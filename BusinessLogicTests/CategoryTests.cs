using Models;
using Models.Enums;
using Models.Exceptions;

namespace DataAccessTests
{
    [TestClass]
    public class CategoryTests
    {
        [TestMethod]
        public void WhenCreatingANewCategory_ShouldReturnTheNewCategory()
        {
            DateTime todayDate = new DateTime(2023, 09, 18);
            CategoryType expectedType = CategoryType.IncomeCategory;
            StatusType expectedStatus = StatusType.Active;
            Category category = new Category("Cine", todayDate, expectedType, expectedStatus);
            ICollection<SpendingGoal> goals = new List<SpendingGoal>();
            ICollection < Category > cats = new List<Category>();
            int userId = 1;
            

            SpendingGoal goal1 = new SpendingGoal("Menos Noche", 6000, Currency.UruguayanPeso, cats);
            goals.Add(goal1);
            category._spendingGoals = goals;
            category.UserId = userId;
            string expectedName = "Cine";

            Assert.AreEqual(expectedName, category.Name);
            Assert.AreEqual(todayDate, category.CreationDate);
            Assert.AreEqual(expectedType, category.TypeOf);
            Assert.AreEqual(expectedStatus, category.Status);
            Assert.AreEqual(goals, category._spendingGoals);
            Assert.AreEqual(userId, category.UserId);
        }

        [TestMethod]
        [ExpectedException(typeof(CategoryExceptions))]
        public void WhenCreatingANewCategoryWithEmptyName_ShouldReturnACategoryException()
        {
            string emptyCategoryName = "";
            DateTime todayDate = new DateTime(2023, 09, 18);
            CategoryType expectedType = CategoryType.IncomeCategory;
            StatusType expectedStatus = StatusType.Active;

            Category category = new Category(emptyCategoryName, todayDate, expectedType, expectedStatus);

        }

        [TestMethod]
        [ExpectedException(typeof(CategoryExceptions))]

        public void WhenCreatingANewCategoryWithInactiveStatusAndTransactionsAssociated_ShouldReturnACategoryException()
        {
            DateTime todayDate = new DateTime(2023, 09, 18);
            CategoryType expectedType = CategoryType.IncomeCategory;
            StatusType expectedStatus = StatusType.Active;
            Account account = new Account("Banco Santander", Currency.UruguayanPeso, todayDate);
            ExchangeRate exchangeRate = new ExchangeRate(todayDate, 1, Currency.UruguayanPeso);
            List<Transaction> transactions = new List<Transaction>();
            
            Category category = new Category("Cine", todayDate, expectedType, expectedStatus);
            Transaction transaction = new Transaction("Cine", todayDate, 100.0, exchangeRate, category, account);
            transactions.Add(transaction);
            category.Transactions = transactions;
            category.Status = StatusType.Inactive;
        }

    }
}