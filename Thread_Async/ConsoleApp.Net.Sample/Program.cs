using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Net.Sample
{
    class Program
    {
        static void Main(string[] args)
        {

            const string Users = "HPT08";
            string content = @"{""Type"":""Esc"",""Message"":""您的帐号在另一台设备登陆"",""ChannelCode"":"""+ Users + "\"}";


            var cc = HealthGroupBusinessLogEnum.医生取消方案;
            string title = string.Empty;


            var cc1 = Enum.Parse(typeof(HealthGroupBusinessLogEnum), "患者解绑");

            HealthGroupBusinessLogEnum logEnum;
            var cc2 = Enum.TryParse("1101", out logEnum);

            Enum.GetName(typeof(HealthGroupBusinessLogEnum), title);
            Enum.GetValues(typeof(HealthGroupBusinessLogEnum));
            var c1 = Enum.GetName(typeof(HealthGroupBusinessLogEnum), "患者解绑");



            var ids = new int[5] { 1, 2, 3, 4, 5 };

            for (int i = 0; i < ids.Length; i++)
            {
                DoSomethiing(ids[i]);
            }
        }


        public static void DoSomethiing(int param)
        {
                     
        }
    }


     
    public enum HealthGroupBusinessLogEnum
    {
        /// <summary>
        /// 患者解绑
        /// </summary>
        患者解绑 = 1101,

        /// <summary>
        /// 退出方案
        /// </summary>
        退出方案,

        /// <summary>
        /// 医生取消方案
        /// </summary>
        医生取消方案
    }

}
