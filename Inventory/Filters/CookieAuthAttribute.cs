using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

public class CookieAuthAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _configKey;

    public CookieAuthAttribute(string configKey)
    {
        _configKey = configKey;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
        var requiredValue = configuration[_configKey];

        var cookieValue = context.HttpContext.Request.Cookies["AccessLevel"];

        if (cookieValue != requiredValue)
        {
            context.Result = new RedirectToPageResult("/Accedi");
        }
    }
}

