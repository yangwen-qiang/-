using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 智能罐装生产线监控系统.Services
{
    public class DbProvider
    {
        private static readonly object _lock = new object();
        public static IFreeSql FreeSqlInstance { get; private set; }

        /// <summary>
        /// 初始化 FreeSql 单例（应用启动时调用一次）
        /// </summary>
        public static void Initialize(string connectionString, DataType dataType = DataType.Sqlite)
        {
            //FreeSql 已经初始化，不可重复调用
            if (FreeSqlInstance != null)
            {
                return;
            }

            lock (_lock)
            {
                if (FreeSqlInstance != null) return; // 双重检查

                FreeSqlInstance = new FreeSqlBuilder()
                    .UseConnectionString(dataType, connectionString)
                    .UseAutoSyncStructure(true)      // 自动同步实体结构到数据库（开发便利，生产可关闭）
                    .UseMonitorCommand(
                        cmd =>
                        {
                            //sql执行前
                        },
                        (cmd, tracelog) =>
                        {
                            Console.Write($"[SQL]{cmd.CommandText}\r\n ->{tracelog}");//在控制台中输出Sql语句
                        }
                    )
                    .Build();
            }
        }
    }
}
