using eTicket.Data.Base;
using eTicket.Models;

namespace eTicket.Data.Services
{
    public class CinemasServices : EntityBaseRepository<Cinema>, ICinemasService 
    {
        public CinemasServices(AppDbContext context) : base(context)
        {

        }
    }
}
