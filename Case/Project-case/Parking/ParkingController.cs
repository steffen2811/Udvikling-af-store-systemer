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

        [HttpGet("{licensePlate}")]
        public Parking Get(string licensePlate) => this.parkingStore.Get(licensePlate);

        [HttpPost("RegisterParking")]
        public Parking Post([FromBody] Parking parking)
        {
            this.parkingStore.Save(parking);
            return parking;
        }

        [HttpDelete("DeleteAllParking/{licensePlate}")]
        public IActionResult Delete(string licensePlate)
        {
            this.parkingStore.DeleteAll(licensePlate);
            return new NoContentResult();
        }
    }
}
