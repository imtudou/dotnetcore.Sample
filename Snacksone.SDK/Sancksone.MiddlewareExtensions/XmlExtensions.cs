using System;
using System.Collections.Generic;
using System.Text;

namespace Sancksone.MiddlewareExtensions
{
    public static class XMLExtensions
    {
        public static string ToXmlStr<T>(this T obj, string methodName, string token = "") where T : class, new()
        {
            var sb = new StringBuilder();
            var sb2 = new StringBuilder();
            var t = obj.GetType();
            foreach (var info in t.GetProperties())
            {
                sb2.Append("<" + info?.Name + ">");
                sb2.Append(t.GetProperty(info?.Name).GetValue(obj));
                sb2.Append("</" + info?.Name + ">");
            }
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sb.Append("<soap:Body>");
            sb.Append("<" + methodName + " xmlns=\"http://tempuri.org/\">");
            sb.Append(" <request>");
            sb.Append("<Header>");
            sb.Append("<Token>");
            sb.Append(token);
            sb.Append("</Token>");
            sb.Append("</Header>");
            sb.Append("<Body>");
            sb.Append(sb2.ToString());
            sb.Append("</Body>");
            sb.Append("</request>");
            sb.Append("</" + methodName + ">");
            sb.Append("</soap:Body>");
            sb.Append("</soap:Envelope>");

            return sb.ToString();
        }
    }
}
