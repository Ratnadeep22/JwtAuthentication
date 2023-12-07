using JwtAuthentication.Utilities;
using Microsoft.AspNetCore.Mvc;
using ModelObjects;

namespace JwtAuthentication.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public TokenModel _tokenModel;
    private readonly ITokenManager _tokenManager;

    protected BaseController(ITokenManager tokenManager, TokenModel tokenModel, IConfiguration configuration)
    {
        this._tokenManager = tokenManager;
        this._tokenModel = tokenModel;
        this._configuration = configuration;
    }

    protected TokenModel GenerateRefreshToken() {
        
        var retVal = new TokenModel();
        
        if (_tokenManager.ValidateRefreshToken(_tokenModel.RefreshToken, _tokenModel.Token))
        {
            retVal = _tokenManager.GenerateToken(_tokenModel.UserName);
        }
        return retVal;
    }

    protected TokenModel GetToken(string username, string userrole=null)
    {
        var retVal = _tokenManager.GenerateToken(username, userrole);
        return retVal;
    }
}
