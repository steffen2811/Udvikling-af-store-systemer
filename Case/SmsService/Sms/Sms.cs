using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace SmsService.Sms
{

    public class SmsWithKey : Sms
    {
        public string key { get; set; }

        public SmsWithKey(string reveiver, string message, string key )
        {
            this.receiver = reveiver;
            this.message = message;
            this.key = key;
        }
    }

    public class Sms
    {
        public string receiver { get; set; }
        public string message { get; set; }
    }
}
