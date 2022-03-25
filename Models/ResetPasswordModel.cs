using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class ResetPasswordModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string NewPassword { get; set; }
        
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}
