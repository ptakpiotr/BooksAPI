using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class ForgotPasswordModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Compare(nameof(Email))]
        public string ConfirmEmail { get; set; }
    }
}
