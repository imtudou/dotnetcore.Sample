using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _05.Configuration.OptionDemo.Service
{
    public interface IUserInfoService
    {
        string GetUserInfoByIOptions();
        string GetUserInfoByIOptionsSnapshot();
        string GetUserInfoByIOptionsMonitor();
    }
}
