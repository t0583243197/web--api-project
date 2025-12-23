using WebApplication2.Models.DTO;
using System.Collections.Generic;

namespace WebApplication2.BLL
{
    public interface ITicketBLL
    {
        List<TicketDTO> GetAllTickets();
        TicketDTO GetTicket(int id);
        void AddTicket(TicketDTO ticket);
        void UpdateTicket(TicketDTO ticket);
        void DeleteTicket(int id);
    }
}
