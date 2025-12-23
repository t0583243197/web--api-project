using AutoMapper;
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

        public void Add(donorDTO newDonor)
        {
            var donor = _mapper.Map<DonorModel>(newDonor); // Fixed type name here
            _context.Donors.Add(donor);
            _context.SaveChanges();
        }

        public void Update(donorDTO donorDto)
        {
            var existingDonor = _context.Donors.Find(donorDto.Id);
            if (existingDonor != null)
            {
                _mapper.Map(donorDto, existingDonor); // מעדכן את השדות הקיימים ב-DB
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