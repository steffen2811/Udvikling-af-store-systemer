namespace ParkingRegistration.EventFeed
{
    public class Event
    {
        public long SequenceNumber { get; set; }
        public DateTimeOffset OccuredAt { get; set; }
        public string Name { get; set; }
        public object Content { get; set; }

        public Event(long SequenceNumber, DateTimeOffset OccuredAt, string Name, object Content)
        {
            this.SequenceNumber = SequenceNumber;
            this.OccuredAt = OccuredAt;
            this.Name = Name;
            this.Content = Content;
        }
    }
}
