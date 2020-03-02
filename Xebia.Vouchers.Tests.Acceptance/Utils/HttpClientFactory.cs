using System;
using System.Dynamic;
using System.Net.Http;

namespace Xebia.Vouchers.Tests.Acceptance
{
    public class HttpClientFactory
    {
        private HttpClient _client;
        
        public HttpClient Get()
        {
            if (_client == null)
            {
                _client = new HttpClient();
                _client.BaseAddress = new Uri("http://localhost:8080/api/");
                _client.Timeout = TimeSpan.FromSeconds(5);
            }
            
            return _client;
        }
    }
}