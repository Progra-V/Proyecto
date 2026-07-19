namespace Proyecto.Models
{
    public class NavigationPages
    {
        public string Title { get; set; } = string.Empty;

        public string Controller { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        // Sección del menú (Main, Catalogs, Administration)
        public string Section { get; set; } = string.Empty;

        // Ícono de Bootstrap Icons
        public string Icon { get; set; } = string.Empty;
    }
}