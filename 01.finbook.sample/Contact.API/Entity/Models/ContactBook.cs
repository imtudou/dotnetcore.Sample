using MongoDB.Bson.Serialization.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Entity.Models
{
    /// <summary>
    /// 通讯录
    /// </summary>
    ///BsonIgnoreExtraElements属性： https://blog.csdn.net/weixin_38211198/article/details/100716198
    [BsonIgnoreExtraElements]
    public class ContactBook
    {
        public ContactBook()
        {
            Contacts = new List<AppContact>();
        }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 联系人列表
        /// </summary>
        public List<AppContact> Contacts { get; set; }

    }
}
