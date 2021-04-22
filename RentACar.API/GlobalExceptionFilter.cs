using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RentACar.Core.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;


namespace RentACar.API
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // custom handling for domain exceptions
            if (!(context.Exception is BusinessException businessException))
                return;
            
            int statusCode = businessException switch
            {
                EntityNotFoundException e => 404,
                _ => 400
            };

            string errorType = businessException switch
            {
                EntityNotFoundException e => "Not Found",
                _ => "Bad Request"
            };

            var typeName = context.Exception.GetType().Name;
            
            var json = new
            {
                // match default asp core validation errors structure
                type = errorType, 
                title = $"An exception {typeName} occurred",
                status = statusCode,
                traceId = Activity.Current?.Id ?? context.HttpContext?.TraceIdentifier, // https://stackoverflow.com/a/61395255
                errors = new Dictionary<string, object>() 
                {
                    {  typeName ,  new string[] { businessException.Message } } 
                } 
                
            };

            context.Result = statusCode switch
            {
                404 => new NotFoundObjectResult(json),
                _ => new BadRequestObjectResult(json)
            };

            context.HttpContext.Response.StatusCode = statusCode;
            context.ExceptionHandled = true;

        }
    }
}
