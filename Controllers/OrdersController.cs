using eTicket.Data.Cart;
using eTicket.Data.Services;
using eTicket.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eTicket.Controllers
{

    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IMoviesService _MovieService;
        private readonly ShoppingCart _ShoppingCart;
        private readonly IOrdersService _ordersService;
        public OrdersController(IMoviesService MovieService, ShoppingCart ShoppingCart, IOrdersService ordersService)
        {
            _MovieService = MovieService;
            _ShoppingCart = ShoppingCart;
            _ordersService = ordersService;
        }


        public async Task<IActionResult> Index()
        {

            var UsrId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var UserRole = User.FindFirstValue(ClaimTypes.Role);

            var Orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(UsrId, UserRole);

            return View(Orders);
        }

        public IActionResult ShoppingCart()
        {
            var Items = _ShoppingCart.GetShoppingCartItems();
            _ShoppingCart.ShoppingCartItems = Items;

            var shoppingCartvm = new ShoppingCartVM()
            {
                ShoppingCart = _ShoppingCart,
                ShoppingCartTotal = _ShoppingCart.GetShoppingCartTotal()
            };

            return View(shoppingCartvm);
        }

        public async Task<RedirectToActionResult> AddItemToShoppingCart(int id)
        {
            var movie = await _MovieService.GetMovieByIdAsync(id);

            if(movie != null)
            {
                _ShoppingCart.AddItemToCart(movie);
                return RedirectToAction(nameof(ShoppingCart));
            }
            else
            {
                return RedirectToAction(nameof(ShoppingCart));
            }
        }

        public async Task<RedirectToActionResult> RemoveItemFromShoppingCart(int id)
        {
            var movie = await _MovieService.GetMovieByIdAsync(id);

            if (movie != null)
            {
                _ShoppingCart.RemoveItemFromCart(movie);
            }
            return RedirectToAction(nameof(ShoppingCart));

        }

        public async Task<IActionResult> OrderCompleted()
        {

            var items = _ShoppingCart.GetShoppingCartItems();

            var UsrId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var UsrEmail = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(UsrId, UsrEmail, items);

            await _ShoppingCart.ClearShoppingCartAsync();


            return View("OrderCompleted");
        }
    }
}
