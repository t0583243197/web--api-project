using WebApplication2.Models.DTO;

public interface ICategoryBLL
{
    List<CategoryDTO> GetAllCategories();
    void AddCategory(CategoryDTO category);
    void UpdateCategory(CategoryDTO category);
    void DeleteCategory(int id);
}