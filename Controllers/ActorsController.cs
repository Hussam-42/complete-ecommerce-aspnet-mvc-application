using eTicket.Data;
using eTicket.Data.Services;
using eTicket.Data.Static;
using eTicket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace eTicket.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ActorsController : Controller
    {
        private readonly IActorsService _service;

        public ActorsController(IActorsService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName, ProfilePictureURL, Bio")]Actor actor)
        {
            if(!ModelState.IsValid)
            {
                return View(actor);
            }

            await _service.AddAsync(actor);
            return RedirectToAction(nameof(Index));
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
		{
            var ActorDetails = await _service.GetByIdAsync(id);
            
            if(ActorDetails == null) return View("NotFound");

            return View(ActorDetails);
		}


        public async Task<IActionResult> Edit(int id)
		{
            var ActorDetails = await _service.GetByIdAsync(id);

            if (ActorDetails == null) return View("NotFound");

            return View(ActorDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, FullName, ProfilePictureURL, Bio")] Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }

            await _service.UpdateAsync(id, actor);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            var ActorDetails = await _service.GetByIdAsync(id);

            if (ActorDetails == null) return View("NotFound");

            return View(ActorDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ActorDetails = await _service.GetByIdAsync(id);

            if (ActorDetails == null) return View("NotFound");

            await _service.DeleteAsync(id);

            return RedirectToAction(nameof(Index));

        }
    }
}
