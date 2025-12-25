
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.DAL
{
    public class TicketDal : ITicketDAL
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;

        public TicketDal(StoreContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Get all purchase tickets
        public Task<List<TicketDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TicketDTO> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(TicketDTO ticketDto)
        {
            throw new NotImplementedException();
        }

        public Task<TicketDTO> Create(TicketDTO ticketDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

     
    }
}

