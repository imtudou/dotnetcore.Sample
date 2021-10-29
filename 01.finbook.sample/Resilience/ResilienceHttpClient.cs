using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Polly;
using Polly.Wrap;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Http.Authentication;
using System.Net.Http.Headers;

namespace Resilience
{

    public class ResilienceHttpClient : IHttpClient
    {
        private readonly IHttpClient _httpClient;

        //根据url origin 去创建policy
        private readonly ILogger<ResilienceHttpClient> _logger;

        //把policy打包成组合 plicy wrapers 进行本地缓存
        private readonly Func<string, IEnumerable<Policy>> _policyCreator;

        private ConcurrentDictionary<string, PolicyWrap> _policyWrappers;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResilienceHttpClient(IHttpClient httpClient,
            ILogger<ResilienceHttpClient> logger,
            Func<string, IEnumerable<Policy>> policyCreator,
            ConcurrentDictionary<string, PolicyWrap> policyWrappers,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _httpClient = httpClient;
            _logger = logger;
            _policyCreator = policyCreator;
            _policyWrappers = policyWrappers;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<HttpResponseMessage> DoPostAsync<T>(HttpMethod method,string url, T item, string authorizationToken, string requestId = null, string authorizationMehod = "Brarer")
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be post or put", nameof(method));
            }

            var origin = GetOriginFormUri(url);
            //return HttpInvoker(origin, async () =>
            //{
            //    var requestMessage = new HttpResponseMessage(method, url);
            //    SetAuthorizationHeader(requestMessage);
            //    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/josn");

            //    if (!string.IsNullOrEmpty(authorizationToken))
            //    {
            //        requestMessage.Headers.WwwAuthenticate = new HttpHeaderValueCollection<AuthenticationHeaderValue>() { 
            //            new AuthenticationHeaderValue(authorizationMehod,authorizationToken)
            //        };
            //    }

            //    if (!string.IsNullOrEmpty(requestId))
            //    {
            //        requestMessage.Headers.Add("x-requestid", requestId);
            //    }


            //});

            return null;
            
        }

        public async Task<T> HttpInvoker<T>(string origin, Func<Context,Task<T>> action)
        {
            var nomealizedOrigin = NomalizerOrigin(origin);
            if (!_policyWrappers.TryGetValue(nomealizedOrigin,out PolicyWrap policyWrap))
            {
                policyWrap = Policy.Wrap(_policyCreator(nomealizedOrigin).ToArray());
                _policyWrappers.TryAdd(nomealizedOrigin, policyWrap);
            }
            return await policyWrap.Execute(action, new Context(nomealizedOrigin));
        }


        private static string NomalizerOrigin(string origin)
        {
            return origin?.Trim()?.ToLower();
        }

        private static string GetOriginFormUri(string uri)
        {
            var url = new Uri(uri);
            var origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";
            return origin;
        }

        private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                requestMessage.Headers.Add("Authorization", new List<string> { authorizationHeader });
            }
        }

        public Task<HttpResponseMessage> PostAsync<T>(string url, T item, string authorizationToken, string requestId = null, string authorizationMehod = "Brarer")
        {
            throw new NotImplementedException();
        }
    }
}
    