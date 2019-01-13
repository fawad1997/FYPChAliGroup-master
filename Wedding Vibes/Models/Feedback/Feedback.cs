using System.ComponentModel.DataAnnotations;

namespace WeddingVibes.Models.Feedback
{

    public class Feedback
    {
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
