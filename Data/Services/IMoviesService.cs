using eTicket.Data.Base;
using eTicket.Models;
using eTicket.ViewModels;
using System.Threading.Tasks;

namespace eTicket.Data.Services
{
    public interface IMoviesService : IEntityBaseRepository<Movie>
    {
        Task<Movie> GetMovieByIdAsync(int id);

        Task<NewMovieDropdownsVM> GetAlldropdownsVM();
        Task AddMovieAsync(NewMovieVM newMovie);
        Task UpdateMovieAsync(NewMovieVM editedMovieVM);
    }
}
