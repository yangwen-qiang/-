using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using 智能罐装生产线监控系统.ViewModel;

namespace 智能罐装生产线监控系统
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // //相当于容器
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

        }
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<AlarmsViewModel>();
            services.AddSingleton<DashQueryViewModel>();
            services.AddSingleton<DashBoardViewModel>();
            services.AddSingleton<LogsViewModel>();
            services.AddSingleton<SettingViewModel>();
            services.AddSingleton<MainWindowVM>();
        }
    }

}
