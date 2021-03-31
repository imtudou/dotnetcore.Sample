using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using NetCore.IntefaceFramework.WebApi.CommonService.Model;

namespace NetCore.IntefaceFramework.WebApi.CommonService
{
    public static class CommonHelper
    {
        /// <summary>
        /// 获取当前程序集下Assembly文件夹路径
        /// </summary>
        public static string AssemblyFolder  
        {
            get
            {
                return Directory.GetCurrentDirectory()+ "\\Assembly\\";
            }
            set{ }
        }


        /// <summary>
        /// 获取当前程序集下JsonData文件夹
        /// </summary>
        /// <returns></returns>
        public static string JsonDataFolder {
            get
            {
                return Directory.GetCurrentDirectory() + "\\JsonFiles\\";
            }
            set{ }

        }


        public static void SaveFile(string path, string contents)
        {
            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, contents, Encoding.UTF8);
        }
        public static async Task<string> SaveFileAsync(string path, string contents)
        {
            if (File.Exists(path))
                File.Delete(path);
            await File.WriteAllTextAsync(path, contents, Encoding.UTF8);
            return path;
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static async Task<string> SaveData(string path, string contents)
        {
            return await SaveFileAsync(path, contents);
              
        }


        public static string ReadAllText(string path)
        {
            return File.ReadAllText(path, Encoding.UTF8);
        }


        /// <summary>
        /// 生成实体类Model
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static string GenerateModel(RequestModel  model)
        {
            StringBuilder sb = new StringBuilder();





            return sb.ToString();
        }

















    }
}
