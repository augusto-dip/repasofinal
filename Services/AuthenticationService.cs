using Microsoft.AspNetCore.Http;

namespace tl2_recupercionparcial2_2025_augusto_dip.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _ctx;

        public AuthenticationService(IHttpContextAccessor ctx)
        {
            _ctx = ctx;
        }

        public bool IsAuthenticated() => !string.IsNullOrEmpty(_ctx.HttpContext.Session.GetString("User"));

        public bool HasAccessLevel(string rol) => _ctx.HttpContext.Session.GetString("Rol") == rol;
    }
}