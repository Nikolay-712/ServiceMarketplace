using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceMarketplace.Models;
using System.Net;

namespace ServiceMarketplace.Infrastructure.Filters;

public class ModelStateFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            IReadOnlyList<string> errors = context.ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage).ToList();

            ModelStateErrorResponse errorResponse = new()
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ValidationMessages = errors
            };

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new ObjectResult(errorResponse);
        }
    }
}
