using Proyecto.Models;

namespace Proyecto.Services
{
    public static class NavigationService
    {
        public static List<NavigationPages> GetMenuPages(int userRole)
        {
            var menu = new List<NavigationPages>
            {
                // PRINCIPAL

                new NavigationPages
                {
                    Section = "Principal",
                    Title = "Inicio",
                    Controller = "Dashboard",
                    Action = "Index",
                    Icon = "bi-house-door"
                },

                new NavigationPages
                {
                    Section = "Principal",
                    Title = "Tickets",
                    Controller = "Ticket",
                    Action = "Index",
                    Icon = "bi-ticket-perforated"
                }
            };


            // TÉCNICO Y ADMINISTRADOR
            if (userRole == 2 || userRole == 3)
            {
                menu.Add(
                    new NavigationPages
                    {
                        Section = "Catálogos",
                        Title = "Categorías",
                        Controller = "Category",
                        Action = "Index",
                        Icon = "bi-tags"
                    }
                );

                menu.Add(
                    new NavigationPages
                    {
                        Section = "Catálogos",
                        Title = "Departamentos",
                        Controller = "Department",
                        Action = "Index",
                        Icon = "bi-building"
                    }
                );
            }


            // SOLO ADMINISTRADOR
            if (userRole == 3)
            {
                menu.Add(
                    new NavigationPages
                    {
                        Section = "Administración",
                        Title = "Usuarios",
                        Controller = "User",
                        Action = "Index",
                        Icon = "bi-people"
                    }
                );
            }


            // CUENTA

            menu.Add(
                new NavigationPages
                {
                    Section = "Cuenta",
                    Title = "Mi perfil",
                    Controller = "Profile",
                    Action = "Index",
                    Icon = "bi-person-circle"
                }
            );

            menu.Add(
                new NavigationPages
                {
                    Section = "Cuenta",
                    Title = "Cerrar sesión",
                    Controller = "Login",
                    Action = "Logout",
                    Icon = "bi-box-arrow-right",
                    RequiresConfirmation = true
                }
            );

            return menu;
        }
    }
}