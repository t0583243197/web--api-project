using WebApplication2.Models.DTO;

namespace WebApplication2.DAL
{
    public interface ICategoryDal
    {
        List<CategoryDTO> GetAll();
        void Add(CategoryDTO category);
        void Update(CategoryDTO category); // חדש
        void Delete(int id); // חדש
    }
}