namespace eHealthWorkshopGroup4.Models
{
    public class LiveIdCredentials
    {
        public string AccessToken { get; set; }

        public long ExpiresIn { get; set; }

        public string RefreshToken { get; set; }
    }
}
