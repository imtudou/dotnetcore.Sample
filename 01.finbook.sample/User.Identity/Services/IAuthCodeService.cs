using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Services
{
    public interface IAuthCodeService
    {

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="authcode"></param>
        /// <returns></returns>
        bool Validate(string phone, string authcode);
    }
}
