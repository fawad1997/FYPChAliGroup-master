using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeddingVibes.Data;
using WeddingVibes.Models.Menu;
using WeddingVibes.Models.MenuVM;

namespace WeddingVibes.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MenusController : Controller
    {
        private readonly ApplicationDbContext _context;
    

        public MenusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Menus
        public async Task<IActionResult> Index()
        {
            return View(await _context.Menu.Include(m=>m.MenuItems).ToListAsync());
        }

        // GET: Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menu.Include(m=>m.MenuItems)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menus/Create
        public async Task<IActionResult> Create()
        {

            return View();
           
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] MenuVM menu)
        {
            if (ModelState.IsValid)
            {
                   

                    var menuEntity = new Menu
                    {
                        MenuName = menu.Name,
                        MenuPrice = menu.Price,
                    };
                    _context.Add(menuEntity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

               
            }
            return View(menu);
        }

        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var items = await _context.MenuItem.ToArrayAsync();
            ViewData["MenuList"] = new SelectList(items, "Id", "ItemName");
            var menu = await _context.Menu.SingleOrDefaultAsync(m => m.Id == id);
            
            if (menu == null)
            {
                return NotFound();
            }
            var menuVM = new MenuVM
            {
                Id = menu.Id,
                Name = menu.MenuName,
                Price = menu.MenuPrice,
                UserID = menu.UserId

            };
            return View(menuVM);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  MenuVM menu)
        {
            if (id != menu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var menuEntity =await _context.Menu.FirstOrDefaultAsync(c => c.Id == id);
                    if (menuEntity !=null)
                    {


                        menuEntity.MenuName = menu.Name;
                        menuEntity.MenuPrice = menu.Price;

                         
                        _context.Update(menuEntity);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        // GET: Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menu.Include(m => m.MenuItems)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menu.SingleOrDefaultAsync(m => m.Id == id);
            _context.Menu.Remove(menu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.Id == id);
        }
    }
}
