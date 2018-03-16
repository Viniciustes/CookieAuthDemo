using System.ComponentModel.DataAnnotations;

namespace CookieAuthDemo.Models
{
    public class UserDetails
    {
        public UserDetails() { }

        public UserDetails(string returnUrl) => ReturnUrl = returnUrl;

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}
