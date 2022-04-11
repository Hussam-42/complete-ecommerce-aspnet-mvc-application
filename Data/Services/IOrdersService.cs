using eTicket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eTicket.Data.Services
{
    public interface IOrdersService
    {
        public Task StoreOrderAsync(string UsrId, string UsrEmail, List<ShoppingCartItem> Items);
        public Task<List<Order>> GetOrdersByUserIdAsync(string UsrId);
    }

}
