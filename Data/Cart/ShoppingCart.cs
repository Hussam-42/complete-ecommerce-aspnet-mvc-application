using eTicket.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTicket.Data.Cart
{
    public class ShoppingCart
    {

        private readonly AppDbContext _context;

        public ShoppingCart(AppDbContext context)
        {
            _context = context;
        }
        
        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<AppDbContext>();

            string cartId = session.GetString("cardId") ?? Guid.NewGuid().ToString();
            session.SetString("cardId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId};
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }


        public void AddItemToCart(Movie movie)
        {
            ShoppingCartItem CartItem = _context.ShoppingCartItems.FirstOrDefault(sh => sh.ShoppingCartId == ShoppingCartId && sh.Movie.Id == movie.Id);

            if (CartItem == null)
            {
                CartItem = new()
                {
                    ShoppingCartId = ShoppingCartId,
                    Movie = movie,
                    Amount = 1
                };
                _context.ShoppingCartItems.Add(CartItem);
            }
            else
            {
                CartItem.Amount++;
            }
            _context.SaveChanges();
        }

        public void RemoveItemFromCart(Movie movie)
        {
            ShoppingCartItem CartItem = _context.ShoppingCartItems.FirstOrDefault(sh => sh.ShoppingCartId == ShoppingCartId && sh.Movie.Id == movie.Id);

            if (CartItem != null)
            {
                if(CartItem.Amount > 1)
                    CartItem.Amount--;
                else
                    _context.ShoppingCartItems.Remove(CartItem);
            }
            _context.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems = _context.ShoppingCartItems.Where(i => i.ShoppingCartId == ShoppingCartId).Include(i => i.Movie).ToList());

        }

        public async Task ClearShoppingCartAsync()
        {

            var ShoppCartitems = await _context.ShoppingCartItems.Where(i => i.ShoppingCartId == ShoppingCartId).Include(i => i.Movie).ToListAsync();
            _context.ShoppingCartItems.RemoveRange(ShoppCartitems);
            await _context.SaveChangesAsync();

            ShoppCartitems = new List<ShoppingCartItem>();
        }

        public double GetShoppingCartTotal() => _context.ShoppingCartItems.Where(i => i.ShoppingCartId == ShoppingCartId).Select(i => i.Movie.Price * i.Amount).Sum();


    }
}
