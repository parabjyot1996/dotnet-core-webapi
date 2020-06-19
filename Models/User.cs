using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.Models
{
    public class User
    {
        [Required]
        [EmailAddress]
        public string EmailId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}