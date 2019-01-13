using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WeddingVibes.Models.MenuVM
{
    public class CustomizedMenuVM
    {
        public IList<string> SelectedItems { get; set; }
        public IList<SelectListItem> AvailableItems { get; set; }

        public CustomizedMenuVM()
        {
            SelectedItems = new List<string>();
            AvailableItems = new List<SelectListItem>();
        }
    }
}
