using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsWorkerService.Models
{
    public class Sms
    {
        public string receiver { get; set; }
        public string key { get; set; }
        public string message { get; set; }

        public Sms(string reveiver, string key, string message) 
        { 
            this.receiver = reveiver;
            this.key = key;
            this.message = message;
        }
    }
}
