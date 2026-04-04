using Microsoft.AspNetCore.Http;

namespace tl2_recupercionparcial2_2025_augusto_dip.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthenticationService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public bool IsAuthenticated()
        {
            return _contextAccessor.HttpContext?.Session.GetString("User") != null;
        }

        public bool HasAccessLevel(string rol)
        {
            return _contextAccessor.HttpContext?.Session.GetString("Rol") == rol;
        }

        public string GetLoggedUserName()
        {
            // Devolvemos el nombre directo desde el servicio, evitando errores en HTML
            return _contextAccessor.HttpContext?.Session.GetString("User") ?? "";
        }
    }
}