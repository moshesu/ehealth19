using Newtonsoft.Json;

namespace CannaBe
{
    class SendEmailRequest : Request
    {
        [JsonConstructor]
        public SendEmailRequest(string emailAddress, string freeMessage)
        {
            EmailAddress = emailAddress;
            FreeMessage = freeMessage;
        }

        [JsonProperty("to")]
        public string EmailAddress { get; set; }

        [JsonProperty("content")]
        public string FreeMessage { get; set; }
    }
}
