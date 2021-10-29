﻿using Contact.API.Entity.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 获取用户的基本信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
       Task<UserInfoDto> GetUserInfoAsync(int userid);
    }
}
