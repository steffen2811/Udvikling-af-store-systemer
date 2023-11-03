using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace ParkingRegistration.EventFeed
{
    public interface IEventStore
    {
        Task<IEnumerable<Event>> GetEvents(int first, int count, Type type);
        void Raise(string eventName, object content);
    }

    public class EventStore : IEventStore
    {
        private readonly IEventStoreConnection connection;

        public EventStore(IEventStoreConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<Event>> GetEvents(int first, int count, Type type)
        {
            var streamName = "parking";
            var slice = await connection.ReadStreamEventsForwardAsync(streamName, first, count, false);

            // Convert the events in the slice to a list of objects
            var eventsList = slice.Events
                .Select(resolvedEvent => new Event
                {
                    SequenceNumber = (int)resolvedEvent.Event.EventNumber,
                    OccuredAt = resolvedEvent.OriginalEvent.Created,
                    Name = streamName,
                    Content = JsonConvert.DeserializeObject(System.Text.Encoding.Default.GetString(resolvedEvent.Event.Data), type)
                })
                .ToList();
            return eventsList;
        }

        public async void Raise(string eventName, object content)
        {
            byte[] eventData = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(content);
            var eventToSave = new EventData(
                Guid.NewGuid(), // EventId
                eventName, // EventType
                true, // IsJson
                eventData, // Event data
                null // Metadata
            );

            await connection.AppendToStreamAsync(eventName, ExpectedVersion.Any, eventToSave);
        }
    }
}
