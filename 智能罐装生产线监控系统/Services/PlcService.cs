using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 智能罐装生产线监控系统.Services
{
    public class PlcService
    {
        public static string[] GetPortName()
        {
            return SerialPort.GetPortNames();
        }
    }
}
