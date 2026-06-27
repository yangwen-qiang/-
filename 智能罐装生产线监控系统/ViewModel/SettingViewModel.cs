using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 智能罐装生产线监控系统.Models;
using 智能罐装生产线监控系统.Services;

namespace 智能罐装生产线监控系统.ViewModel
{
    public partial class SettingViewModel:ObservableObject
    {
        //只能调用不能修改
        public ObservableCollection<string> PortNames { get; } = new ObservableCollection<string>();
        public ObservableCollection<int> BaudRates { get; } = new ObservableCollection<int>() { 9600, 19200, 38400, 57600, 115200 };
        public ObservableCollection<int> DataBitOptions { get; } = new ObservableCollection<int>() {7,8};

        public ObservableCollection<string> ParityOptions { get; } = new ObservableCollection<string>() { "None", "Odd", "Even" };
        public ObservableCollection<string> StopOptions { get;} = new ObservableCollection<string>() { "None", "One", "Two" };

        //选择项各个commbox
        [ObservableProperty]
        private string portName = "COM3";
        [ObservableProperty]
        private int selectedtBaud = 115200;
        [ObservableProperty]
        private int selectedDataBits = 8;
        [ObservableProperty]
        private string selectedParity = "None";
        [ObservableProperty]
        private string selectedStopBits = "One";

        [ObservableProperty]
        private bool autoConnect = true;
        [ObservableProperty]
        private bool alarmSound = true;
        [ObservableProperty]
        private bool debugLogMode = false;
        public SettingViewModel()
        {
            //只做初始化或调用同步方法
            
            
        }

        public  async Task InitializeAsync()
        {
            //先读取json的通讯配置文件,防止文件里面的串口没有计算机读到的
            await ReadJsonSetting();
            //后获取计算机串口号
            RefreshPortNameList();
        }

        //读取json文件中的配置设置
        private async Task ReadJsonSetting()
        {
            SettingModel setting = await ConfigService.LoadDeviceSettingAsync();
            PortName=setting.PortName;
            SelectedtBaud = setting.BuadRate;
            SelectedDataBits = setting.DataBit;
            SelectedParity = setting.parity;
            SelectedStopBits=setting.StopBit;
            AutoConnect= setting.AutoConnect;
            AlarmSound= setting.AlarmSound;
            DebugLogMode = setting.DebugLogMode;

        }

        //获取计算机串口号
        private void RefreshPortNameList()
        {
            try
            {
                var portNs = PlcService.GetPortName();
                foreach (var portName in portNs)
                {
                    PortNames.Add(portName);
                }
                if(string.IsNullOrEmpty(PortName)|| !PortNames.Contains(PortName))
                {
                    PortName = PortNames[0];
                }
            }
            catch(Exception ex)
            {

            }
        }
        //异步写于文件
        [RelayCommand]
        public async Task SaveAsync()
        {
            SettingModel model = new SettingModel();
            model.PortName = PortName;
            model.parity = SelectedParity;
            model.StopBit = SelectedStopBits;
            model.AutoConnect = AutoConnect;
            model.AlarmSound = AlarmSound;
            model.DebugLogMode = DebugLogMode;
            model.DataBit= SelectedDataBits;
            model.BuadRate = SelectedtBaud;
            await ConfigService.SaveDeviceSettingAsync(model);
        }
    }
}
