using Models;
using DataAccess.Context;
using Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class CategoryRepository
    {
        private ApplicationDbContext _database;

        public CategoryRepository(ApplicationDbContext database)
        {
            _database = database;
        }
        public void AddCategory(Category category, int userId)
        {
            if (CategoryAlreadyExists(category,userId))
            {
                CategoryAlreadyExistsException();
            }

            AddNewCategoryToCategoryTable(category, userId);

        }

        private static void CategoryAlreadyExistsException()
        {
            throw new RepositoryExceptions("The category already exists");
        }

        private void AddNewCategoryToCategoryTable(Category category, int userId)
        {
            category.UserId = userId;
            _database.Categories.Add(category);
            _database.SaveChanges();
        }
        public bool CategoryAlreadyExists(Category newCategory, int userId)
        {
            return _database.Categories.Any(category => category.Name == newCategory.Name && category.UserId == userId);
        }

        public Category FindCategoryByName(string name, int userId)
        {
            Category cat = _database.Categories.FirstOrDefault(category => category.Name == name && category.UserId == userId);
            return cat;
        }

        public void UpdateCategory(Category category)
        {
            var dbCategory = _database.Categories.FirstOrDefault(C => C.Id == category.Id);
            if (dbCategory != null)
            {
                dbCategory.Name = category.Name;
                dbCategory.Status = category.Status;
                _database.SaveChanges();
            }
        }
        
        public List<Category> GetCategories(int userId)
        {
            User user = _database.Users.FirstOrDefault(u => u.Id == userId);
            return _database.Categories.Where(c => c.UserId == userId).ToList();
        }
        
        public void DeleteCategory(string categoryName, int userId)
        {
            Category dbCategory = _database.Categories.FirstOrDefault(c=> c.Name == categoryName && c.UserId == userId);
            if(dbCategory != null)
            {
                _database.Categories.Remove(dbCategory);
                _database.SaveChanges();
            }

        }

        public Category FindCategoryById(int id)
        {
            Category category = _database.Categories.Include(s => s.Transactions).FirstOrDefault(c => c.Id == id);
            return category;
        }
    }
}