using WebApplication2.Models.DTO;

public interface ICategoryBLL
{
    Task<List<CategoryDTO>> GetAllCategories();
    Task AddCategory(CategoryDTO category);
    Task UpdateCategory(CategoryDTO category);
    Task DeleteCategory(int id);
}