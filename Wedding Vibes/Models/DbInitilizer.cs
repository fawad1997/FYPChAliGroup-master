using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wedding_Vibes.Data;

namespace Wedding_Vibes.Models
{
    public class DbInitilizer
    {
        public static async Task InitilizeAsync(ApplicationDbContext db,IServiceProvider serviceProvider)
        {
            var RolesManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Member", "Admin" };
            IdentityResult roleResult;
            foreach(var roleName in roleNames)
            {
                var roleExists = await RolesManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    roleResult = await RolesManager.CreateAsync(new IdentityRole(roleName));
                }
                ApplicationUser user = await UserManager.FindByEmailAsync("fawad_12@outlook.com");
                if (user != null)
                    await UserManager.AddToRoleAsync(user, "Admin");
            }

            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
            if (!context.Menu.Any())
            {
                context.Menu.Add(new Menu.Menu { MenuName = "Desert", MenuPrice = 1700.0 });
                context.Menu.Add(new Menu.Menu { MenuName = "Menu 2", MenuPrice = 2000.0 });
                context.SaveChanges();
            }
            if (!context.MenuItem.Any())
            {
                context.MenuItem.Add(new Menu.MenuItem { ItemName = "Item 1", Category = "Category 1", MenuId = 1 });
                context.MenuItem.Add(new Menu.MenuItem { ItemName = "Item 2", Category = "Category 1", MenuId = 1 });
                context.MenuItem.Add(new Menu.MenuItem { ItemName = "Item 3", Category = "Category 1", MenuId = 2 });
                context.MenuItem.Add(new Menu.MenuItem { ItemName = "Item 4", Category = "Category 4", MenuId = 2 });
                context.SaveChanges();
            }
        }
    }
}
