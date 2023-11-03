using Microsoft.AspNetCore.Mvc;

namespace ParkingRegistration.EventFeed
{
    [Route("/events")]
    public class EventFeedController : Controller
    {
        private readonly IEventStore eventStore;
        private const int MAX_COUNT = 4096;

        public EventFeedController(IEventStore eventStore) => this.eventStore = eventStore;

        [HttpGet("")]
        public async Task<Event[]> Get([FromQuery] int start, [FromQuery] int count = MAX_COUNT)
        {
            if (count > MAX_COUNT)
                count = MAX_COUNT;

            IEnumerable<Event> returnEvents = await eventStore.GetEvents(start, count, typeof(Parking.Parking));
            return returnEvents.ToArray();
        }
    }
}
