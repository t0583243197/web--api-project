using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Models.DTO;
using AutoMapper.QueryableExtensions;
using System;
using System.Linq.Expressions;

namespace WebApplication2.DAL
{
    public class DonorDAL : IDonorDal
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public DonorDAL(StoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Read-only: ProjectTo + AsNoTracking
        public async Task<List<DonorDTO>> GetAll()
        {
            return await _context.Donors
                .AsNoTracking()
                .ProjectTo<DonorDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // Read-only with filters: apply filters then ProjectTo
        public async Task<List<DonorDTO>> GetByFilter(string? name, string? email, string? giftName)
        {
            var query = _context.Donors.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(d => d.Name.Contains(name));

            if (!string.IsNullOrEmpty(email))
                query = query.Where(d => d.Email.Contains(email));

            if (!string.IsNullOrEmpty(giftName))
                query = query.Where(d => d.Gifts.Any(g => g.Name.Contains(giftName)));

            return await query
                .AsNoTracking()
                .ProjectTo<DonorDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task Add(DonorDTO newDonor)
        {
            var donor = _mapper.Map<DonorModel>(newDonor);
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
        }

        public async Task Update(DonorDTO donorDto)
        {
            var existingDonor = await _context.Donors.FindAsync(donorDto.Id);
            if (existingDonor != null)
            {
                _mapper.Map(donorDto, existingDonor);
                await _context.SaveChangesAsync();
            }
        }

        // Connected partial update for Donor:
        // Example: await UpdatePartialAsync(donorId, d => d.Address = "new address", d => d.Address);
        public async Task UpdatePartial(int id, Action<DonorModel> setValues, params Expression<Func<DonorModel, object>>[] modifiedProperties)
        {
            var entity = new DonorModel { Id = id };
            _context.Donors.Attach(entity);
            setValues(entity);

            var entry = _context.Entry(entity);
            foreach (var prop in modifiedProperties)
            {
                var propName = GetPropertyName(prop);
                entry.Property(propName).IsModified = true;
            }

            await _context.SaveChangesAsync();
        }

        // Soft-delete for Donor
        public async Task Delete(int id)
        {
            var donor = await _context.Donors.FindAsync(id);
            if (donor != null)
            {
                donor.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        private static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression member)
                return member.Member.Name;

            if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
                return memberOperand.Member.Name;

            throw new ArgumentException("Invalid expression");
        }
    }
}