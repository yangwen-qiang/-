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
        [ObservableProperty]
        public object userControl;

        private IServiceProvider serviceProvider;
        public MainWindowVM(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            NavigateToDashBoard();
        }

        private void NavigateToDashBoard()
        {
            userControl = serviceProvider.GetRequiredService<DashBoardViewModel>();
        }

        [RelayCommand]
        public void Navigate(string destination)
        {

        }
    }
}
