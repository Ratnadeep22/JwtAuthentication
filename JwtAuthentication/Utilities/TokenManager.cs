using Microsoft.IdentityModel.Tokens;
using ModelObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Utilities
{
    public class TokenManager : ITokenManager
    {
        private readonly JwtInfo _jwtInfo;
        public TokenManager(JwtInfo jwtInfo)
        {
            _jwtInfo = jwtInfo;
        }

        public string GenerateAccessToken(string username, string? userrole=null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtInfo.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expirationtime = DateTime.UtcNow.AddMinutes(_jwtInfo.ExpiryInMins);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, expirationtime.ToString(), ClaimValueTypes.Integer),
            };

            if (userrole is not null)
                claims.Append(new Claim(JwtRegisteredClaimNames.Acr, userrole));

            var accesstoken = new JwtSecurityTokenHandler().WriteToken(
                    new JwtSecurityToken(
                            _jwtInfo.Audience,
                            _jwtInfo.Issuer,
                            claims,
                            expires: expirationtime,
                            signingCredentials: credentials)
                );

            return accesstoken;
        }

        public string GenerateRefreshToken(string accessToken)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(accessToken));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expirationtime = DateTime.UtcNow.AddHours(_jwtInfo.RefreshTokenExpiryInHrs);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, "refreshToken"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, expirationtime.ToString(), ClaimValueTypes.Integer),
            };

            var token = new JwtSecurityToken(
                _jwtInfo.Audience,
                _jwtInfo.Issuer,
                claims,
                expires: expirationtime,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenModel GenerateToken(string username, string userrole = null)
        {
            var tokenmodel = new TokenModel();
            tokenmodel.Token = GenerateAccessToken(username, userrole);
            tokenmodel.RefreshToken = GenerateRefreshToken(tokenmodel.Token);
            tokenmodel.UserName = username;

            return tokenmodel;
        }

        public bool ValidateRefreshToken(string tokenString, string secretKey)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidAudience = _jwtInfo.Audience,
                    ValidIssuer = _jwtInfo.Issuer,
                };

                SecurityToken validatedToken;
                tokenHandler.ValidateToken(tokenString, validationParameters, out validatedToken);
                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                Console.WriteLine("Token has expired.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return false;
            }
        }

    }
}
