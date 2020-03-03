using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Student.Infrasructure.DBModel;
using Student.Infrastructure.DBModel;
using Student.Infrastructure.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IIT.Server.WebServices
{
    public class AppStartService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public AppStartService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            // Get the DbContext instance
            var db = scope.ServiceProvider.GetRequiredService<StudentDBContext>();
            await db.Database.MigrateAsync();

            var _roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityDBUser>>();
            var adminRole = await _roleManager.FindByNameAsync(AppConstants.Admin);
            if (adminRole == null)
            {
                adminRole = new IdentityRole(AppConstants.Admin);
                //create the roles and seed them to the database
                await _roleManager.CreateAsync(adminRole);
                await _roleManager.CreateAsync(new IdentityRole(AppConstants.Student));
            }
            //Assign Admin role to the main User here we have given our newly registered 
            //login id for Admin management
            var user = await _userManager.FindByNameAsync("admin");
            if (user == null)
            {

                var dbUser = new DBUser()
                {
                    UserName = "admin",
                    Email = "iit.mobile19@gmail.com",
                    Role = AppConstants.Admin,
                    PhoneNumber = "017000000"
                };
                user = IdentityDBUser.Create(dbUser);
                await _userManager.CreateAsync(user, "pass_WORD_1234");
                await _userManager.AddToRoleAsync(user, AppConstants.Admin);


                var _feedService = scope.ServiceProvider.GetRequiredService<FeedDataService>();
#if DEBUG
                await _feedService.StartAsync(dbUser.Id);
#endif
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
