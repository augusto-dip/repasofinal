namespace tl2_recupercionparcial2_2025_augusto_dip.Services
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated();
        bool HasAccessLevel(string rol);
        string GetLoggedUserName(); // ¡Agregamos esto!
    }
}