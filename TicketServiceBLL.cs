using WebApplication2.DAL;
using WebApplication2.Models.DTO;
using System.Collections.Generic;

namespace WebApplication2.BLL
{
    public class TicketServiceBLL : ITicketBLL
    {
        private readonly ITicketDal _ticketDal;

        public TicketServiceBLL(ITicketDal ticketDal)
        {
            _ticketDal = ticketDal;
        }

        public List<TicketDTO> GetAllTickets() => _ticketDal.GetAll();

        public TicketDTO GetTicket(int id) => _ticketDal.GetById(id);

        public void AddTicket(TicketDTO ticket) => _ticketDal.Add(ticket);

        public void UpdateTicket(TicketDTO ticket) => _ticketDal.Update(ticket);

        public void DeleteTicket(int id) => _ticketDal.Delete(id);
    }
}
