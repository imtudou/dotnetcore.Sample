using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using User.Identity.Entity.Dtos;

namespace User.Identity.Services
{
    public interface IUserService
    {

        /// <summary>
        /// 检查手机号是否注册，当没有注册的时候就创建一个用户
        /// </summary>
        /// <param name="phone"></param>
        Task<UserInfoDto> CheckOrCreateAsync(string phone);
    }
}
