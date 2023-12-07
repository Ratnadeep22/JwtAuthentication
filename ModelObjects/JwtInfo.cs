namespace ModelObjects
{
    public class JwtInfo
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryInMins { get; set; } = 1;
        public int RefreshTokenExpiryInHrs { get; set; } = 10*10;
    }
}
