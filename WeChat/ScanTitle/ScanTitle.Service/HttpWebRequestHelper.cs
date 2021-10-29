using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ScanTitle
{
    public static class HttpWebRequestHelper
    {
        /// <summary>
        /// GET请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">URL包含参数</param>
        /// <returns></returns>
        public static async Task<T> HttpGet<T>(string url) where T : new()
        {
            try
            {
                var handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip
                };

                using (var httpclient = new HttpClient(handler))
                {
                    httpclient.BaseAddress = new Uri(url);
                    httpclient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await httpclient.GetAsync(url);
                    //确保HTTP的状态值
                    response.EnsureSuccessStatusCode();
                    string responseStr = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(responseStr);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">参数</param>
        /// <param name="url">url包含参数</param>
        /// <param name="timout">响应时间</param>
        /// <returns></returns>
        public static async Task<T> HttpPost<T>(FormUrlEncodedContent param, string url, int timout) where T:new()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.Proxy = null;
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var httpClicent = new HttpClient(handler))
            {
                httpClicent.BaseAddress = new Uri(url);
                httpClicent.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClicent.PostAsync(url,param);
                string responseStr = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(responseStr);
            }

        }

        /// <summary>
        /// POST请求 
        /// </summary>
        /// <param name="parms">参数（唯一）</param>
        /// <param name="url">接口</param>
        /// <param name="timeout">响应时间</param>
        /// <returns></returns>
        public static  string HttpPostOne(string param, string url, int timeout)
        {
            try
            {
                var httpwebRequest = WebRequest.Create(url) as HttpWebRequest;
                httpwebRequest.Proxy = null;
                httpwebRequest.ContinueTimeout = timeout;
                httpwebRequest.Method = "POST";
                httpwebRequest.ContentType = "application/json";
                httpwebRequest.Headers["Accept-Encoding"] = "gzip,deflate";

                byte[] data = Encoding.UTF8.GetBytes(param);

                using (var requestStream= httpwebRequest.GetRequestStream())//释放
                {
                    requestStream.Write(data,0,data.Length);
                }

                string result = string.Empty;
                //响应流
                using (var response = (HttpWebResponse)httpwebRequest.GetResponse())
                {
                    Stream responseStream = null;
                    if (response.StatusCode==HttpStatusCode.OK)
                    {
                        responseStream = response.GetResponseStream();
                        if (responseStream!=null)
                        {
                            var streamReader = new StreamReader(responseStream, Encoding.UTF8);
                            //获取返回信息
                            result = streamReader.ReadToEnd();
                            return result;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return "";
        }

        /// <summary>
        /// POIST请求
        /// </summary>
        /// <param name="list">键值对的方式存储多个参数（多个）</param>
        /// <param name="url">URL包含参数</param>
        /// <returns></returns>
        public async static Task<string> HttpPosts(List<KeyValuePair<string, string>> list, string url)
        {
            using (var client = new HttpClient())
            {
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("key1", "values1"));
                values.Add(new KeyValuePair<string, string>("key2", "values2"));

                var content = new FormUrlEncodedContent(list);
                var response = await client.PostAsync(url, content);
                var responseStr = await response.Content.ReadAsStringAsync();
                return responseStr;
            }
        }
    }
}       