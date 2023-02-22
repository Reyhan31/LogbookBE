using Kalbe.Library.Common.EntityFramework.Auth;
using System.Security.Claims;

namespace LogBookAPI.Authentication
{
    public class LogbookJwtMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public LogbookJwtMiddleware(RequestDelegate next, IConfiguration configuration){
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Headers["AppAuthorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                AttachUserToContext(context, token);
            }

            await _next(context);
        }

        protected virtual void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                if (Utils.IsTokenValid(token, _configuration.GetSection("LogbookJwtSecret").Value, out var securityToken))
                {
                    context.User = new ClaimsPrincipal(new ClaimsIdentity(securityToken.Claims));
                }
            }
            catch
            {
            }
        }
    }
}