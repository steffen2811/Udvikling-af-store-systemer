using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace EmailService.Email
{
    [Route("/Email")]
    public class EmailController : Controller, IEmailController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public EmailController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<HttpStatusCode> SendEmail([FromBody] Email email)
        {
            string key = _configuration.GetValue<string>("EmailApiKey");
            string url = $"https://twilioproxy.azurewebsites.net/api/SendEmail?code={key}";

            using (var c = _httpClientFactory.CreateClient())
            {
                c.DefaultRequestHeaders.Accept.Clear();
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                c.DefaultRequestHeaders.Add("x-auth-token", key);
                var result = await c.PostAsJsonAsync(url, email);
                return result.StatusCode;
            }

        }
    }
}
