using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using CheckinLibrary.Database;
using CheckinLibrary.Database.EF;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using CheckInSystem.Platform;

namespace CheckInSystem.Desktop
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            MapsterConfig.Register();
            TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = false;

            var services = new ServiceCollection();

            string connectionString = "YourConnectionStringHere";
            services.AddDbContext<CheckInDbContext>(options =>
                options.UseSqlServer(connectionString)
                       .UseChangeTrackingProxies() 
            );

            services.AddSingleton<IDatabaseHelper, DatabaseSqlExpressEF>();

            AppServices.Provider = services.BuildServiceProvider();

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();
    }
}
