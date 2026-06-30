using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 智能罐装生产线监控系统.Models;
using 智能罐装生产线监控系统.Services;
using 智能罐装生产线监控系统.Services.Logs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace 智能罐装生产线监控系统.ViewModel
{
    public partial class LogsViewModel:ObservableObject
    {
        [ObservableProperty]
        private DateTime startDate = DateTime.Today.AddDays(-1);
        [ObservableProperty]
        private DateTime endDate = DateTime.Today.AddDays(1).AddSeconds(-1);
        [ObservableProperty]
        private string selelctLevel= "All";
        [ObservableProperty]
        private string searchtext="";
        [ObservableProperty]
        private bool isbuys=false;
        [ObservableProperty]
        private int indexpage=1;
        [ObservableProperty]
        private int pagesize=50;
        [ObservableProperty]
        private int totalCount;
        public ObservableCollection<string> Levels {  get;private set; }= new ObservableCollection<string>()
        {
           "Information",
           "Debug",
           "Warning",
           "All",
           "Error"
        };
        //每次查询时都要给Logs赋予新值
        [ObservableProperty]
        private ObservableCollection<SystemLog> logs  = new ObservableCollection<SystemLog>();

        public LogsViewModel()
        {
            _LoadUpdate();
        }
        [RelayCommand]
        public async Task QueryAsync()
        {
            _LoadUpdate();
        }
        [RelayCommand]
        public async Task PreviousPage()
        {
           
            if(Indexpage ==1)
            {
                return;
            }
            else
            {
                Indexpage--;
                await _LoadUpdate();
            }
        }
        [RelayCommand]
        public async Task NextPage()
        {
            if (Indexpage*Pagesize>=TotalCount|| TotalCount<=Pagesize)
            {
                return;
            }
            else
            {
                Indexpage++;
                await _LoadUpdate();
            }
        }
        [RelayCommand]
        public async Task ExportAsync()
        {
            if(Logs==null||Logs.Count==0)
                return;
            var save=new SaveFileDialog()
            {
                //以下两个必填
                Filter= "CSV.文件|*.csv",//文件类型
                FileName=$"ProductionLogs_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.csv"//默认文件名
            };
            if(save.ShowDialog()== true)
            {
                try
                {
                    await LogService.LogExportAsync(Logs.ToList(),save.FileName);
                    MessageBox.Show("导出文件成功","提示");
                }
                catch(Exception ex) 
                {
                    LogService.Error($"导出日志Csv文件失败：{ex.Message}");
                }
            }


        }
        private async Task _LoadUpdate()
        {
            //防重入锁
            if (Isbuys)
                return;
            Isbuys = true;
            try
            {
                //读取该条件筛选下的日志数据
                var querylog= filter();
                //对读出来的数据进行时间排序和分页查询
                //这两步骤才是对数据库的读写操作需要异步
                TotalCount = (int) await querylog.CountAsync();
                var data=await querylog.OrderBy(x=>x.Timestamp).Page(Indexpage,Pagesize).ToListAsync();
                
                Logs = new ObservableCollection<SystemLog>(data);
                

            }
            catch (Exception ex)
            {
                LogService.Error($"读取日志数据库失败：{ex.Message}");
            }
            finally
            {
                Isbuys = false;
            }
        }
        //查询指定条件下的日志数据
        private FreeSql.ISelect<SystemLog> filter()
        {
            var QueryAllLogs = DbProvider.FreeSqlInstance.Select<SystemLog>();
            string start = StartDate.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            string end = EndDate.ToString("yyyy-MM-ddTHH:mm:ss.fff");
           
            //先对时间进行查询
            var QueryLogs = QueryAllLogs.Where("Timestamp >= @start AND Timestamp <= @end", new { start, end });
            //在对搜索框和等级框进行查询
            if (SelelctLevel != "All" && SelelctLevel != null)
            {
                QueryLogs= QueryLogs.Where(x=>x.Level== SelelctLevel);
            }
            if (Searchtext != "" && Searchtext != null)
            {
                QueryLogs = QueryLogs.Where(x => x.RenderedMessage.Contains(Searchtext));
            }
            return QueryLogs;
        }
    }
}
