using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScanTitle
{
    public static class WechatConfig
    {
        /// <summary>
        /// APPID：绑定支付的APPID（必须配置）
        /// </summary>
        public static string AppID { get; set; } = "APPID";
        /// <summary>
        /// APPSECRET：应用密码
        /// </summary>
        public static string AppSecret { get; set; } = "APPSECRET";
        /// <summary>
        /// 凭证Access_token
        /// </summary>
        public static string AccessTokenUrl { get; set; } = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        /// <summary>
        /// 获取用户抬头url
        /// </summary>
        public static string GetScanTitle { get; set; } = "https://api.weixin.qq.com/card/invoice/scantitle?access_token={0}";

    }
}