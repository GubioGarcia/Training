using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using Template.CrossCutting.ExceptionHandler.Extensions;
using Training.ExceptionHandler.ViewModels;

namespace Training.ExceptionHandler.Providers
{
    public static class ExceptionHandlerMiddleware
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app, bool includeStackTrace)
        {
            return app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (exceptionHandlerFeature == null) return;

                    var error = exceptionHandlerFeature.Error;
                    var response = context.Response;

                    response.ContentType = "application/json";
                    ExceptionViewModel errorViewModel;

                    if (error is ApiException apiEx)
                    {
                        response.StatusCode = (int)apiEx.StatusCode;
                        errorViewModel = new ExceptionViewModel
                        {
                            StatusCode = apiEx.StatusCode,
                            Message = apiEx.Message,
                            Details = apiEx.Details,
                            StackTrace = includeStackTrace ? apiEx.StackTraceInfo : null
                        };
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorViewModel = new ExceptionViewModel
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            Message = "An unexpected error occurred. Please try again later.",
                            Details = error.Message,
                            StackTrace = includeStackTrace ? error.StackTrace : null
                        };
                    }

                    await response.WriteAsync(JsonConvert.SerializeObject(errorViewModel));
                }
            });
        }
    }
}
