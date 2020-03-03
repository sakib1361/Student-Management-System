using CoreEngine.Engine;
using CoreEngine.Helpers;
using CoreEngine.Model.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mobile.Core.Engines.Dependency;
using Mobile.Core.Engines.Services;
using Mobile.Core.Models.Core;
using Mobile.Core.ViewModels;
using Mobile.Core.Worker;
using MobileApp.Controls;
using MobileApp.Service;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class App : Application
    {
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        public App(IPlatformService platformService)
        {
            InitializeComponent();
            var nav = new NavigationService(platformService);
            RegisterPages(nav);
            AppService.Init(nav, nav);
            LogEngine.Initialize(nav);
            nav.Init<SplashViewModel>();
        }

        public static App Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            var host = new HostBuilder()
                            //ConfigureHostConfiguration
                            .ConfigureServices((c, x) =>
                            {
                                nativeConfigureServices(c, x);
                                ConfigureServices(c, x);
                            })
                            .ConfigureLogging(l => l.AddConsole(o => o.DisableColors = true))
                            .UseContentRoot(Environment.CurrentDirectory)
                            .Build();

            Locator.Init(host.Services);
            return Locator.GetInstance<App>();
        }

        private static void ConfigureServices(HostBuilderContext c, IServiceCollection services)
        {
            services.AddSingleton<App>();
            services.AddSingleton<IPreferenceEngine, PreferenceEngine>();
            RegisterAllTypes<BaseViewModel>(services, typeof(BaseViewModel).Assembly);
            var http = new HttpClient
            {
                BaseAddress = new Uri(AppConstants.BaseUrl)
            };

            services.AddSingleton(http);
            services.AddSingleton<SettingService>();
            ServiceHelper.Register(services);
        }

        public static void RegisterAllTypes<T>(IServiceCollection services, Assembly assembly)
        {

            var types = assembly.GetTypes()
                                .Where(myType => myType.IsClass &&
                                      !myType.IsAbstract &&
                                      myType.IsSubclassOf(typeof(T)));

            foreach (var type in types)
            {
                services.AddTransient(type);
            }
        }


        private static void RegisterPages(INavigationService _nav)
        {
            var assembly = typeof(App).Assembly;
            var types = from x in Assembly.GetAssembly(typeof(App)).GetTypes()
                        let y = x.BaseType
                        where !x.IsAbstract && !x.IsInterface &&
                        y != null && y.IsGenericType &&
                        y.GetGenericTypeDefinition() == typeof(CustomPage<>)
                        select x;

            var tabTypes = from x in Assembly.GetAssembly(typeof(App)).GetTypes()
                           let y = x.BaseType
                           where !x.IsAbstract && !x.IsInterface &&
                           y != null && y.IsGenericType &&
                           y.GetGenericTypeDefinition() == typeof(CustomTabPage<>)
                           select x;

            foreach (var type in types)
            {
                var page = type.BaseType.GetGenericArguments().FirstOrDefault();
                _nav.Configure(page, type);
            }

            foreach (var type in tabTypes)
            {
                var page = type.BaseType.GetGenericArguments().FirstOrDefault();
                _nav.Configure(page, type);
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
