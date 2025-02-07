using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using Microsoft.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace DayCare.Service.Middlewares
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly ILogger<ResponseMiddleware> _logger;
        public ResponseMiddleware(RequestDelegate next, ILogger<ResponseMiddleware> logger)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                var originalBodyStream = context.Response.Body;
                using var responseBody = _recyclableMemoryStreamManager.GetStream();
                context.Response.Body = responseBody;
                Response response = null;
                try
                {
                    await _next(context);

                    dynamic actionResponse = default;
                    if (context.Response.Body.Length > 0)
                    {
                        context.Response.Body.Seek(0, SeekOrigin.Begin);
                        using var reader = new StreamReader(context.Response.Body);
                        actionResponse = await reader.ReadToEndAsync();
                        actionResponse = JsonConvert.DeserializeObject(actionResponse);
                    }
                    response = new Response
                    {
                        TimeStamp = DateTime.Now,
                        StatusCode = context.Response.StatusCode,
                        Errors = actionResponse != null && (context.Response.StatusCode < 200 || context.Response.StatusCode > 299)
                                                    ? actionResponse is JArray aj ? aj.Select(b => b.ToString()).ToList() : new List<string> { actionResponse?.ToString() }
                                                    : new List<string>(),

                        Data = actionResponse != null && context.Response.StatusCode >= 200 && context.Response.StatusCode <= 299
                                                    ? actionResponse
                                                    : null
                    };

                }
                catch (Exception ex)
                {
                    var statusCode = 500;
                    var errors = new List<string> { ex.Message };
                    context.Response.StatusCode = statusCode;
                    response = new Response { TimeStamp = DateTime.Now, StatusCode = statusCode, Data = null, Errors = errors };
                    _logger.LogError("StatusCode:{0} | Timestamp:{1} | Error:{2}", new object[] { statusCode,DateTime.Now, ex });
                }
                
                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)));
                SetHeaders(context, stream.Length.ToString());

                await stream.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                _logger.LogError("StatusCode:{0} | Timestamp:{1} | Error:{2}", new object[] { 500, DateTime.Now, ex });
                throw;
            }
        }
        private static void SetHeaders(HttpContext context, string contentLength)
        {
            context.Response.Headers.TryGetValue("Content-Type", out StringValues ct);
            if (ct.Any())
                context.Response.Headers.Remove("Content-Type");

            context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            context.Response.Headers.TryGetValue("Content-Length", out StringValues cl);
            if (cl.Any())
                context.Response.Headers.Remove("Content-Length");

            context.Response.Headers.Add("Content-Length", contentLength);
        }
        public class Response
        {
            public DateTime TimeStamp { get; set; }
            public int StatusCode { get; set; }
            public List<string> Errors { get; set; }
            public object Data { get; set; }
        }
    }
    public static class ResponseExtensions
    {
        public static IApplicationBuilder UseResponse(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseMiddleware>();
        }
    }
}
