using Microsoft.AspNetCore.Mvc;
using System.Net;
using static ParkingRegistration.Parking.ParkingStore;
using Microsoft.AspNetCore.Http;
using CarTypeService.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace ParkingRegistration.Parking
{
    [Route("/parking")]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingStore parkingStore;
        private readonly IMotorApiService motorApiService;

        public ParkingController(IParkingStore parkingStore, 
            IMotorApiService motorApiService)
        {
            this.parkingStore = parkingStore;
            this.motorApiService = motorApiService;
        }

        [HttpGet("Parkings/{licensePlate}")]
        public List<Parking> GetAll(string licensePlate) => this.parkingStore.GetAllParkings(licensePlate);
        
        [HttpGet("ActiveParking/{licensePlate}")]
        public Parking GetActive(string licensePlate) => this.parkingStore.GetActiveParking(licensePlate);

        [HttpPost("RegisterParking")]
        public async Task<IActionResult> RegisterParking([FromBody] Parking parking)
        {
            var carDescription = await motorApiService.GetDescriptionAsync(parking.LicensePlate);
            parking.SetCarDescription(carDescription);
            bool result = this.parkingStore.RegisterParking(parking);
            return result ? StatusCode(200) : StatusCode(406, "Licenseplate is already registered.");
        }

        [HttpPost("EndParking/{licensePlate}")]
        public IActionResult RegisterParking(string licensePlate)
        {
            bool result = this.parkingStore.EndParking(licensePlate);
            return result ? StatusCode(200) : StatusCode(406, "No active parking found");
        }

        [HttpDelete("DeleteAllParking/{licensePlate}")]
        public IActionResult Delete(string licensePlate)
        {
            this.parkingStore.DeleteAll(licensePlate);
            return StatusCode(200);
        }
    }
}
