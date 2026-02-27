using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Stone.Entities.Generic;

namespace Stone.Persistence
{
    public static class UserDataSeeder
    {
        public static async Task Seed(IServiceProvider service)
        {
            //User repository
            var userManager = service.GetRequiredService<UserManager<User>>();
            //Role repository
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            //Creating roles
            var adminRole = new IdentityRole(Constants.RoleAdmin);
            var customerRole = new IdentityRole(Constants.RoleCustomer);

            if (!await roleManager.RoleExistsAsync(Constants.RoleAdmin))
                await roleManager.CreateAsync(adminRole);

            if (!await roleManager.RoleExistsAsync(Constants.RoleCustomer))
                await roleManager.CreateAsync(customerRole);

            //Admin user
            var adminUser = new User()
            {
                FirstName = "System",
                LastName = "Administrator",
                UserName = "admin@stone.com",
                Email = "admin@stone.com",
                PhoneNumber = "56991214034",
                EmailConfirmed = true
            };
            if (await userManager.FindByEmailAsync("admin@stone.com") is null)
            {
                var result = await userManager.CreateAsync(adminUser, "Admin91214o34.");//"Admin1234*"
                if (result.Succeeded)
                {
                    // Obtenemos el registro del usuario
                    adminUser = await userManager.FindByEmailAsync(adminUser.Email);
                    // Aqui agregamos el Rol de Administrador para el usuario Admin
                    if (adminUser is not null)
                        await userManager.AddToRoleAsync(adminUser, Constants.RoleAdmin);
                }
            }
        }
    }
}
