using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class RegisterModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ComparePassword { get; set; }
    }
}
