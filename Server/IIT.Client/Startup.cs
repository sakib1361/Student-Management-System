using Blazored.LocalStorage;
using Blazored.SessionStorage;
using CoreEngine.Helpers;
using CoreEngine.Model.Common;
using IIT.Client.Services;
using MatBlazor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace IIT.Client
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorizationCore(config =>
            {
                config.AddPolicy(AppConstants.Admin, HasPolicy(AppConstants.Admin));
                config.AddPolicy(AppConstants.Student, HasPolicy(AppConstants.Student));
                // config.AddPolicy(Policies.IsMyDomain, Policies.IsMyDomainPolicy());  Only works on the server end
            });
            services.AddLoadingBar();
            services.AddBlazoredSessionStorage();
            services.AddBlazoredLocalStorage();
            services.AddScoped<AppState>();
            services.AddScoped<ApiAuthenticationProvider>();
            services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<ApiAuthenticationProvider>());

            ServiceHelper.Register(services);


            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });
        }
        private static AuthorizationPolicy HasPolicy(string claim)
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IComponentsApplicationBuilder app)
        {
            app.UseLoadingBar();
            app.AddComponent<App>("app");
        }
    }
}
