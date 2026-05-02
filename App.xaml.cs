using RestaurantOrderingApp.Layers.DataAccessLayer;
using RestaurantOrderingApp.Utils;
using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.ViewModels;
using RestaurantOrderingApp.Views;
using RestaurantOrderingApp.Dialog_Service;

namespace RestaurantOrderingApp
{
    public partial class App : Application
    {
        private IHost _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
            {
                services.AddSingleton<CurrentUserSession>();
                services.AddSingleton<UserDAL>();
                services.AddSingleton<UserBLL>();
                services.AddSingleton<IDialogService, DialogService>();

                services.AddTransient<LoginWindow>();
                services.AddTransient<LoginVM>();

                services.AddTransient<RestaurantWindow>();
            }
            ).Build();

            var loginWindow = _host.Services.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }
    }

}
