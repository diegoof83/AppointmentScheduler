using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduler.Models.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} caracters long.", MinimumLength = 7)]
        public string Password { get; set; }
                
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage ="The password and confirmation do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name ="Role")]
        public string RoleName { get; set; }
    }
}
