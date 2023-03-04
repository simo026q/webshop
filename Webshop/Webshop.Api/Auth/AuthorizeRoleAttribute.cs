using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Security.Claims;
using Webshop.Api.Entities;

namespace Webshop.Api.Auth;

public class AuthorizeRoleAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly UserRole[] _roles;

    public AuthorizeRoleAttribute(params UserRole[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>();

        if (allowAnonymous != null)
            return;

        var role = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

        if (role == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!Enum.TryParse<UserRole>(role, out var userRole))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (!_roles.Contains(userRole))
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}
