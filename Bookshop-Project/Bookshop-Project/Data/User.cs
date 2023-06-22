namespace Bookshop_Project.Data
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    public class User:IdentityUser
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
