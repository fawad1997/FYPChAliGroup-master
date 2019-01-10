using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wedding_Vibes.Models.Menu;

namespace Wedding_Vibes.Models.MenuVM
{
    public class MenuVM
    {
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; }
        [Display(Name = "Menu Items")]
        public List<MenuItem> Items { get; set; }
        public List<string> SelectedItems { get; set; }
        public double Price { get; set; }
        public string UserID { get; set; }
        public bool HasCustomizedMenu { get; set; }
    }
}
