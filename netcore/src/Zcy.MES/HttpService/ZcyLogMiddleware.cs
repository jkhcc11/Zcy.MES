﻿using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;

namespace Zcy.MES.HttpService
{
    /// <summary>
    /// 日志中间件 扩展
    /// </summary>
    public static class ZcyLogMiddlewareExt
    {
        /// <summary>
        /// 使用日志中间件
        /// </summary>
        /// <returns></returns>
        public static void UseKdyLog(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ZcyLogMiddleware>();
        }
    }

    /// <summary>
    /// 日志中间件
    /// </summary>
    public class ZcyLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Stopwatch _stopwatch;
        //private readonly IHostingEnvironment _environment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ZcyLogMiddleware> _logger;

        public ZcyLogMiddleware(RequestDelegate next, ILogger<ZcyLogMiddleware> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _next = next;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _stopwatch = new Stopwatch();

        }

        public async Task Invoke(HttpContext context)
        {
            var currentFlag = context.TraceIdentifier;
            var request = context.Request;
            var reqPath = request.Path.Value;
            if (string.IsNullOrEmpty(reqPath))
            {
                return;
            }
            // var response = context.Response;

            var isSkipLog = false;
            var includeApiPrefix = new[] { "/api/" };

            if (includeApiPrefix.Any(reqPath.StartsWith) == false)
            {
                isSkipLog = true;
            }
            else
            {
                //跳过记录 通过EndPoint获取Controller特性
                var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
                var anonymous = endpoint?.Metadata.GetMetadata<DisableZcyLogAttribute>();
                if (anonymous != null)
                {
                    isSkipLog = true;
                }
            }

            if (isSkipLog)
            {
                //跳过的只记录异常
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Host:{host},Path:{path},Ref:{ref},Ua:{ua}",
                        request.Host,
                        reqPath,
                        request.Headers.Referer,
                        request.Headers.UserAgent);

                    context.Response.Clear();
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "text/json;charset=utf-8;";
                    await context.Response.WriteAsJsonAsync(KdyResult.Error(KdyResultCode.SystemError, "系统异常"));
                }

                return;
            }

            _stopwatch.Restart();
            var data = new ConcurrentDictionary<string, object>();
            data.TryAdd("request.url", request.Path.ToString());
            data.TryAdd("request.method", request.Method);
            data.TryAdd("request.executeStartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            if (request.Headers.TryGetValue("Authorization", out var token))
            {
                _logger.LogInformation("Flag:{flag},请求Token:{token}",
                    currentFlag,
                    token);
                //data.TryAdd("request.token", request.Headers["Authorization"]);
            }

            if (request.Method.ToLower() == "get")
            {
                data.TryAdd("request.body", request.QueryString.Value ?? "none");
            }
            else
            {
                request.EnableBuffering();
                var sr = new StreamReader(request.Body);
                var bodyStr = await sr.ReadToEndAsync();
                data.TryAdd("request.body", bodyStr);

                //重置
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            // 获取Response.Body内容
            var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                //执行其他
                await _next(context);

                //重置
                responseBody.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(responseBody);
                var str = await reader.ReadToEndAsync();

                //todo:如果这里有请求慢的时候 两个请求会在一起 导致add字典异常
                data.TryAdd("response.body", str);
                data.TryAdd("response.executeEndTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);

                _stopwatch.Stop();
                data.TryAdd("time", _stopwatch.ElapsedMilliseconds + "ms");
                if (request.Method.ToLower() != "get")
                {
                    _logger.LogInformation("用户请求{url}结束.Flag:{flag},时间：{time}ms",
                        request.Path.Value,
                        currentFlag,
                        _stopwatch.ElapsedMilliseconds);
                    _logger.LogInformation("Flag:{flag},扩展:{exData}",
                        currentFlag,
                        data.ToJsonStrExt());
                }
                else
                {
                    //记录日志
                    _logger.LogTrace("用户请求{url}结束.Flag:{flag},时间：{time}ms",
                        request.Path.Value,
                        currentFlag,
                        _stopwatch.ElapsedMilliseconds);
                    _logger.LogTrace("Flag:{flag},扩展:{exData}",
                        currentFlag,
                        data.ToJsonStrExt());
                }
            }
            catch (Exception ex) when (ex is ZcyCustomException customException)
            {
                var errResult = KdyResult.Error(KdyResultCode.Error, customException.Message);
                var bytes = Encoding.UTF8.GetBytes(errResult.ToJsonStrExt());
                var newStream = new MemoryStream(bytes);

                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "text/json;charset=utf-8;";
                await newStream.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                var errResult = KdyResult.Error(KdyResultCode.SystemError, "系统错误，请稍后再试");
                if (_webHostEnvironment.IsDevelopment())
                {
                    errResult.Msg = ex.Message;
                }

                _logger.LogError(ex, "系统异常：{ex.Message}.Flag:{flag}", ex.Message, currentFlag);
                var bytes = Encoding.UTF8.GetBytes(errResult.ToJsonStrExt());
                var newStream = new MemoryStream(bytes);

                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "text/json;charset=utf-8;";
                await newStream.CopyToAsync(originalBodyStream);
            }
        }
    }
}
