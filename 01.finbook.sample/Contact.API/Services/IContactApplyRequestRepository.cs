using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Entity.Models;

using Microsoft.AspNetCore.Identity;

namespace Contact.API.Services
{
    /// <summary>
    /// 好友申请请求记录Repository
    /// </summary>
    public interface IContactApplyRequestRepository
    {
        /// <summary>
        /// 添加好友申请请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> AddApplyRequestAsync(ContactApplyRequest request);
            
        /// <summary>
        /// 通过好友申请
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<bool> ApprovalRqeuestAsync(int userid, int ApplierId);

        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<List<ContactApplyRequest>> GetRequestListAsync(int userid);


    }
}
