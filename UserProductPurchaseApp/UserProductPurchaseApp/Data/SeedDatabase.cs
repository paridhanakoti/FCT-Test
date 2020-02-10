using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserProductPurchaseApp.Data
{
    public class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                //seed users
                ApplicationUser user1 = new ApplicationUser()
                {
                    Email = "testuser1@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "TestUser1"

                };

                ApplicationUser user2 = new ApplicationUser()
                {
                    Email = "testuser2@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "TestUser2"

                };
                userManager.CreateAsync(user1, "P@ssword11");
                userManager.CreateAsync(user2, "P@ssword22");
            }
        }
    }
}
