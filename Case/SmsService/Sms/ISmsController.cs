using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SmsService.Sms
{
    public interface ISmsController
    {
        Task<HttpStatusCode> SendSms([FromBody] Sms sms);
    }
}
