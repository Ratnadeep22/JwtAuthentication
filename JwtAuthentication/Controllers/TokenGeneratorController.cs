using JwtAuthentication.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelObjects;

namespace JwtAuthentication.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]/[action]")]
[ApiController]
public class TokenGeneratorController : BaseController
{

    public TokenGeneratorController(ITokenManager tokenManager, TokenModel tokenModel, IConfiguration configuration): base(tokenManager, tokenModel, configuration)
    {

    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login(LoginModel loginModel)
    {
        if (loginModel.UserName != "RSK" || loginModel.Password != "RSK")
            return BadRequest();
        TokenModel tokenModel = GetToken(loginModel.UserName);

        return Ok(tokenModel);
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult GetRefreshToken([FromBody] TokenModel tokenModel1)
    {
        _tokenModel = tokenModel1;

        TokenModel refreshedtoken = GenerateRefreshToken();
        if (refreshedtoken is null)
            return BadRequest();

        return Ok(refreshedtoken);
    }

    [HttpGet]
    public IActionResult TestToken()
    {
        return Ok("User is successfully authenticated...");
    }
}
