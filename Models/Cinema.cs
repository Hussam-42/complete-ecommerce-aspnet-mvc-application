using eTicket.Data.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eTicket.Models
{
    public class Cinema : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Cinema logo is required")]
        [Display(Name="Cinema Logo")]
        public string Logo { get; set; }

        [Required(ErrorMessage = "Cinema Name is required")]

        [Display(Name = "Cinema Name")]

        public string Name { get; set; }

        
        [Required(ErrorMessage = "Cinema Description is required")]
        public string Description { get; set; }
        
        public ICollection<Movie> Movies { get; set; }
          




    }
}
