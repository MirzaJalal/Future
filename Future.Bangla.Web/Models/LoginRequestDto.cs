using System.ComponentModel.DataAnnotations;

namespace Future.Bangla.Web.Models
{
    /*
    ======================================================
        Only Add the properties from identity user
        which are neccessary to use, here I only added
        the necessary properties to login
    ======================================================
    */
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
