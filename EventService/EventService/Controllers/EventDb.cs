using EventService.Models;

namespace EventService.Controllers
{
    public interface IEventDb
    {
        IEnumerable<Event> GetEvents(long firstEvent, long lastEvent);
        public void Raise(object eventObject);
    }

    public class EventStore : IEventDb
    {
        private static long currentSequenceNumber = 0;
        private static readonly IList<Event> Database = new List<Event>();

        public IEnumerable<Event> GetEvents(long firstEvent, long numberOfEvents)
          => Database
            .Where(e =>
              e.EventNumber >= firstEvent &&
              e.EventNumber <= firstEvent+numberOfEvents)
            .OrderBy(e => e.EventNumber);

        public void Raise(object eventObject)
        {
            long seqNumber = Interlocked.Increment(ref currentSequenceNumber);
            Database.Add(new Event(seqNumber,DateTime.Now,eventObject));
        }
    }
}
