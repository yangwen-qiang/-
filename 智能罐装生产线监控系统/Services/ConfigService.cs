using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using 智能罐装生产线监控系统.Models;

namespace 智能罐装生产线监控系统.Services
{
    public class ConfigService
    {
        private static SemaphoreSlim _lock=new SemaphoreSlim(1,1);
        private static string PathName = "device-settings.json"; 
        /// <summary>
        /// 得到配置文件路径
        /// </summary>
        /// <returns></returns>
        public static string GetSettingPath()
        {
            return Path.Combine(AppContext.BaseDirectory, PathName);
        }
        public static async Task<SettingModel> LoadDeviceSettingAsync()
        {
            string path=GetSettingPath();
            SettingModel ?settingmodel=null;
            await _lock.WaitAsync();
            try
            {
                if(File.Exists(path))
                {
                   string json=await File.ReadAllTextAsync(path);//一次性读完
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // 关键：反序列化时忽略属性名大小写
                    };
                    settingmodel =JsonSerializer.Deserialize<SettingModel>(json, options);
                    if(settingmodel != null )
                    {
                        //完成文件读取
                       return settingmodel;
                    }
                    else
                    {
                        //恢复默认值
                        settingmodel = new SettingModel();
                    }
                }
            }
            catch (Exception ex)
            {
                BackCorruptFile(path);
            }
            finally
            {
                _lock.Release();//释放锁
            }
            
            return settingmodel;

        }
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="settingmodel"></param>
        /// <returns></returns>
        public static async Task<bool> SaveDeviceSettingAsync(SettingModel settingmodel)
        {
            string path = GetSettingPath();
            string PathTemp = GetSettingPath() + "tmp";//弄一个临时文件
            if (settingmodel == null)
            {
                return false;
            }
            await _lock.WaitAsync();
            try
            {
                //原子写入
                //序列化成格式美观（带缩进和换行）的 JSON 字符串
                var json = JsonSerializer.Serialize(settingmodel, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(PathTemp, json);
                File.Move(PathTemp, path, true);//覆盖写入
                                                //写入成功
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {
                _lock.Release();//释放锁
                //如果写进去了临时文件，则说明已经覆盖到path的文件了
                if (File.Exists(PathTemp))
                {
                    File.Delete(PathTemp);
                }
            }

        }
        /// <summary>
        /// 备份已损坏的文件
        /// </summary>
        /// <param name="path"></param>
        public static void BackCorruptFile(string path)
        {
            try
            {
                string BackupPath = path + "-BackCorrup-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                File.Copy(path, BackupPath, true);

            }
            catch (Exception ex)
            {

            }
        }



    }
}
