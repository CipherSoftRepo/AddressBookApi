using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AddressBook.Services.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace AddressBook.Services.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {


                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            // this can be done much better, but since it's only for demonstration, ill leave it as is.
                            Message = "Internal Server Error. - " + contextFeature.Error.Message + "\n" + contextFeature.Error.InnerException
                        }.ToString());
                    }
                });
            });
        }
    }
}
