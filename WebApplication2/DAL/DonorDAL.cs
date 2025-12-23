using AutoMapper;
using Microsoft.EntityFrameworkCore; // נדרש עבור Include
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

        public List<donorDTO> GetAll() => _mapper.Map<List<donorDTO>>(_context.Donors.ToList());

        // מימוש הסינון
        public List<donorDTO> GetByFilter(string? name, string? email, string? giftName)
        {
            // מתחילים משאילתה הכוללת את המתנות של התורם
            var query = _context.Donors.Include(d => d.Gifts).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(d => d.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(d => d.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(giftName))
            {
                // סינון תורמים שתרמו לפחות מתנה אחת עם השם המבוקש
                query = query.Where(d => d.Gifts.Any(g => g.Name.Contains(giftName)));
            }

            var results = query.ToList();
            return _mapper.Map<List<donorDTO>>(results);
        }

        public void Add(donorDTO newDonor)
        {

            var donor = _mapper.Map<DonorModel>(newDonor);

            _context.Donors.Add(donor);
            _context.SaveChanges();
        }

        public void Update(donorDTO donorDto)
        {
            var existingDonor = _context.Donors.Find(donorDto.Id);
            if (existingDonor != null)
            {
                _mapper.Map(donorDto, existingDonor);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var donor = _context.Donors.Find(id);
            if (donor != null)
            {
                _context.Donors.Remove(donor);
                _context.SaveChanges();
            }
        }
    }
}