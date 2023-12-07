using ModelObjects;

namespace JwtAuthentication.Utilities
{
    public interface ITokenManager
    {
        TokenModel GenerateToken(string username, string userrole = null);
        string GenerateAccessToken(string username, string userrole = null);
        string GenerateRefreshToken(string accessToken);

        bool ValidateRefreshToken(string tokenString, string secretKey);

    }
}