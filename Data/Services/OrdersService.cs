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

        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string UsrId, string UserRole)
        {
            var orders = await _context.Orders.Include(o => o.OrderItems)
                                              .ThenInclude(oi => oi.Movie)
                                              .Include(oi => oi.User)
                                              .ToListAsync();

            if(UserRole != "Admin")
            {
                orders = orders.Where(o => o.UserId == UsrId).ToList();
            }

            return orders;
        }



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
