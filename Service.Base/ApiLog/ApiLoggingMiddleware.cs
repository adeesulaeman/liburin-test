using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using Service.Base.ApiLog.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Service.Base.ApiLog
{
    public class ApiLoggingMiddleware
    {
        private readonly ILogger logger = Log.ForContext<ApiLoggingMiddleware>();
        private readonly RequestDelegate _next;

        public ApiLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var request = httpContext.Request;
                var stopWatch = Stopwatch.StartNew();
                var requestTime = DateTime.UtcNow;
                var requestBodyContent = await ReadRequestBody(request);
                var originalBodyStream = httpContext.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    var response = httpContext.Response;
                    response.Body = responseBody;
                    await _next(httpContext);
                    stopWatch.Stop();

                    string responseBodyContent = null;
                    responseBodyContent = await ReadResponseBody(response);
                    await responseBody.CopyToAsync(originalBodyStream);

                    VIPLog(requestTime,
                        stopWatch.ElapsedMilliseconds,
                        response.StatusCode,
                        request.Method,
                        request.Path,
                        request.QueryString.ToString(),
                        requestBodyContent,
                        responseBodyContent);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected Error : {ex}");
                await _next(httpContext);
            }
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private void VIPLog(DateTime requestTime,
                            long responseMillis,
                            int statusCode,
                            string method,
                            string path,
                            string queryString,
                            string requestBody,
                            string responseBody)
        {

            if (path.ToLower().Contains("token"))
            {
                requestBody = "(Request logging disabled for token)";
                responseBody = "(Response logging disabled for token)";
            }

            if (path.ToLower().Contains("upload") || requestBody.Contains("FileBytes"))
                requestBody = "(Request logging disabled for upload file)";


            if (requestBody != null && requestBody.ToLower().Contains("password"))
            {
                dynamic jsonObj = JsonConvert.DeserializeObject(requestBody.ToLower());
                if (jsonObj["password"] != null)
                    jsonObj["password"] = "*****";
                if (jsonObj["newpassword"] != null)
                    jsonObj["newpassword"] = "*****";
                if (jsonObj["ownerpassword"] != null)
                    jsonObj["ownerpassword"] = "*****";
                if (jsonObj["ownerconfirmpassword"] != null)
                    jsonObj["ownerconfirmpassword"] = "*****";
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                requestBody = output;
            }



            if (!IsSuccessStatusCode(statusCode))
            {
                var apiLog = new ApiLogItem
                {
                    RequestTime = requestTime,
                    ResponseMillis = responseMillis,
                    StatusCode = statusCode,
                    Method = method,
                    Path = path,
                    QueryString = queryString,
                    RequestBody = requestBody,
                    ResponseBody = responseBody
                };

                try
                {
                    logger.Error(JsonConvert.SerializeObject(apiLog));
                }
                catch (Exception ex)

                {
                    logger.Error($"Exception on writing log: {ex.Message}");
                }
            }
        }
        private bool IsSuccessStatusCode(int statusCode)
        {
            return (statusCode >= 200) && (statusCode <= 299);
        }
    }
}
