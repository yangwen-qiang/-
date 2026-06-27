using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 智能罐装生产线监控系统.Models
{
    public class SettingModel
    {
        //串口设置
        public string PortName { get; set; } = "COM3";
        public int BuadRate { get; set; } = 115200;
        public int DataBit { get; set; } = 8;
        public string parity { get; set; } = "None";
        public string StopBit { get; set; } = "One";
        //系统设置
        public bool AutoConnect {  get; set; }=true;
        public bool AlarmSound {  get; set; }=false;
        public bool DebugLogMode { get; set; } = false;
    }
}
