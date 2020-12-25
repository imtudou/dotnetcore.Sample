using System;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace Tools.Sample
{
    public static class JsonHelper
    {
        public static bool IsJson(this string str) 
        {
            return string.IsNullOrWhiteSpace(str);
        }
        public static string ToDateTimeStr(this DateTime dateTime)
        {
            return Convert.ToDateTime(dateTime).ToString();
        }       

         
    }
}
