using System.Diagnostics;
using System.Net;
using ProgTracker.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProgTracker.WebApi.Controllers;

public class BaseApiController : ControllerBase
{
    protected readonly IMediator Mediator;

    protected BaseApiController(IMediator mediator)
    {
        Mediator = mediator;
    }
    
    [NonAction]
        protected new OkObjectResult Ok()
        {
            return base.Ok(new
            {
                Success = true
            });
        }

        [NonAction]
        protected OkObjectResult Ok(string data)
        {
            return base.Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [NonAction]
        protected new OkObjectResult Ok(object data)
        {
            return base.Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [NonAction]
        protected static JsonResult Fail(HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return Fail("", statusCode);
        }

        [NonAction]
        protected static JsonResult Fail(object error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var json = new JsonResult(new { Success = false, Error = error })
            {
                StatusCode = (int)statusCode
            };

            return json;
        }

        [NonAction]
        protected static JsonResult Fail(object error,string errorType, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var json = new JsonResult(new { Success = false, Error = error, ErrorType = errorType }) {
                StatusCode = (int)statusCode
            };

            return json;
        }

        [NonAction]
        protected static JsonResult JsonResult(object data, int statusCode)
        {
            var success = statusCode >= 200 && statusCode < 300;
            return new JsonResult(new {Success = success, Data = data})
            {
                StatusCode = statusCode
            };
        }

        [Conditional("DEBUG")]
        protected void DebugOnlyAddHeaderException(Exception e)
        {
            HttpContext.Request.RouteValues.TryGetValue("action", out var action);
            HttpContext.Request.RouteValues.TryGetValue("controller", out var controller);

            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                Controller = controller,
                Action = action,
                e.Message,
                e.StackTrace,
                e.InnerException,
               
            });

            HttpContext.Response.Headers.Add("x-desenv", json);
        }
        
        protected string GetRemoteIp()
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                remoteIpAddress = Request.Headers["X-Forwarded-For"];

            return remoteIpAddress;
        }

        protected string GetUserAgent()
        {
            return HttpContext.Request.Headers["User-Agent"].ToString();
        }
        
        protected async Task<ActionResult> Send<T>(IRequest<Response<T>> request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await Mediator.Send(request, cancellationToken);

                return response.Errors.Any() ? Fail(response.Errors) : JsonResult(response.Result, response.Code);
            }
            catch (Exception e)
            {
                DebugOnlyAddHeaderException(e);
                return Fail("Ocorreu um erro interno. Se o problema persitir por favor entre em contato", HttpStatusCode.InternalServerError);
            }
        }

        protected async Task<ActionResult> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await Mediator.Send(request, cancellationToken);
                return Ok(result);
            }
            catch (Exception e)
            {
                DebugOnlyAddHeaderException(e);
                return Fail("Ocorreu um erro interno. Se o problema persitir por favor entre em contato", HttpStatusCode.InternalServerError);
            }
        }
}