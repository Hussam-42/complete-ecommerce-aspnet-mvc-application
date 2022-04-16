using eTicket.Data;
using eTicket.Data.Services;
using eTicket.Data.Static;
using eTicket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eTicket.Controllers
{
    
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;

        public MoviesController(IMoviesService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {

            var AllMovies = await _service.GetAllAsync(m => m.Cinema);

            return View(AllMovies);
        }

        [HttpPost]
        public async Task<IActionResult> Filter(string searchString)
        {
            var AllMovies = await _service.GetAllAsync(m => m.Cinema);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = AllMovies.Where(m => m.Name.Contains(searchString) || m.Description.Contains(searchString)).ToList();
                return View("Index",filteredResult);
            }

            return View("Index", AllMovies);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _service.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return View("Not Found");
            }
            else
            {
                return View(movie);
            }

        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Create()
        {
            var Dropdowns = await _service.GetAlldropdownsVM();

            ViewBag.Actors = new SelectList(Dropdowns.Actors, "Id", "FullName");
            ViewBag.Cinemas = new SelectList(Dropdowns.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(Dropdowns.Producers, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewMovieVM newMovie)
        {
            if (!ModelState.IsValid)
            {
                var Dropdowns = await _service.GetAlldropdownsVM();

                ViewBag.Actors = new SelectList(Dropdowns.Actors, "Id", "FullName");
                ViewBag.Cinemas = new SelectList(Dropdowns.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(Dropdowns.Producers, "Id", "FullName");

                return View(newMovie);
            }

            await _service.AddMovieAsync(newMovie);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var dbMovie = await _service.GetMovieByIdAsync(id);

            if (dbMovie == null)
                return View("NotFound");

            NewMovieVM editedMovieVM = new NewMovieVM()
            {
                Id = dbMovie.Id,
                Name = dbMovie.Name,
                Description = dbMovie.Description,
                StartDate = dbMovie.StartDate,
                EndDate = dbMovie.EndDate,
                ImageURL = dbMovie.ImageURL,
                CinemaId = dbMovie.CinemaId,
                ProducerId = dbMovie.ProducerId,
                Price = (double)Math.Round(dbMovie.Price),
                MovieCategory = dbMovie.MovieCategory,
                ActorIds = dbMovie.Actors_Movies.Select(am => am.ActorId).ToList()
            };

            var Dropdowns = await _service.GetAlldropdownsVM();

            ViewBag.Actors = new SelectList(Dropdowns.Actors, "Id", "FullName");
            ViewBag.Cinemas = new SelectList(Dropdowns.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(Dropdowns.Producers, "Id", "FullName");

            return View(editedMovieVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewMovieVM updatedmovie)
        {

            if (id != updatedmovie.Id)
                return View("NotFound");


            if (!ModelState.IsValid)
            {
                var Dropdowns = await _service.GetAlldropdownsVM();

                ViewBag.Actors = new SelectList(Dropdowns.Actors, "Id", "FullName");
                ViewBag.Cinemas = new SelectList(Dropdowns.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(Dropdowns.Producers, "Id", "FullName");

                return View(updatedmovie);
            }

            await _service.UpdateMovieAsync(updatedmovie);
            return RedirectToAction(nameof(Index));
        }
    }
}
