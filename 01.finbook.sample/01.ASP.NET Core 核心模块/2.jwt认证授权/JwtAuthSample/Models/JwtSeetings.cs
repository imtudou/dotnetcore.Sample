using System;

namespace JwtAuthSample.Models
{
    public class JwtSeetings
    {
        /// <summary>
        /// 证书颁发者
        /// </summary>
        /// <value></value>
        public string Issuer{get;set;}
        
        /// <summary>
        /// 允许使用的角色
        /// </summary>
        /// <value></value>
        public string Audience{get;set;}


        /// <summary>
        /// 加密字符串(appseetings.json 文件中SecretKey 字符必须大于16个字符)
        /// </summary>
        /// <value></value>
        public string SecretKey{get;set;}
    }
}