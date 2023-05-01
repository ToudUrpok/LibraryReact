using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Library.Services;
using Library.Entities;
using Microsoft.AspNetCore.Identity;
using static Library.Services.Helpers.RoleHelpers;
using Library.Services.Helpers;
using Microsoft.EntityFrameworkCore;
namespace Library
{
    public class Program
    {
		public static void Main(string[] args)
		{
			// CreateWebHostBuilder(args).Build().Run();

			var hostBuilder = CreateWebHostBuilder(args);
			var host = hostBuilder.Build();

			InitializeDatabase(host);

			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				//.ConfigureLogging(logging =>
				//{
				//	logging.ClearProviders();
				//	logging.AddConsole(); 
				//})
				.UseStartup<Startup>();

		private static void InitializeDatabase(IWebHost host)
		{
			using (var serviceScope = host.Services.CreateScope())
			{
				var services = serviceScope.ServiceProvider;

				if (!services.GetService<ApplicationDbContext>().AllMigrationsApplied())
				{
					services.GetService<ApplicationDbContext>().Database.Migrate();
				}

				// Seed database
				serviceScope.ServiceProvider.GetService<ApplicationDbContext>().EnsureSeeded();

				IUserManagementService umService = services.GetRequiredService<IUserManagementService>();
				var usersCount = umService.GetAllUsersCountAsync("").Result;
				if (usersCount == 0)
				{
					RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

					foreach (RolePair role in RoleHelpers.Roles)
					{
						if (!roleManager.RoleExistsAsync(role.Name).Result)
						{
							var idRole = new IdentityRole(role.Name);
							roleManager.CreateAsync(idRole).Wait();
						}
					}

					// Create admin user
					ApplicationUser adminUser = new ApplicationUser
					{
						UserName = "admin@domain.com",
						Email = "admin@domain.com",
						FirstName = "Admin",
						MidName = "Admin",
						LastName = "Admin",
						DateOfBirth = new DateTime(1997, 3, 14),
						PhoneNumber = "+375299112600",
						IdentificationNumber = "5140397B013PB7",
						EmailConfirmed = true,
						Approved = true
					};

					umService.AddUserAsync(adminUser, "!Hga45dtrYu", "administrator").Wait();

					//UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
					//userManager.AddToRoleAsync(adminUser, "user").Wait();
                }
			}
		}
	}
}
