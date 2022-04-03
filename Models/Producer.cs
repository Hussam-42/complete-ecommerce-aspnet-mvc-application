using eTicket.Data.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eTicket.Models
{
    public class Producer : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Profile picture is required")]
        [Display(Name = "Profile Picture")]
        public string ProfilePictureURL { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Full name length must be between 3 and 25 characters")]
        public string FullName { get; set; }


        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required")]

        public string Bio { get; set; }

        public ICollection<Movie> Movies { get; set; }


    }
}
