using Core.Constants;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Core.Helpers;
using System.Net;
using Core.Interfaces;
using Core.Models.Cart;

namespace Infrastructure
{
    public static class DbContextExtensions
    {
        public static async Task SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<OnlineStoreDbContext>();
                context.Database.Migrate();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var cartsService = scope.ServiceProvider.GetRequiredService<ICartsService>();

                await SeedRoles(roleManager);
                await SeedUsers(userManager, cartsService);
            }
        }

        private static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            foreach (var role in Roles.All)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    var result = roleManager.CreateAsync(new Role
                    {
                        Name = role
                    }).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception($"Error seeding role '{role}': {string.Join(", ", result.Errors)}");
                    }
                }
            }
        }

        private static async Task SeedUsers(UserManager<User> userManager, ICartsService cartsService)
        {
            if (!userManager.Users.Any())
            {
                var Users = new[]
                {
                    new { Email = "admin@gmail.com", UserName = "admin", Password = "123456", Role = Roles.Admin, PhoneNumber = "5551234123",FirstName = "AdminFirstName",LastName = "AdminLastName", Sex = "Man", Position = "Seller" },
                    new { Email = "john.doe@gmail.com", UserName = "john.doe", Password = "123456", Role = Roles.User, PhoneNumber = "5551234123",FirstName = "John", LastName = "Doe", Sex = "Man", Position = "Seller" },
                    new { Email = "jane.smith@yahoo.com", UserName = "jane.smith", Password = "123456", Role = Roles.User, PhoneNumber = "5555678123",FirstName = "John", LastName = "Smith", Sex = "Man", Position = "Seller" },
                    new { Email = "sam.jones@hotmail.com", UserName = "sam.jones", Password = "123456", Role = Roles.User, PhoneNumber = "5559876123",FirstName = "Sam", LastName = "Jones", Sex = "Man", Position = "Seller" },
                    new { Email = "susan.white@gmail.com", UserName = "susan.white", Password = "123456", Role = Roles.User, PhoneNumber = "5554321123",FirstName = "Susan", LastName = "White", Sex = "Man", Position = "Seller" },
                    new { Email = "alex.brown@gmail.com", UserName = "alex.brown", Password = "123456", Role = Roles.User, PhoneNumber = "5558765123",FirstName = "Alex", LastName = "Brown", Sex = "Man", Position = "Seller" },
                    new { Email = "olivia.martin@yahoo.com", UserName = "olivia.martin", Password = "123456", Role = Roles.User, PhoneNumber = "5555432123",FirstName = "Olivia", LastName = "Martin", Sex = "Man", Position = "Seller" },
                    new { Email = "michael.clark@hotmail.com", UserName = "michael.clark", Password = "123456", Role = Roles.User, PhoneNumber = "5552109123",FirstName = "Michael", LastName = "Clark", Sex = "Man", Position = "Seller" },
                    new { Email = "emma.wilson@gmail.com", UserName = "emma.wilson", Password = "123456", Role = Roles.User, PhoneNumber = "5551111222",FirstName = "Emma", LastName = "Wilson", Sex = "Man", Position = "Seller" },
                    new { Email = "william.jenkins@yahoo.com", UserName = "william.jenkins", Password = "123456", Role = Roles.User, PhoneNumber = "5552222333",FirstName = "William",  LastName = "Jenkins", Sex = "Man", Position = "Seller" },
                    new { Email = "grace.morris@hotmail.com", UserName = "grace.morris", Password = "123456", Role = Roles.User, PhoneNumber = "5553333444",FirstName = "Grace", LastName = "Morris", Sex = "Man", Position = "Seller" },
                    new { Email = "lucas.baker@gmail.com", UserName = "lucas.baker", Password = "123456", Role = Roles.User, PhoneNumber = "5554444555",FirstName = "Lucas",  LastName = "Baker", Sex = "Man", Position = "Seller" },
                    new { Email = "hannah.smith@yahoo.com", UserName = "hannah.smith", Password = "123456", Role = Roles.User, PhoneNumber = "5555555666",FirstName = "Hannah",  LastName = "Smith", Sex = "Man", Position = "Seller" },
                    new { Email = "david.jones@hotmail.com", UserName = "david.jones", Password = "123456", Role = Roles.User, PhoneNumber = "5556666777",FirstName = "David",  LastName = "Jones", Sex = "Man", Position = "Seller" },
                    new { Email = "emily.white@gmail.com", UserName = "emily.white", Password = "123456", Role = Roles.User, PhoneNumber = "5557777888",FirstName = "Emily",  LastName = "White", Sex = "Man", Position = "Seller" },
                    new { Email = "noah.brown@yahoo.com", UserName = "noah.brown", Password = "123456", Role = Roles.User, PhoneNumber = "5558888999",FirstName = "Noah", LastName = "Brown", Sex = "Man", Position = "Seller" }
                };

                foreach (var userData in Users)
                {
                    User user = new()
                    {
                        Email = userData.Email,
                        UserName = userData.UserName,
                        PhoneNumber = userData.PhoneNumber,
                        LastName= userData.LastName,
                        FirstName= userData.FirstName,
                        Sex = userData.Sex,
                        Position = userData.Position,
                        Image = "default_user.webp",
                    };

                    var result = await userManager.CreateAsync(user, userData.Password);

                    if (result.Succeeded)
                    {
                        result = await userManager.AddToRoleAsync(user, userData.Role);

                        var createCartDto = new CreateCartDto { UserId = user.Id };
                        await cartsService.Create(createCartDto);
                    }

                    if (!result.Succeeded)
                    {
                        string message = string.Join(", ", result.Errors.Select(x => x.Description));
                        throw new HttpException(message, HttpStatusCode.BadRequest);
                    }
                }
            }
        }
    }
}