using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTicket.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Profile picture URL must be entered")]
        [Display(Name = "Profile Picture")]
        public string ProfilePictureURL { get; set; }


        [Required(ErrorMessage ="Full name must be entered")]
        [StringLength(25,MinimumLength = 3, ErrorMessage ="Full name length must be between 3 and 50 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

 
        [Required(ErrorMessage = "Biography must be entered")]
        [Display(Name = "Biography")]
        public string Bio  { get; set; }

        public ICollection<Actor_Movie> Actors_Movies { get; set; }
        
    }
}
