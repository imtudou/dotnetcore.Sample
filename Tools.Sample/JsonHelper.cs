using System;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace Tools.Sample
{
    public static class JsonHelper
    {
        public static bool IsJson(this string str) 
        {
            if (string.IsNullOrWhiteSpace(str)) return false;

            return true;        
        }
        public static string ToDateTimeStr(this DateTime dateTime)
        {
            return Convert.ToDateTime(dateTime).ToString();
        }       

         
    }
}
