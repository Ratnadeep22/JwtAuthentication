using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModelObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TokenGeneratorController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly JwtBearerOptions _jwtOptions;

    public TokenGeneratorController(IConfiguration config, IOptions<JwtBearerOptions> jwtOptions)
    {
        _config = config;
        _jwtOptions = jwtOptions.Value;
    }

    [HttpGet]
    public IActionResult GenerateToken([FromBody] LoginModel loginModel)
    {
        if(loginModel.UserName != "RSK" || loginModel.Password != "RSK")
            return BadRequest();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, loginModel.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddDays(1).ToString()),
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
          _config["Jwt:Issuer"],
          claims,
          expires: DateTime.Now.AddMinutes(120),
          signingCredentials: credentials);

        TokenModel tokenModel = new()
        {
            UserName = loginModel.UserName,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
        
        return Ok(tokenModel);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public IActionResult TestToken()
    {
        
        return Ok("User is successfully authenticated...");
    }
}
