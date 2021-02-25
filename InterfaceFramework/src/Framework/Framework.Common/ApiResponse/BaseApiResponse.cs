using System;
using System.Net;

namespace Framework.Common.ApiResponse
{
    public class BaseApiResponse<T>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseApiResponse()
        {
            this.Head = new Header();
            this.Data = default(T);
        }

        /// <summary>
        /// 响应头
        /// </summary>
        public Header Head { get; set; }

        /// <summary>
        /// 响应体
        /// </summary>
        public T Data { get; set; }


    }


    

    public class Header
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Header()
        {
            this.IsSuccess = true;
            this.Code = (int)ErrCode.Sucess;
            this.Msg = CommonHelper.GetEnumDescription(ErrCode.Sucess);
        }
        /// <summary>
        /// 枚举
        /// </summary>
        /// <param name="code"></param>
        public Header(ErrCode code)
        {
            if (code != ErrCode.Sucess)
                this.IsSuccess = false;
            else
                this.IsSuccess = true;
            this.Code = (int)code;
            this.Msg = CommonHelper.GetEnumDescription(code);
        }

        public Header(bool isSuccess, int code, string msg)
        {
            this.IsSuccess = IsSuccess;
            Code = code;
            Msg = msg;
        }

        /// <summary>
        /// 返回值
        /// true   :成功
        /// false  :失败
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Msg { get; set; }



    }
    
}
