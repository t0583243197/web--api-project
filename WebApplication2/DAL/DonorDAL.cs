using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

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

        public async Task<List<DonorDTO>> GetAllAsync()
        {
            var donors = await _context.Donors
                .Include(d => d.Gifts)
                .ThenInclude(g => g.Category) // Include the Category for each Gift
                .ToListAsync();
            return _mapper.Map<List<DonorDTO>>(donors);
        }

        public async Task<List<DonorDTO>> GetByFilterAsync(string? name, string? email, string? giftName)
        {
            var query = _context.Donors
                .Include(d => d.Gifts)
                .ThenInclude(g => g.Category) // Include the Category for each Gift
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(d => d.Name.Contains(name));

            if (!string.IsNullOrEmpty(email))
                query = query.Where(d => d.Email.Contains(email));

            if (!string.IsNullOrEmpty(giftName))
                query = query.Where(d => d.Gifts.Any(g => g.Name.Contains(giftName)));

            var results = await query.ToListAsync();
            return _mapper.Map<List<DonorDTO>>(results);
        }

        public async Task AddAsync(DonorDTO newDonor)
        {
            var donor = _mapper.Map<DonorModel>(newDonor);
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DonorDTO donorDto)
        {
            var existingDonor = await _context.Donors.FindAsync(donorDto.Id);
            if (existingDonor != null)
            {
                _mapper.Map(donorDto, existingDonor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var donor = await _context.Donors.FindAsync(id);
            if (donor != null)
            {
                _context.Donors.Remove(donor);
                await _context.SaveChangesAsync();
            }
        }
    }
}