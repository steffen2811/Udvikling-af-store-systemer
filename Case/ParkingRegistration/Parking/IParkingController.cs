using CarTypeService.Services;
using Microsoft.AspNetCore.Mvc;
using ParkingRegistration.EventFeed;

namespace ParkingRegistration.Parking
{
    public interface IParkingController
    {
        List<Parking> GetAll(string licensePlate);
        Parking GetActive(string licensePlate);
        Task<IActionResult> RegisterParking([FromBody] Parking parking);
        IActionResult RegisterParking(string licensePlate);
        IActionResult Delete(string licensePlate);
    }
}
