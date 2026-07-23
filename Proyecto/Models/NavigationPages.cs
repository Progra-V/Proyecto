namespace Proyecto.Models
{
    public class NavigationPages
    {
        public string Title { get; set; } = string.Empty;

        public string Controller { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public string Section { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public bool RequiresConfirmation { get; set; }
    }
}