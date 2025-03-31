using Models;
using Models.Enums;
using Models.Exceptions;

namespace DataAccessTests
{
    [TestClass]
    public class SpendingGoalTests
    {
        [TestMethod]

        public void WhenCreatingANewEmptySpendingGoal_ShouldReturnAnEmptySpendingGoal()
        {
            SpendingGoal spendingGoal = new SpendingGoal();

            Assert.AreEqual("", spendingGoal.Title);
            Assert.AreEqual(0, spendingGoal.MaximumAmount);
            Assert.AreEqual(null, spendingGoal.Categories);
        }
        
        [TestMethod]
        public void WhenCreatingANewSpendingGoal_ShouldReturnTheNewSpendingGoal()
        {
            Category category = new Category("Cine", DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);
            string expectedTitle = "Menos Cine";
            double expectedMaximumAmount = 2000;
            Currency goalCurrency = Currency.UruguayanPeso;
            
            SpendingGoal spendingGoal = new SpendingGoal(expectedTitle, expectedMaximumAmount,goalCurrency,categories);

            Assert.AreEqual(expectedTitle, spendingGoal.Title);
            Assert.AreEqual(expectedMaximumAmount, spendingGoal.MaximumAmount);
            Assert.AreEqual(categories, spendingGoal.Categories);
        }

        [TestMethod]
        public void WhenCreatingANewSpendingGoalWithMultiplesCategories_ShouldReturnTheNewSpendingGoal()
        {
            Category category = new Category("Cine", DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);
            Category category2 = new Category("Salidas de Noche", DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);
            Category category3 = new Category("Paseos con la novia", DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);
            Category category4 = new Category("Futbol con los gurises", DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);
            categories.Add(category2);
            categories.Add(category3);
            categories.Add(category4);
            string expectedTitle = "Menos Cine";
            double expectedMaximumAmount = 2000;
            Currency goalCurrency = Currency.UruguayanPeso;
            SpendingGoal spendingGoal = new SpendingGoal(expectedTitle, expectedMaximumAmount, goalCurrency,categories);

            Assert.AreEqual(expectedTitle, spendingGoal.Title);
            Assert.AreEqual(expectedMaximumAmount, spendingGoal.MaximumAmount);
            Assert.AreEqual(categories, spendingGoal.Categories);
        }

        [TestMethod]
        [ExpectedException(typeof(SpendingGoalExceptions))]
        public void WhenCreatingANewSpendingGoalWithEmptyTitle_ShouldReturnASpendingGoalException()
        {
            Category category = new Category("Cine", DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories = new List<Category>();
            Currency goalCurrency = Currency.UruguayanPeso;
            categories.Add(category);
            SpendingGoal spendingGoal = new SpendingGoal("", 2000, goalCurrency,categories);
        }
        
        
        
    }
}