using WebApplication2.DAL;
using WebApplication2.Models.DTO;
using System.Threading.Tasks;
using System.Collections.Generic; // Add this using directive

public class CategoryServiceBLL : ICategoryBLL
{
    private readonly ICategoryDal _categoryDal;

    public CategoryServiceBLL(ICategoryDal categoryDal)
    {
        _categoryDal = categoryDal;
    }

    public async Task<List<CategoryDTO>> GetAllCategories() => await _categoryDal.GetAll();
    public async Task AddCategory(CategoryDTO category) => await _categoryDal.Add(category);
    public async Task UpdateCategory(CategoryDTO category) => await _categoryDal.Update(category);
    public async Task DeleteCategory(int id) => await _categoryDal.Delete(id);
}