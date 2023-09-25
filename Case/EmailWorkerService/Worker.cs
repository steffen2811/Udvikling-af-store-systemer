using EmailWorkerService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace EmailWorkerService
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
            int nextEventIndex = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                string key = "qMTJzZtnKGD4c0LgyYHyepoT7VdFOir1Wig9yrU6LeQLAzFuCJeiWw==";
                string url = $"https://twilioproxy.azurewebsites.net/api/SendEmail?code={key}";

                using (var c = _httpClientFactory.CreateClient())
                {
                    var response = await c.GetAsync($"https://localhost:7186/events?start={nextEventIndex}");
                    string events = await response.Content.ReadAsStringAsync();

                    dynamic json = JsonConvert.DeserializeObject(events);
                    foreach (var item in json)
                    {
                        string lisenceplate = item.content.licensePlate;
                        string email = item.content.email;

                        c.DefaultRequestHeaders.Accept.Clear();
                        c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        c.DefaultRequestHeaders.Add("x-auth-token", key);
                        Email body = new Email(email, "Parking", $"Car with licenseplate {lisenceplate} has been registered");

                        var result = await c.PostAsJsonAsync(url, body);

                        if (result.IsSuccessStatusCode)
                        {
                            _logger.LogInformation("Email sent");
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