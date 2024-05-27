using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LostPolygon.AspNetCore.Utility;

public class DisabledAttribute : ActionFilterAttribute, IAuthorizationFilter {
    // ReSharper disable once UnusedParameter.Local
    public DisabledAttribute(string reason) {
    }

    public override void OnActionExecuting(ActionExecutingContext context) {
        context.Result = new NotFoundResult();
    }

    public void OnResourceExecuting(ResourceExecutingContext context) {
        context.Result = new NotFoundResult();
    }

    public void OnAuthorization(AuthorizationFilterContext context) {
        context.Result = new NotFoundResult();
    }
}
