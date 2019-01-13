using System.ComponentModel.DataAnnotations;

namespace WeddingVibes.Models.Menu
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        [StringLength(maximumLength:100),Display(Name ="Item Name"),Required]
        public string ItemName { get; set; }
        [StringLength(maximumLength: 100), Display(Name = "Category"), Required]
        public string Category { get; set; }
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }
    }
}
