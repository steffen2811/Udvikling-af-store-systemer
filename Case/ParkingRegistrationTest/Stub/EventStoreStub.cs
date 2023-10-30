using ParkingRegistration.EventFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRegistrationTest.Stub
{
    internal class EventStoreStub : IEventStore
    {
        private static long currentSequenceNumber = 0;
        private static readonly IList<Event> Database = new List<Event>();

        public IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
        {
            return Database.Where(e =>
              e.SequenceNumber >= firstEventSequenceNumber &&
              e.SequenceNumber <= lastEventSequenceNumber)
            .OrderBy(e => e.SequenceNumber);
        }

        public void Raise(string eventName, object content)
        {
            var seqNumber = Interlocked.Increment(ref currentSequenceNumber);
            Database.Add(
              new Event(
                seqNumber,
                DateTimeOffset.UtcNow,
                eventName,
                content));
        }
    }
}
