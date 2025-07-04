using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace _09.ExceptionDemo.Exceptions
{
    public interface IMyException
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public object[] Content { get; set; }
    }

    public class MyException :Exception,IMyException
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public object[] Content { get; set; }


        public MyException() { }
        public MyException(string Msg) 
        {
            this.Msg = Msg;
        }
        public MyException(int Code, string Msg)
        {
            this.Code = Code;
            this.Msg = Msg;
        }
        public MyException(int Code, string Msg, object[] Content)
        {
            this.Code = Code;
            this.Msg = Msg;
            this.Content = Content;
        }

        public void SetError(int Code, string Msg, object[] Content = null)
        {
            new MyException(Code, Msg, Content);
        }

        public static readonly IMyException Unknown = new MyException(9999, Msg: "未知错误");


    }
}
