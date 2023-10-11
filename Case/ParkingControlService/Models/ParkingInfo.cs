namespace ParkingControlService.Models
{
    public class ParkingInfo
    {
        public DateTime timeStart {  get; set; }
        public parkingSpot parkingSpot { get; set; }

    }
    public class parkingSpot
    {
        public string level { get; set; }
        public string number { get; set; }
    }
}
