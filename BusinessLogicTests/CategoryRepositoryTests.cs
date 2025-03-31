using DataAccess.Context;
using DataAccess.Repository;
using Models;
using Models.Enums;
using Models.Exceptions;

namespace DataAccessTests
{
    [TestClass]
    public class CategoryRepositoryTests
    {
        private CategoryRepository _repository;
        private ApplicationDbContext _context;
        private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    
        [TestInitialize]
        public void SetUp()
        {
            _context = _contextFactory.CreateDbContext();
            _repository = new CategoryRepository(_context);
        }
    
        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
        
        [TestMethod]
        public void WhenAddingANewCategory_ShouldAddTheNewCategoryInCategoryTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Category category = new Category("Category1",DateTime.Now,CategoryType.ExpensesCategory,StatusType.Active);

            _repository.AddCategory(category,1);

            var categoryInDb = _context.Categories.First();
            
            Assert.AreEqual(category, categoryInDb);
        }
        
        [TestMethod]
        [ExpectedException(typeof(RepositoryExceptions))]
        public void WhenAddingAnExistingCategory_ShouldReturnARepositoryException()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Category category = new Category("Category1",DateTime.Now,CategoryType.ExpensesCategory,StatusType.Active);

            _repository.AddCategory(category,1);
            
            _repository.AddCategory(category,1);
        }

        [TestMethod]
        public void WhenCategoryRepositoryFindsACategoryByName_ShouldReturnTheCategoryAssociated()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Category category = new Category("Category",DateTime.Today,CategoryType.ExpensesCategory,StatusType.Active);

            _repository.AddCategory(category,1);

            Category categoryInDb = _repository.FindCategoryByName(category.Name, 1);
            
            Assert.AreEqual(category, categoryInDb);
        }
        
        [TestMethod]
        public void WhenCategoryRepositoryUpdatesACategory_ShouldReturnTheUpdatedCategory()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Category category = new Category("Category",DateTime.Today,CategoryType.ExpensesCategory,StatusType.Inactive);

            _repository.AddCategory(category,1);

            Category categoryInDb = _repository.FindCategoryByName(category.Name, 1);
            
            categoryInDb.Name = "NewCategory";
            categoryInDb.Status = StatusType.Active;
            
            _repository.UpdateCategory(categoryInDb);
            
            Assert.AreEqual(categoryInDb.Name,category.Name);
            Assert.AreEqual(categoryInDb.Status,category.Status);
        }

        [TestMethod]
        public void WhenCategoryRepositoryListsAllTheCategories_ShouldReturnAllTheCategoriesInTheDatabase()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Category category = new Category("Category",DateTime.Today,CategoryType.ExpensesCategory,StatusType.Inactive);

            _repository.AddCategory(category,1);
            
            List<Category> categories = _repository.GetCategories(user.Id);
            
            Assert.AreEqual(1,categories.Count);
        }
        
        [TestMethod]
        public void WhenCategoryRepositoryDeletesACategory_ShouldDeleteTheCategoryFromTheCategoriesTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Category category = new Category("Category",DateTime.Today,CategoryType.ExpensesCategory,StatusType.Inactive);

            _repository.AddCategory(category,1);
            
            _repository.DeleteCategory(category.Name, 1);
            
            Assert.IsFalse(_repository.CategoryAlreadyExists(category,1));
        }
        
        [TestMethod]
        public void WhenCategoryRepositoryFindsACategoryById_ShouldReturnTheCategoryWithTheAssociatedId()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Category category = new Category("Category",DateTime.Today,CategoryType.ExpensesCategory,StatusType.Inactive);
            category.Id = 1;
            
            _repository.AddCategory(category,1);

            Category expectedCategory = _repository.FindCategoryById(category.Id);
            
            Assert.AreEqual(category,expectedCategory);
        }
    }
}