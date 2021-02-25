using System.ComponentModel;

namespace Framework.Common.ApiResponse
{
            
    public enum ErrCode
    {
        [Description("成功!")]
        Sucess = 200,
        [Description("未经授权!")]
        Unauthorized = 401,
        [Description("未找到!")]
        NotFound=404
    }
}
