namespace Project_case.Parking
{
    public class Parking
    {
        public string LicensePlate { get; set; }
        public DateTime Time { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public ParkingSpot parkingSpot { get; set; }

        public class ParkingSpot
        {
            public int Level { get; set; }
            public int Number { get; set; }
        }
    }
}
