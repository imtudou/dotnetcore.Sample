using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _05.Configuration.OptionDemo.Service
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IOptions<UserInfoOptions> _userInfoOptions;
        //private readonly IOptionsSnapshot<UserInfoOptions> _userinfoOptionsSnapshot;
        private readonly IOptionsMonitor<UserInfoOptions> _userinfoOptionsMonitor;

        public UserInfoService(IOptions<UserInfoOptions> userInfoOptions,
            //IOptionsSnapshot<UserInfoOptions> userinfoOptionsSnapshot,
            IOptionsMonitor<UserInfoOptions> userinfoOptionsMonitor
            )
        {
            _userInfoOptions = userInfoOptions;
            //_userinfoOptionsSnapshot = userinfoOptionsSnapshot;
            _userinfoOptionsMonitor = userinfoOptionsMonitor;
        }

        public string GetUserInfoByIOptions()
        {
            return $"GetUserInfoByIOptions:{_userInfoOptions.Value.name}";
        }

        public string GetUserInfoByIOptionsMonitor()
        {
            return $"GetUserInfoByIOptionsMonitor:{_userinfoOptionsMonitor.CurrentValue.name}";

        }

        public string GetUserInfoByIOptionsSnapshot()
        {
            //_userinfoOptionsSnapshot.Value.name
            return $"GetUserInfoByIOptionsSnapshot:";

        }
    }
}
