using eTicket.Data.Base;
using eTicket.Models;
using eTicket.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eTicket.Data.Services
{
    public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
    {
        private readonly AppDbContext _context;
        public MoviesService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddMovieAsync(NewMovieVM newMovievm)
        {
            var Movie = new Movie()
            {
                Name = newMovievm.Name,
                Description = newMovievm.Description,
                StartDate = newMovievm.StartDate,
                EndDate = newMovievm.EndDate,
                ImageURL = newMovievm.ImageURL,
                CinemaId = newMovievm.CinemaId,
                ProducerId = newMovievm.ProducerId,
                Price = newMovievm.Price,
                MovieCategory = newMovievm.MovieCategory
            };

            await _context.Movies.AddAsync(Movie);
            await _context.SaveChangesAsync();


            foreach (var actorId in newMovievm.ActorIds)
            {
                var MovieActors = new Actor_Movie()
                {
                    MovieId = Movie.Id,
                    ActorId = actorId
                };
                await _context.AddAsync(MovieActors);
            }

            await _context.SaveChangesAsync();

        }

        public async Task<NewMovieDropdownsVM> GetAlldropdownsVM()
        {
            var Dropdowns = new NewMovieDropdownsVM()
            {
                Actors = await _context.Actors.OrderBy(a => a.FullName).ToListAsync(),
                Cinemas = await _context.Cinemas.OrderBy(c => c.Name).ToListAsync(),
                Producers = await _context.Producers.OrderBy(p => p.FullName).ToListAsync()
            };

            return Dropdowns;
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            return await _context.Movies.Include(m => m.Cinema).Include(m => m.Producer).Include(m => m.Actors_Movies).ThenInclude(m => m.Actor).FirstOrDefaultAsync(m => m.Id == id);

        }

        public async Task UpdateMovieAsync(NewMovieVM movieVM)
        {

            var dbMovie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieVM.Id);


            if (dbMovie != null)
            {
                dbMovie.Id = movieVM.Id;
                dbMovie.Name = movieVM.Name;
                dbMovie.Description = movieVM.Description;
                dbMovie.StartDate = movieVM.StartDate;
                dbMovie.EndDate = movieVM.EndDate;
                dbMovie.ImageURL = movieVM.ImageURL;
                dbMovie.CinemaId = movieVM.CinemaId;
                dbMovie.ProducerId = movieVM.ProducerId;
                dbMovie.Price = movieVM.Price;
                dbMovie.MovieCategory = movieVM.MovieCategory;
                await _context.SaveChangesAsync();
            }

            var UpdatedmovieActors = await _context.Actors_Movies.Where(am => am.MovieId == dbMovie.Id).ToListAsync();
            _context.RemoveRange(UpdatedmovieActors);
            await _context.SaveChangesAsync();

            foreach (var actorId in movieVM.ActorIds)
            {
                var MovieActors = new Actor_Movie()
                {
                    MovieId = dbMovie.Id,
                    ActorId = actorId
                };
                await _context.Actors_Movies.AddAsync(MovieActors);
            }
            await _context.SaveChangesAsync();
        }
    }
}
