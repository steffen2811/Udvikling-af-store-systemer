using Microsoft.AspNetCore.Mvc;
using PlateRecognizer;

namespace PlateRecognizerService.Controllers
{
    [Route("/PlateRecognizer")]
    public class PlateRecognizerController : Controller
    {
        private readonly IConfiguration _configuration;

        public PlateRecognizerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult ControlParking([FromBody] Byte[] image)
        {
            string postUrl = "http://api.platerecognizer.com/v1/plate-reader/";
            string regions = "dk";
            string key = _configuration.GetValue<string>("Token");

            PlateReaderResult plateReaderResult = PlateReader.Read(postUrl, null, image, regions, key, false);

            if (plateReaderResult.Results.Count == 0)
            {
                Console.WriteLine("Der blev ikke fundet nogen nummerplader");
                return StatusCode(404);
            }

            Result result = plateReaderResult.Results[0];
            Console.WriteLine($"Nummerplanen er  {result.Plate} med {result.Score * 100: 0}% sandsynlighed");

            PlateRecognizer plateRecognizer = new PlateRecognizer(result.Plate, result.Score);
            return StatusCode(200, plateRecognizer);
        }
    }
}
