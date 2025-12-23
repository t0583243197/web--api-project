using WebApplication2.Models.DTO;
using System.Collections.Generic;

namespace WebApplication2.DAL
{
    public interface ITicketDal
    {
        List<TicketDTO> GetAll();
        TicketDTO GetById(int id);
        void Add(TicketDTO ticket);
        void Update(TicketDTO ticket);
        void Delete(int id);
    }
}
