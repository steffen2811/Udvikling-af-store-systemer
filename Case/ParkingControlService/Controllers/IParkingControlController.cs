using Microsoft.AspNetCore.Mvc;
using ParkingControlService.Models;

namespace ParkingControlService.Controllers
{
    public interface IParkingControlController
    {
        Task<IActionResult> ControlParking([FromForm] ParkingControl parkingControl);
    }
}
