using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using 智能罐装生产线监控系统.Services;
using 智能罐装生产线监控系统.Services.Logs;
using 智能罐装生产线监控系统.ViewModel;

namespace 智能罐装生产线监控系统
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private const string LogTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] ({ThreadId}) {Message:lj}{NewLine}{Exception}";
        private const string LogPath = "Logs\\log-.txt";
        private const string DbFilePath = "SmartFillmoniter66.db";
        //Freesql连接字符串
        private const string DbConnectionString = "Data Source=SmartFillmoniter66.db";

        //日志刷新再RichTextBox控件上
        public static RichTextBox LogsView = new RichTextBox()
        {
            IsReadOnly = true,
            Background = new SolidColorBrush(Colors.Black),
            Foreground = Brushes.White,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            FontFamily = new FontFamily("Consolas")
        };
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //初始化配置日志
            ConfigLogging();
            //初始化配置Freesql-sqlite
            InitialCoreService();
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
        //配置号控制台应用程序
        private void InitialCoreService()
        {
            LogService.Info("initialize database......");
            DbProvider.Initialize(DbConnectionString);
        }
        //配置日志Log
        private void ConfigLogging()
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            //为每条日志事件添加一个属性 ThreadId，值为当前托管线程的 ID。
            .Enrich.WithThreadId()
            .WriteTo.RichTextBox(LogsView,outputTemplate: LogTemplate)
            .WriteTo.Console(outputTemplate:LogTemplate)
            .WriteTo.SQLite(DbFilePath, tableName: "SystemLog", storeTimestampInUtc: false)
            .WriteTo.File(LogPath, rollingInterval: RollingInterval.Day, outputTemplate: LogTemplate, shared: true)
            .CreateLogger();
        }
    }

}
