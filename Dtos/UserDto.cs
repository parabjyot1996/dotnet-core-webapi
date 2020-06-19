using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.Dtos
{
    public class UserDto
    {
        [Required]
        [EmailAddress]
        public string EmailId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}