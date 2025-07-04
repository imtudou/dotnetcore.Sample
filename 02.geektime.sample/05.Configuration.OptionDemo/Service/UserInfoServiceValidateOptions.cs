using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _05.Configuration.OptionDemo.Service
{
    public class UserInfoServiceValidateOptions : IValidateOptions<UserInfoOptions>
    {
        public ValidateOptionsResult Validate(string name, UserInfoOptions options)
        {
            if (options.name != "张三")
                return ValidateOptionsResult.Fail("name 值配置错误！");
            else
                return ValidateOptionsResult.Success;
        }
    }
}
