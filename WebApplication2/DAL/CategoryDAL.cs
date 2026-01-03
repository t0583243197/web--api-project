using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.Models.DTO;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class CategoryDAL : ICategoryDal
{
    private readonly StoreContext _context;
    private readonly IMapper _mapper;

    public CategoryDAL(StoreContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Read-only: use ProjectTo + AsNoTracking
    public async Task<List<CategoryDTO>> GetAll() =>
        await _context.Categories
                .AsNoTracking()
                .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

    public async Task Add(CategoryDTO categoryDto)
    {
        var category = _mapper.Map<CategoryModel>(categoryDto);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task Update(CategoryDTO categoryDto)
    {
        var existingCategory = await _context.Categories.FindAsync(categoryDto.Id);
        if (existingCategory != null)
        {
            _mapper.Map(categoryDto, existingCategory);
            await _context.SaveChangesAsync();
        }
    }

    // Connected partial update for Category:
    // Example: await UpdatePartialAsync(categoryId, c => c.Name = "new", c => c.Name);
    public async Task UpdatePartialAsync(int id, Action<CategoryModel> setValues, params Expression<Func<CategoryModel, object>>[] modifiedProperties)
    {
        var entity = new CategoryModel { Id = id };
        _context.Categories.Attach(entity);
        setValues(entity);

        var entry = _context.Entry(entity);
        foreach (var prop in modifiedProperties)
        {
            var propName = GetPropertyName(prop);
            entry.Property(propName).IsModified = true;
        }

        await _context.SaveChangesAsync();
    }

    // Soft-delete: set IsDeleted so global query filters apply
    public async Task Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    // Helper method to extract property name from expression
    private static string GetPropertyName(Expression<Func<CategoryModel, object>> propertyExpression)
    {
        if (propertyExpression.Body is MemberExpression member)
            return member.Member.Name;
        if (propertyExpression.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
            return memberOperand.Member.Name;
        throw new ArgumentException("Invalid property expression", nameof(propertyExpression));
    }
}