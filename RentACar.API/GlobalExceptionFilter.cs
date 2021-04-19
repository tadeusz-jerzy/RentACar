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
            if (context.Exception is BusinessException)
            {
                
                var type = context.Exception.GetType().Name;
                var exception = (BusinessException)context.Exception;
                var details = new
                {
                    Status = 400,
                    Title = "Bad Request",
                    Message = exception.Message,
                    BusinessException = type,
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
