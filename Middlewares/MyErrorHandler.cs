namespace POS_System_DotNet.Middlewares
{
    public class MyErrorHandler
    {
        private readonly RequestDelegate _next;
        public MyErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = "Error occured",
                    Reason = e.Message
                });
                return;
            }

            if (context.Response.StatusCode == 405 ||
                context.Response.StatusCode == 404)
            {
                var json = new
                {
                    Message = "Method not supported or Not found",
                    HttpMethod = context.Request.Method,
                    RequestedPath = context.Request.Path
                };
                await context.Response.WriteAsJsonAsync(json);
            }

            if (context.Response.StatusCode == 401)
            {
                var json = new
                {
                    Message = "Please submit valid jwt",
                    HttpMethod = context.Request.Method,
                    RequestedPath = context.Request.Path
                };
                await context.Response.WriteAsJsonAsync(json);
            }
        }
    }
}
