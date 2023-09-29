using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EmailService.Email
{
    public interface IEmailController
    {
        Task<HttpStatusCode> SendEmail([FromBody] Email email);
    }
}
