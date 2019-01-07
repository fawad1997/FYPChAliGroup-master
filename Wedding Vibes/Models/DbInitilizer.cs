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
        }
    }
}
