using eTicket.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTicket.Data.Services
{
    public class OrdersService : IOrdersService
    {

        private readonly AppDbContext _context;

        public OrdersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string UsrId) =>

            await _context.Orders.Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.Movie)
                                 .Where(o => o.UserId == UsrId)
                                 .ToListAsync();

        public async Task StoreOrderAsync(string UsrId, string UsrEmail, List<ShoppingCartItem> Items)
        {
            var NewOrder = new Order()
            {
                UserId = UsrId,
                Email = UsrEmail,
            };

            await _context.Orders.AddAsync(NewOrder);
            await _context.SaveChangesAsync();

            foreach (var item in Items)
            {
                var NewOrderItem = new OrderItem()
                {
                    OrderId = NewOrder.Id,
                    Price = item.Movie.Price,
                    Amount = item.Amount,
                    MovieId = item.Movie.Id
                };
                await _context.OrderItems.AddAsync(NewOrderItem);
            }
            await _context.SaveChangesAsync();
        }
    }
}
