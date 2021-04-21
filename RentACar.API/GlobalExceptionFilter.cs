using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RentACar.Core.Exceptions;
using System.Net;


namespace RentACar.API
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // custom handling for domain exceptions
            if (context.Exception is BusinessException businessException)
            {
                var statusCode = businessException switch
                {
                    EntityNotFoundException e => 404,
                    _ => 400
                };

                var typeName = context.Exception.GetType().Name;
                var exception = businessException;
                var details = new
                {
                    Status = 400,
                    Title = "Bad Request",
                    Message = exception.Message,
                    BusinessException = typeName,
                    Trace = context.HttpContext.TraceIdentifier
                };

                var json = new
                {
                    errors = new[] { details }
                };

                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.ExceptionHandled = true;
            } 
            


        }
    }
}
