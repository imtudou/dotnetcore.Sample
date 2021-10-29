using MongoDB.Bson.Serialization.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Contact.API.Entity.Models
{
    /// <summary>
    /// 联系人列表
    /// </summary>
    /// BsonIgnoreExtraElements属性： https://blog.csdn.net/weixin_38211198/article/details/100716198
    [BsonIgnoreExtraElements]
    public class AppContact
    {
        public AppContact()
        {
            Tags = new List<string>();
        }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 用户标签
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
