using Newtonsoft.Json;

namespace CannaBe
{
    class LoginRequest : Request
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public LoginRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public LoginRequest() { }
    }
}
