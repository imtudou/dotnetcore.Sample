using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Common.ApiResponse
{
    public class ApiResponse: BaseApiResponse<object>
    {
        /// <summary>
        /// 重写ToString()方法返回JSON字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            return jsonStr;
        }

    }
}
