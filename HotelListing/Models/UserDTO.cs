using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models
{
    public class LoginUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class UserDTO : LoginUserDTO
    {
        public string FirstName { get; set; }

        public string Lastname { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
