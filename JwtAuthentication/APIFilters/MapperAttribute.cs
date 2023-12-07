using JwtAuthentication.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JwtAuthentication.APIFilters
{
    public class MapperAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.Filters.OfType<MapperAttribute>().Any())
            {
                return;
            }
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"];
            var refreshTokenHeader = context.HttpContext.Request.Headers["RefreshToken"];
            if (string.IsNullOrEmpty(authorizationHeader) || string.IsNullOrEmpty(refreshTokenHeader))
            {
                // No token found, return unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }

            var basecontroller = (BaseController)context.Controller;
            basecontroller._tokenModel.Token = authorizationHeader.ToString().Split(" ")[1];
            basecontroller._tokenModel.RefreshToken = refreshTokenHeader.ToString();
            base.OnActionExecuting(context);
        }
    }
}
