using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScanTitle.Business
{
    public static class WeChatInvoice
    {
        public static DateTime time { get; set; }

        /// <summary>
        /// 获取微信token
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            if (!string.IsNullOrWhiteSpace(WechatConfig.AccessTokenUrl))
            {
                if (time.AddMinutes(6)>=DateTime.Now)
                {
                    return WechatConfig.AccessTokenUrl;
                }
            }

            string getTokenUrl = string.Format(WechatConfig.AccessTokenUrl, WechatConfig.AppID, WechatConfig.AppSecret);
            try
            {
                string token;
                using (WebClient clicent=new WebClient())
                {
                    clicent.Encoding = Encoding.UTF8;
                    string data = clicent.DownloadString(getTokenUrl);
                    JObject jobj = JObject.Parse(data);
                    token = jobj["access_token"].ToString();
                }
                return token;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        /// <summary>
        /// 获取微信抬头
        /// </summary>
        /// <param name="scanText"></param>
        /// <returns></returns>
        public  static string GetScanTitle(string scanText)

        {
            string token = GetToken();
            string getScanTitleUrl = string.Format(WechatConfig.GetScanTitle, token);

            var data = JsonConvert.SerializeObject(new {
                scan_text = scanText
            });
            string result =  HttpWebRequestHelper.HttpPostOne(data, getScanTitleUrl, 90);
            return result;
        }

    }
}
