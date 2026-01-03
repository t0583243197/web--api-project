using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions; // <-- Add this using directive
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WebApplication2.DAL
{
    public class UserDAL : IUserDal
    {
        private readonly IMapper _mapper;
        private readonly StoreContext _context;

        public UserDAL(StoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private static readonly ValueConverter<UserRole, string> _userRoleConverter = new ValueConverter<UserRole, string>(
            v => v.ToString(),
            v => Enum.Parse<UserRole>(v, true));

        public async Task Add(UserDto userDto)
        {
            if (!Enum.TryParse<UserRole>(userDto.Role, true, out _))
                throw new ArgumentException("Invalid role");

            var userModel = _mapper.Map<UserModel>(userDto);
            _context.Users.Add(userModel);
            await _context.SaveChangesAsync();
        }

        // Read-only: use ProjectTo and AsNoTracking.
        // Ensure AutoMapper mapping UserModel -> UserDto excludes Password so EF doesn't fetch it.
        public async Task<List<UserDto>> GetAll()
        {
                return await _context.Users
                .AsNoTracking()
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // Soft-delete for User
        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        // Connected partial update: update only the specified properties.
        // Example usage:
        // await UpdatePartialAsync(userId, u => u.Role = UserRole.Manager, u => u.Role);
        public async Task UpdatePartialAsync(int id, Action<UserModel> setValues, params Expression<Func<UserModel, object>>[] modifiedProperties)
        {
            var entity = new UserModel { Id = id };
            _context.Users.Attach(entity);
            setValues(entity);

            var entry = _context.Entry(entity);
            foreach (var prop in modifiedProperties)
            {
                var propName = GetPropertyName(prop);
                entry.Property(propName).IsModified = true;
            }

            await _context.SaveChangesAsync();
        }

        // Helper to extract property name from expression
        private static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression member)
                return member.Member.Name;

            if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
                return memberOperand.Member.Name;

            throw new ArgumentException("Invalid expression");
        }
     
        public async Task<UserModel> GetFullUserByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }
    }
}
