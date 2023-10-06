using EventService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Metadata.Ecma335;

namespace EventService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventDb eventDb;

        public EventController(IEventDb eventDb)
        {
            this.eventDb = eventDb;
        }

        /// <summary>
        /// Kaldes af en anden service, når den har brug for at offentliggøre (publishe) et event
        /// </summary>
        /// <param name="e">Information om den event, der er opstået</param>
        [HttpPost("/order")]
        public void RaiseOrderEvent(Order order) {
            eventDb.Raise(order);
        }

        /// <summary>
        /// Henter events
        /// </summary>
        /// <param name="startIndex">Index på det første event der skal hentes</param>
        /// <param name="antal">Antallet af events der maksimalt skal hentes (der kan være færre)</param>
        /// <returns></returns>
        [HttpGet("/order")]
        public IEnumerable<Event> ListEvents(long startIndex, long antal)
        {
            return eventDb.GetEvents(startIndex, antal);
        }
    }
}
