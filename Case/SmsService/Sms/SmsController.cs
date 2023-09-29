using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;

namespace SmsService.Sms
{
    [Route("/Sms")]
    public class SmsController : Controller,ISmsController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public SmsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<HttpStatusCode> SendSms([FromBody] Sms sms)
        {
            string key = _configuration.GetValue<string>("SmsApiKey");
            string url = $"https://twilioproxy.azurewebsites.net/api/SendSMS?code={key}";

            using (var c = _httpClientFactory.CreateClient())
            {
                c.DefaultRequestHeaders.Accept.Clear();
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                c.DefaultRequestHeaders.Add("x-auth-token", key);
                SmsWithKey smsWithKey = new SmsWithKey(sms.receiver, sms.message, _configuration.GetValue<string>("SmsKey"));
                var result = await c.PostAsJsonAsync(url, smsWithKey);
                return result.StatusCode;
            }

        }
    }
}
