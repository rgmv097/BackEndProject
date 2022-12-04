using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Project.Data;

namespace Project.DAL
{
    public class DataInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _dbContext;
        private readonly AdminUser _adminUser;

        public DataInitializer(IServiceProvider serviceProvider)
        {
            _adminUser = serviceProvider.GetService<IOptions<AdminUser>>().Value;
            _userManager=serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            _roleManager=serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _dbContext=serviceProvider.GetRequiredService<AppDbContext>();
        }

        public async Task SeedData()
        {
            await _dbContext.Database.MigrateAsync();
            var roles=new List<string> { Constants.AdminRole };
            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                    continue;

                var result = await _roleManager.CreateAsync(new IdentityRole { Name = role });
            }
                var userExist = await _userManager.FindByNameAsync(_adminUser.Username);
                if (userExist != null)
                    return;

                var adminResult = await _userManager.CreateAsync(new IdentityUser
                {
                    UserName = _adminUser.Username,
                    Email = _adminUser.Email,


                }, _adminUser.Password);

                if (adminResult.Succeeded)
                {
                    var existUser = await _userManager.FindByNameAsync(_adminUser.Username);
                  await _userManager.AddToRoleAsync(existUser,Constants.AdminRole);
                }
            
        }

    }
}
