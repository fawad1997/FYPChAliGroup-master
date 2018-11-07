using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wedding_Vibes.Models.MenuVM
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
