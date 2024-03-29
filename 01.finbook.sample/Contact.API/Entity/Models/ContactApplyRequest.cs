﻿using MongoDB.Bson.Serialization.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Entity.Models
{
    /// <summary>
    /// 好友申请请求记录
    /// </summary>
    /// BsonIgnoreExtraElements属性： https://blog.csdn.net/weixin_38211198/article/details/100716198
    [BsonIgnoreExtraElements]
    public class ContactApplyRequest    
    {
        /// <summary>
        /// 用户id
        /// </summary>
      
        public int UserId { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Nmae { get; set; }

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
        /// 申请人
        /// </summary>
        public int ApplierId { get; set; }

        /// <summary>
        /// 是否同意 1:同意  0 不同意
        /// </summary>
        public int Approvaled{ get; set; }


        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime HandledTime { get; set; }   

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ApplyTime { get; set; }
    }
}
