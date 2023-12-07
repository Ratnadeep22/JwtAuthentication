namespace ModelObjects
{
    [Serializable]
    public class TokenModel
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
