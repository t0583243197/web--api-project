using AutoMapper;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

public class CategoryDAL : ICategoryDal
{
    private readonly StoreContext _context;
    private readonly IMapper _mapper;

    public CategoryDAL(StoreContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<CategoryDTO> GetAll() => _mapper.Map<List<CategoryDTO>>(_context.Categories.ToList());

    public void Add(CategoryDTO categoryDto)
    {
        var category = _mapper.Map<CategoryModel>(categoryDto);
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void Update(CategoryDTO categoryDto)
    {
        var existingCategory = _context.Categories.Find(categoryDto.Id);
        if (existingCategory != null)
        {
            _mapper.Map(categoryDto, existingCategory);
            _context.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}