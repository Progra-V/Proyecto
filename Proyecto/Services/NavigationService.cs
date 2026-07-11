using Proyecto.Models;

namespace Proyecto.Services
{
    public static class NavigationService
    {
        public static List<NavigationPages> GetMenuPages(int userRole)
        {
            if (userRole == 1)
            {
                return new List<NavigationPages>
        {
            new NavigationPages { Title="Tickets", Controller="Ticket", Action="Index"},
            new NavigationPages { Title="Usuarios", Controller="User", Action="Index"},
            new NavigationPages { Title="Logout", Controller="Login", Action="Logout"}
        };
            }

            if (userRole == 2 || userRole == 3)
            {
                return new List<NavigationPages>
        {
            new NavigationPages { Title="Tickets", Controller="Ticket", Action="Index"},
            new NavigationPages { Title="Logout", Controller="Login", Action="Logout"}
        };
            }

            return new List<NavigationPages>();
        }
    }
}