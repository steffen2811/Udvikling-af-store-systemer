using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingControlService.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace ParkingControlService.Controllers
{
    [Route("/control")]
    public class ParkingControlController : Controller, IParkingControlController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ParkingControlController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [RequestFormLimits(ValueLengthLimit = int.MaxValue,
        MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<IActionResult> ControlParking([FromForm] ParkingControl parkingControl)
        {
            var ms = new MemoryStream();
            parkingControl.imageFile.CopyTo(ms);
            var fileBytes = ms.ToArray();
            string base64String = Convert.ToBase64String(fileBytes);

            using (var c = _httpClientFactory.CreateClient())
            {
                var obj = new
                {
                    image = base64String,
                };
                var result = await c.PostAsJsonAsync($"http://plate_recognizer_service/PlateRecognizer", base64String);
                var content = result.Content.ReadAsStringAsync();
                LicensePlate licensePlate = Newtonsoft.Json.JsonConvert.DeserializeObject<LicensePlate>(content.Result);

                if ((licensePlate != null) && (licensePlate.score < 80))
                {
                    HttpResponseMessage parkingResponse = c.GetAsync($"http://parking_registration/parking/ActiveParking/{licensePlate.plate}").Result;
                    if (parkingResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var parkingResponseInfo = parkingResponse.Content.ReadAsStringAsync();
                        ParkingInfo parkingInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<ParkingInfo>(parkingResponseInfo.Result);
                        if ((parkingInfo != null) && (parkingInfo.parkingSpot.level == parkingControl.level) && (parkingInfo.parkingSpot.number == parkingControl.number))
                            return StatusCode(200, "Parking OK");
                        else
                            return StatusCode(200, $"Licenseplate {licensePlate.plate} found in system but not at level: {parkingControl.level}, number: {parkingControl.number}");

                    }
                    else
                    {
                        return StatusCode(200, $"Licenseplate {licensePlate.plate} not found in system.");
                    }


                }
                else
                {
                    return StatusCode(200, "No licenseplate found");
                }
            }
        }
    }
}
