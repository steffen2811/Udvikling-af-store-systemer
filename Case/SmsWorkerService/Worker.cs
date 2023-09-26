using Newtonsoft.Json;
using SmsWorkerService.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SmsWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            int nextEventIndex = 1;

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                string key = config["SmsApiKey"];
                string url = $"https://twilioproxy.azurewebsites.net/api/SendSMS?code={key}";

                using (var c = _httpClientFactory.CreateClient())
                {
                    var response = await c.GetAsync($"https://localhost:7186/events?start={nextEventIndex}");
                    string events = await response.Content.ReadAsStringAsync();

                    dynamic json = JsonConvert.DeserializeObject(events);
                    foreach (var item in json)
                    {
                        string lisenceplate = item.content.licensePlate;
                        string phone = item.content.phone;

                        c.DefaultRequestHeaders.Accept.Clear();
                        c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        c.DefaultRequestHeaders.Add("x-auth-token", key);
                        Sms body = new Sms(phone, config["SmsKey"], $"Car with licenseplate {lisenceplate} has been registered");

                        var result = await c.PostAsJsonAsync(url, body);

                        if (result.IsSuccessStatusCode)
                        {
                            _logger.LogInformation("SMS sent");
                        }
                        else
                        {
                            _logger.LogInformation("Something is wrong");
                        }
                        nextEventIndex++;
                    }
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}