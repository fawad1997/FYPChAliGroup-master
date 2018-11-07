using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Wedding_Vibes.Data;
using Wedding_Vibes.Models;
using Wedding_Vibes.Models.Menu;
using Wedding_Vibes.Models.MenuVM;
using Wedding_Vibes.Services;

namespace Wedding_Vibes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Menu()
        {
            var user = await _userManager.GetCurrentUser(HttpContext);
            var menus = (from men in _context.Menu
                            select new MenuVM
                            {
                                Id = men.Id,
                                Name = men.MenuName,
                                Price = men.MenuPrice,
                                UserID = men.UserId,
                                Items = _context.MenuItem.Where(x=>x.MenuId==men.Id).OrderBy(x=>x.Category).ToList()
                            }).ToList();
            if (user != null)
            {
                foreach(var menu in menus)
                {
                    if (menu.UserID == user.Id)
                        menu.HasCustomizedMenu = true;
                    else
                        menu.HasCustomizedMenu = false;
                }
            }
            return View(menus);
        }
        [Authorize]
        public IActionResult CustomizedMenu()
        {
            CustomizedMenuVM customizedMenu = new CustomizedMenuVM();
            customizedMenu.AvailableItems = new List<SelectListItem>();
            var items = _context.MenuItem.Select(aa=>aa.ItemName).Where(x=>!x.ToLower().Contains("any")).Distinct().ToList();
            foreach(var i in items)
            {
                customizedMenu.AvailableItems.Add(new SelectListItem { Text = i, Value = i });
            }
            return View(customizedMenu);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CustomizedMenuPost(CustomizedMenuVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetCurrentUser(HttpContext);
                Menu m = new Menu
                {
                    MenuName = "Customized Menu",
                    MenuPrice = 0.00,
                    UserId = user.Id
                };
                _context.Menu.Add(m);
                await _context.AddRangeAsync();
                foreach(var itemname in model.SelectedItems)
                {
                    string cat = _context.MenuItem.Where(y => y.ItemName == itemname).Select(x => x.Category).FirstOrDefault();
                    var item = new MenuItem
                    {
                        MenuId = m.Id,
                        Category = cat,
                        ItemName = itemname
                    };
                    _context.MenuItem.Add(item);
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Menu");
        }
        [Authorize]
        public async Task<IActionResult> DeleteMenu()
        {
            var user = await _userManager.GetCurrentUser(HttpContext);
            Menu m = _context.Menu.Where(x => x.UserId == user.Id).FirstOrDefault();
            if (m != null)
            {
                var items = _context.MenuItem.Where(y => y.MenuId == m.Id).ToList();
                foreach(var item in items)
                {
                    _context.MenuItem.Remove(item);
                }
                _context.Menu.Remove(m);
                _context.SaveChanges();
            }
            return RedirectToAction("Menu");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
