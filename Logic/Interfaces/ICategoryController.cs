using Logic.DTOs;
using Models;

namespace Logic.Interfaces
{
    public interface ICategoryController
    {
        public Category CreateCategory(CategoryDto categoryDto, int userId);
        public void AddCategory(CategoryDto categoryDto, int userId);
        public Category FindCategoryByName(string name, int userId);
        public void ChangeCategoryName(Category category, CategoryDto categoryDto);
        public void ChangeCategoryType(Category category, CategoryDto categoryDto);
        public void ChangeCategoryStatus(Category category, CategoryDto categoryDto);
        public void ChangeCategoryCreationDate(Category category, CategoryDto categoryDto);
        public List<CategoryDto> GetAllCategories(int userId);
        public void DeleteCategory(CategoryDto categoryDto, int userId);
        CategoryDto GetCategoryById(int id);
        void UpdateCategory(CategoryDto element, int userId);
        public List<CategoryDto> GetAllCategoriesForSpending(int userId);
    }
}
