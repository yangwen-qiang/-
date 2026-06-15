using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 智能罐装生产线监控系统.ViewModel;



namespace 智能罐装生产线监控系统
{
    public partial class MainWindowVM:ObservableObject
    {
        [ObservableProperty]//自动生成属性
        public object userControl;

        private IServiceProvider serviceProvider;
        public MainWindowVM(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            NavigateToDashBoard();
        }

        private void NavigateToDashBoard()
        {
            UserControl = serviceProvider.GetRequiredService<DashBoardViewModel>();
        }

        [RelayCommand]
        public void Navigate(string destination)
        {
            if (!string.IsNullOrEmpty(destination)&&destination!="")
            {
                switch (destination)
                {
                    case "Dashboard":
                        UserControl = serviceProvider.GetRequiredService<DashBoardViewModel>();
                        break;

                    case "Query":
                        UserControl = serviceProvider.GetRequiredService<DashQueryViewModel>();
                        break;
                    case "Logs":
                        UserControl = serviceProvider.GetRequiredService<LogsViewModel>();
                        break;
                    case "Alarm":
                        UserControl = serviceProvider.GetRequiredService<AlarmsViewModel>();
                        break;
                    case "Setting":
                        UserControl = serviceProvider.GetRequiredService<SettingViewModel>();
                        break;
                    default:
                        break;


                }
            }
            
        }
    }
}
