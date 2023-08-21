using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SMSystem_Api.Model;
using System.ComponentModel;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace SMSystem_Api
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                var w32ex = ex as Win32Exception;
                if (w32ex == null)
                {
                    w32ex = ex.InnerException as Win32Exception;
                }
                else
                {
                    var statusCode = w32ex.ErrorCode;
                }
                var message = ex.Message;
            }
        }
    }
}
