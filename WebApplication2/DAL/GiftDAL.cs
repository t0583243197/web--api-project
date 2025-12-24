using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.DTO;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.DAL
{
    public class GiftDAL : IGiftDal
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public GiftDAL(StoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GiftDTO>> GetByFilterAsync(string? name, string? donorName, int? minPurchasers)
        {
            var query = _context.Gifts
                .Include(g => g.Donor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(g => EF.Functions.Like(g.Name, $"%{name}%"));

            if (!string.IsNullOrEmpty(donorName))
                query = query.Where(g => EF.Functions.Like(g.Donor.Name, $"%{donorName}%"));

            if (minPurchasers.HasValue)
            {
                var min = minPurchasers.Value;
                query = query.Where(g =>
                    _context.OrderTicket
                        .Where(t => t.GiftId == g.Id)
                        .Sum(t => (int?)t.Quantity) >= min); // cast to nullable to handle empty sets
            }

            return await query
                .ProjectTo<GiftDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<List<GiftDTO>> GetGiftsSortedByPriceAsync() =>
            _context.Gifts
                .OrderByDescending(g => g.TicketPrice)
                .ProjectTo<GiftDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public Task<List<GiftDTO>> GetMostPurchasedGiftsAsync() =>
            _context.Gifts
                .OrderByDescending(g => _context.OrderTicket.Where(t => t.GiftId == g.Id).Sum(t => t.Quantity))
                .ProjectTo<GiftDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task AddAsync(GiftDTO giftDto)
        {
            // map basic fields
            var gift = _mapper.Map<GiftModel>(giftDto);

            // resolve category by name
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == giftDto.Category);
            if (category == null)
            {
                // either create new category or return error
                category = new CategoryModel { Name = giftDto.Category };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync(); // now category.Id is set
            }

            gift.CategoryId = category.Id;
            gift.Category = null; // avoid EF interpreting it as a new entity

            // resolve donor similarly (by name) or ensure DonorId is set
            var donor = await _context.Donors.FirstOrDefaultAsync(d => d.Name == giftDto.DonorName);
            if (donor != null) gift.DonorId = donor.Id;

            _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GiftDTO giftDto)
        {
            var existingGift = await _context.Gifts.FindAsync(giftDto.Id);
            if (existingGift != null)
            {
                _mapper.Map(giftDto, existingGift);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift != null)
            {
                _context.Gifts.Remove(gift);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<GiftDTO>> GetAllAsync()
        {
            var gifts = await _context.Gifts
                .Include(g => g.Donor)
                .ToListAsync();
            return _mapper.Map<List<GiftDTO>>(gifts);
        }
    }
}