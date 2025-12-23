using WebApplication2.DAL;
using WebApplication2.Models.DTO;

public class CategoryServiceBLL : ICategoryBLL
{
    private readonly ICategoryDal _categoryDal;
    public CategoryServiceBLL(ICategoryDal categoryDal) => _categoryDal = categoryDal;

    public List<CategoryDTO> GetAllCategories() => _categoryDal.GetAll();
    public void AddCategory(CategoryDTO category) => _categoryDal.Add(category);
    public void UpdateCategory(CategoryDTO category) => _categoryDal.Update(category);
    public void DeleteCategory(int id) => _categoryDal.Delete(id);
}