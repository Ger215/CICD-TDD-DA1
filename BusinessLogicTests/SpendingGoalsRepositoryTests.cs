using DataAccess.Context;
using DataAccess.Repository;
using Models;
using Models.Enums;
using Models.Exceptions;

namespace DataAccessTests
{
    [TestClass]
    public class SpendingGoalsRepositoryTests
    {
        private SpendingGoalsRepository _repository;
        private ApplicationDbContext _context;
        private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    
        [TestInitialize]
        public void SetUp()
        {
            _context = _contextFactory.CreateDbContext();
            _repository = new SpendingGoalsRepository(_context);
        }
    
        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
        
        [TestMethod]
        public void WhenAddingANewSpendingGoal_ShouldAddTheNewSpendingGoalInSpendingGoalsTable()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Category category = new Category("Salidas",DateTime.Today, CategoryType.ExpensesCategory,StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);

            Currency goalCurrency = Currency.UruguayanPeso;
            
            SpendingGoal spendingGoal = new SpendingGoal("Menos Noche", 6000, goalCurrency,categories);
            spendingGoal.Id = 1;
            
            _repository.AddSpendingGoal(spendingGoal, user.Id);
            
            SpendingGoal spendingGoalInDb = _repository.FindSpendingGoalById(1);
            
            Assert.AreEqual(spendingGoal, spendingGoalInDb);
        }
        
        [TestMethod]
        [ExpectedException(typeof(RepositoryExceptions))]
        public void WhenAddingAnExistingSpendingGoal_ShouldReturnARepositoryException()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Category category = new Category("Salidas",DateTime.Today, CategoryType.ExpensesCategory,StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);

            Currency goalCurrency = Currency.UruguayanPeso;
            
            SpendingGoal spendingGoal = new SpendingGoal("Menos Noche", 6000, goalCurrency,categories);
            spendingGoal.Id = 1;
            
            _repository.AddSpendingGoal(spendingGoal, user.Id);
            
            _repository.AddSpendingGoal(spendingGoal, user.Id);
        }
        
        [TestMethod]
        public void WhenSpendingGoalsRepositoryFindsAnSpendingGoalById_ShouldReturnTheSpendingGoalWithTheAssociatedId()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Category category = new Category("Salidas",DateTime.Today, CategoryType.ExpensesCategory,StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);

            Currency goalCurrency = Currency.UruguayanPeso;
            
            SpendingGoal spendingGoal = new SpendingGoal("Menos Noche", 6000,goalCurrency,categories);
            spendingGoal.Id = 1;
            
            _repository.AddSpendingGoal(spendingGoal,user.Id);
            
            SpendingGoal spendingGoalInDb = _repository.FindSpendingGoalById(1);
            
            Assert.AreEqual(spendingGoal, spendingGoalInDb);
        }
        
        [TestMethod]
        public void WhenSpendingGoalsRepositoryListsAllTheSpendingGoals_ShouldReturnAllTheSpendingGoalsInTheDatabase()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Category category = new Category("Salidas",DateTime.Today, CategoryType.ExpensesCategory,StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);
            
            Currency goalCurrency = Currency.UruguayanPeso;

            SpendingGoal spendingGoal = new SpendingGoal("Menos Noche", 6000, goalCurrency,categories);
            spendingGoal.Id = 1;
            
            _repository.AddSpendingGoal(spendingGoal,user.Id);
            
            List<SpendingGoal> spendingGoals = _repository.GetSpendingGoals(user.Id);
            
            Assert.AreEqual(1, spendingGoals.Count);
        }
        
        [TestMethod]
        public void WhenSpendingGoalsRepositoryUpdatesAnSpendingGoal_ShouldReturnTheUpdatedSpendingGoal()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Category category = new Category("Salidas",DateTime.Today, CategoryType.ExpensesCategory,StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);

            Currency goalCurrency = Currency.UruguayanPeso;
            
            SpendingGoal spendingGoal = new SpendingGoal("Menos Noche", 6000, goalCurrency,categories);
            spendingGoal.Id = 1;
            
            _repository.AddSpendingGoal(spendingGoal, user.Id);

            SpendingGoal spendingGoalInDb = _repository.FindSpendingGoalById(1);
            
            spendingGoalInDb.MaximumAmount = 10000;
            Category category2 = new Category("Salidas",DateTime.Today, CategoryType.ExpensesCategory,StatusType.Active);
            List<Category> categories2 = new List<Category>();
            categories2.Add(category2);
            spendingGoalInDb.Categories = categories2;
            
            _repository.UpdateSpendingGoal(spendingGoalInDb, user.Id);
            
            SpendingGoal updatedSpendingGoal = _repository.FindSpendingGoalById(1);
            
            Assert.AreEqual(10000, updatedSpendingGoal.MaximumAmount);
            Assert.AreEqual(categories2, updatedSpendingGoal.Categories);
        }
    }
}