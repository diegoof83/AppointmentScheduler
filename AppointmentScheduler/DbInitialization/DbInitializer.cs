using AppointmentScheduler.Models;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppointmentScheduler.DbInitialization
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext dbContext, UserManager<ApplicationIdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                //check if there are any pending migrations
                if (_dbContext.Database.GetPendingMigrations().Any())
                {
                    //push them automatically to the data base in case
                    _dbContext.Database.Migrate();
                }

            }
            catch
            {

            }

            //if (_dbContext.Roles.Any(r => r.Name == Helper.Admin)) return;

            if (!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Helper.ServiceProvider)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Helper.Client)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationIdentityUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "Complete"
                }, "Admin123*").GetAwaiter().GetResult();

                var adminUser = _dbContext.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(adminUser, Helper.Admin).GetAwaiter().GetResult();
            }
        }
    }
}
