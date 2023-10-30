using CarTypeService.Models;
using CarTypeService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRegistrationTest.Mocks
{
    internal class MotorApiServiceMock : IMotorApiService
    {
        public async Task<CarDescription> GetDescriptionAsync(string licensePlate)
        {
            CarDescription description = new CarDescription();
            description.Make = "Kia";
            description.Model = "Ceed";
            description.Variant = "1.6";
            return description;
        }
    }
}
