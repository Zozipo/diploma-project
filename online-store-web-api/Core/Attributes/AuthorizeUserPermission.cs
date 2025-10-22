using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Core.Attributes
{
    public class AuthorizeUserPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var userRoles = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            var userIdClaim = user.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

            if (userIdClaim == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var routeUserId = context.RouteData.Values["userId"]?.ToString();

            if (userIdClaim == routeUserId || userRoles.Contains("Admin"))
            {
                return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
