using System.ComponentModel.DataAnnotations;

namespace Future.Bangla.Web.Models
{
    /*
    ======================================================
        Only Add the properties from identity user
        which are neccessary to use, here I don't need
        every properties.
    ======================================================
    */
    public class RegistrationRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}
