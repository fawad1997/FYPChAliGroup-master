using System.ComponentModel.DataAnnotations;

namespace WeddingVibes.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
