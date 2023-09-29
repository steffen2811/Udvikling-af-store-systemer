namespace EmailService.Email
{
    public class Email
    {
        public string receiver { get; set; }
        public string subject { get; set; }
        public string message { get; set; }

        public Email(string receiver, string subject, string message)
        {
            this.receiver = receiver;
            this.subject = subject;
            this.message = message;
        }
    }
}
