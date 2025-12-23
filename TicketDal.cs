using WebApplication2.Models.DTO;
using WebApplication2.Entities;
using WebApplication2.Data;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication2.DAL
{
    public class TicketDal : ITicketDal
    {
        private readonly ApplicationDbContext _context;

        public TicketDal(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TicketDTO> GetAll() =>
            _context.Tickets
                .Select(t => new TicketDTO
                {
                    Id = t.Id,
                    GiftId = t.GiftId,
                    UserId = t.UserId,
                    PurchaseDate = t.PurchaseDate,
                    IsUsed = t.IsUsed
                }).ToList();

        public TicketDTO GetById(int id)
        {
            var t = _context.Tickets.Find(id);
            if (t == null) return null;
            return new TicketDTO
            {
                Id = t.Id,
                GiftId = t.GiftId,
                UserId = t.UserId,
                PurchaseDate = t.PurchaseDate,
                IsUsed = t.IsUsed
            };
        }

        public void Add(TicketDTO ticket)
        {
            var entity = new Ticket
            {
                GiftId = ticket.GiftId,
                UserId = ticket.UserId,
                PurchaseDate = ticket.PurchaseDate,
                IsUsed = ticket.IsUsed
            };
            _context.Tickets.Add(entity);
            _context.SaveChanges();
            ticket.Id = entity.Id;
        }

        public void Update(TicketDTO ticket)
        {
            var entity = _context.Tickets.Find(ticket.Id);
            if (entity == null) return;
            entity.GiftId = ticket.GiftId;
            entity.UserId = ticket.UserId;
            entity.PurchaseDate = ticket.PurchaseDate;
            entity.IsUsed = ticket.IsUsed;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Tickets.Find(id);
            if (entity == null) return;
            _context.Tickets.Remove(entity);
            _context.SaveChanges();
        }
    }
}