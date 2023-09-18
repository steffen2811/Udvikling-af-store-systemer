using CarTypeService.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ParkingRegistration.Parking
{
    public class Parking
    {
        public string LicensePlate { get; set; }
        public DateTime TimeStart { get; private set; } = DateTime.Now;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public int Phone { get; set; }
        public ParkingSpot parkingSpot { get; set; }
        public bool IsActive { get; private set; } = true;
        public DateTime TimeEnd { get; private set; }
        public CarDescription? CarDescription { get; private set; } = null;

        public void EndParking()
        {
            IsActive = false;
            TimeEnd = DateTime.Now;
        }

        public void SetCarDescription(CarDescription carDescription) => CarDescription = carDescription;

        public class ParkingSpot
        {
            public int Level { get; set; }
            public int Number { get; set; }
        }
    }
}
