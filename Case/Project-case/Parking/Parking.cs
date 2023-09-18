namespace Project_case.Parking
{
    public class Parking
    {
        public string LicensePlate { get; set; }
        public DateTime TimeStart { get; private set; } = DateTime.Now;
        public string Email { get; set; }
        public int Phone { get; set; }
        public ParkingSpot parkingSpot { get; set; }
        public bool IsActive { get; private set; } = true;
        public DateTime TimeEnd { get; private set; }


        public void EndParking()
        {
            IsActive = false;
            TimeEnd = DateTime.Now;
        }

        public class ParkingSpot
        {
            public int Level { get; set; }
            public int Number { get; set; }
        }
    }
}
