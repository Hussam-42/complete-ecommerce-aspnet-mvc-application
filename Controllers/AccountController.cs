using eTicket.Data;
using eTicket.Data.Static;
using eTicket.Models;
using eTicket.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace eTicket.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Login() => View(new LoginVM());

        [HttpPost]

        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

            if (user != null)
            {
                var pass = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                if (pass.Succeeded)
                {
                    return RedirectToAction("Index", "Movies");
                }
            }
            TempData["Error"] = "Wrong credentials, please try again!";
            return View(loginVM);
        }

        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);

            if (user != null)
            {
                TempData["Error"] = "this email address is already in use";
                return View(registerVM);
            }

            var newUser = new ApplicationUser()
            {
                Email = registerVM.EmailAddress,
                FullName = registerVM.FullName,
                UserName = registerVM.EmailAddress
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return View("RegisterCompleted");
        }



        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Movies");
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();

            return View(users);
        }

        public IActionResult AccessDenied(string ReturnedURL)
        {

            return View();
        }
    }
}