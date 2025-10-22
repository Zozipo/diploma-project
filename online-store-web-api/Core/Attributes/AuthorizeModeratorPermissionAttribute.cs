using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Core.Attributes
{
    public class AuthorizeModeratorPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var userId = user.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            var userRoles = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            if (userRoles.Contains("Admin"))
            {
                return;
            }
            if (userRoles.Contains("Moderator"))
            {
                if (userId != null)
                {
                    var controllerName = context.RouteData.Values["controller"]?.ToString();

                    if (controllerName != null)
                    {
                        var permissionsService = context.HttpContext.RequestServices.GetService<ITablePermissionsService>();

                        if (permissionsService != null)
                        {
                            var hasPermission = permissionsService.CanModifyTable(userId, controllerName).GetAwaiter().GetResult();

                            if (!hasPermission)
                            {
                                context.Result = new ForbidResult();
                            }
                        }
                        else
                        {
                            context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                        }
                    }
                    else
                    {
                        context.Result = new BadRequestResult();
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
