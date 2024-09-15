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
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
