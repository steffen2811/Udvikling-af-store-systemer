using EventService.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EventService.handlers
{
    public class ParkingEvent
    {
        static int nextEventIndex = 1;

        public static async Task parkingEvents(ILogger<Worker> logger, IHttpClientFactory httpClientFactory)
        {
            using (var c = httpClientFactory.CreateClient())
            {
                var response = await c.GetAsync($"https://localhost:7186/events?start={nextEventIndex}");
                string events = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(events);
                foreach (var item in json)
                {
                    string phone = item.content.phone;
                    string email = item.content.email;
                    string subject = $"New parking {item.content.licensePlate}";
                    string message = $"Car with licenseplate {item.content.licensePlate} has been parked at {item.content.timeStart}";
                    c.DefaultRequestHeaders.Accept.Clear();
                    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    logger.LogInformation(message);

                    if (phone != "")
                    {
                        Sms body = new Sms(phone, message);
                        var result = await c.PostAsJsonAsync($"https://localhost:7002/sms", body);
                        logger.LogInformation($"SMS response: {result.StatusCode}");
                    }

                    if (email != "")
                    {
                        Email body = new Email(email, subject, message);
                        var result = await c.PostAsJsonAsync($"https://localhost:7173/email", body);
                        logger.LogInformation($"Email response: {result.StatusCode}");
                    }
                    nextEventIndex++;
                }
            }
        }

    }
}
