using Serilog;
using System.Text.Json;

namespace Vk.Api.Middleware;

public class HeartBeatMiddleware
{
    private readonly RequestDelegate next;

    public HeartBeatMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context) // HttpContext includes: "Headers, body, parameters" in HttpRequest and HttpResponse
    {
        Log.Information("HeartBeat");

        if(context.Request.Path.StartsWithSegments("/hello"))
        {
            await context.Response.WriteAsync(JsonSerializer.Serialize("Hello from server!"));
            context.Response.StatusCode = 200;
            return;
        }

        await next.Invoke(context);
    }
}
