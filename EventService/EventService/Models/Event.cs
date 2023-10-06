namespace EventService.Models
{
    /// <summary>
    /// Information om den event, der er opstået
    /// Tænk over, hvilke data det er relevant at event'et indeholder
    /// </summary>
    public class Event
    {
        public long EventNumber { get; set; }
        public DateTime dateTime { get; set; }
        public object eventObject { get; set; }

        public Event(long EventNumber, DateTime dateTime, object eventObject)
        {
            this.EventNumber = EventNumber;
            this.dateTime = dateTime;
            this.eventObject = eventObject;
        }

    }
}
