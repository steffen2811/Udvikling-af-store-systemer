using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventService.models
{
    public class Sms
    {
        public string receiver { get; set; }
        public string message { get; set; }

        public Sms(string receiver, string message)
        {
            this.receiver = receiver;
            this.message = message;
        }
    }
}
