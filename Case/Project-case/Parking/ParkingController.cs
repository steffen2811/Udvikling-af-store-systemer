using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Project_case.Parking.ParkingStore;
using Microsoft.AspNetCore.Http;

namespace Project_case.Parking
{
    [Route("/parking")]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingStore parkingStore;

        public ParkingController(IParkingStore parkingStore)
        {
            this.parkingStore = parkingStore;
        }

        [HttpGet("Parkings/{licensePlate}")]
        public List<Parking> GetAll(string licensePlate) => this.parkingStore.GetAllParkings(licensePlate);
        
        [HttpGet("ActiveParking/{licensePlate}")]
        public Parking GetActive(string licensePlate) => this.parkingStore.GetActiveParking(licensePlate);

        [HttpPost("RegisterParking")]
        public IActionResult RegisterParking([FromBody] Parking parking)
        {
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
