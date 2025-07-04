using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Entity.Dtos
{
    public class UserIdentity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名称f
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图像
        /// </summary>
        public string Avatar { get; set; }

        public string Company { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
    }
}
