﻿namespace Bangla.Services.AuthenticationAPI.Models.Dto
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
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}