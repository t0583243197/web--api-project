using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models.DTO;

namespace WebApplication2.DAL
{
    public interface ICategoryDal
    {
        Task<List<CategoryDTO>> GetAll();
        Task Add(CategoryDTO category);
        Task Update(CategoryDTO category);
        Task Delete(int id);
    }
}