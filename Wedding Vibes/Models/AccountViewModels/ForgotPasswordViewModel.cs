using System.ComponentModel.DataAnnotations;

namespace WeddingVibes.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
