using eTicket.Data;
using eTicket.Data.Services;
using eTicket.Data.Static;
using eTicket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace eTicket.Controllers
{

    [Authorize(Roles = UserRoles.Admin)]
    public class CinemasController : Controller
    {

        private readonly ICinemasService _service;

        public CinemasController(ICinemasService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {

            var AllCinemas = await _service.GetAllAsync();

            return View(AllCinemas);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Logo, Name, Description")] Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return View(cinema);
            }

            await _service.AddAsync(cinema);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var CinemaDetails = await _service.GetByIdAsync(id);

            if (CinemaDetails == null) return View("NotFound");

            return View(CinemaDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var CinemaDetails = await _service.GetByIdAsync(id);

            if (CinemaDetails == null) return View("NotFound");

            return View(CinemaDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Logo, Description")] Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return View(cinema);
            }

            await _service.UpdateAsync(id, cinema);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            var CinemaDetails = await _service.GetByIdAsync(id);

            if (CinemaDetails == null) return View("NotFound");

            return View(CinemaDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var CinemaDetails = await _service.GetByIdAsync(id);

            if (CinemaDetails == null) return View("NotFound");

            await _service.DeleteAsync(id);

            return RedirectToAction(nameof(Index));

        }
    }
}
