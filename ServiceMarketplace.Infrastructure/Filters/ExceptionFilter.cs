using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions;
using ServiceMarketplace.Models;
using System.Net;

namespace ServiceMarketplace.Infrastructure.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment environment;
    private readonly ILogger _logger;

    public ExceptionFilter(IHostEnvironment environment, ILogger<ExceptionFilter> logger)
    {
        this.environment = environment;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        ErrorResponse error = new();

        switch (context.Exception)
        {
            case ClientException clientException:
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                error.StatusCode = (int)HttpStatusCode.BadRequest;
                error.Message = clientException.Message;
                break;
            case ServerException serverException:
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                error.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (environment.IsDevelopment())
                {
                    error.Message = serverException.Message;
                    break;
                }
                error.Message = "General error message";

                break;
            default:
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                error.StatusCode = (int)HttpStatusCode.InternalServerError;
                if (environment.IsDevelopment())
                {
                    error.Message = context.Exception.Message;
                    break;
                }

                error.Message = "General error message";
                break;
        }

        ResponseContent response = new ResponseContent()
        {
            ErrorResponse = error
        };

        context.Result = new ObjectResult(response);
        context.ExceptionHandled = true;
    }
}
