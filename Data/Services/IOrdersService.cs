using eTicket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eTicket.Data.Services
{
    public interface IOrdersService
    {
        public Task StoreOrderAsync(string UsrId, string UsrEmail, List<ShoppingCartItem> Items);

        public Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string UsrId, string UserRole);
    }

}
