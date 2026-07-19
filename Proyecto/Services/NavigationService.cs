using Proyecto.Models;

namespace Proyecto.Services
{
    public static class NavigationService
    {
        public static List<NavigationPages> GetMenuPages(int userRole)
        {
            // Employee
            if (userRole == 3)
            {
                return new List<NavigationPages>
                {
                    // PRINCIPAL

                    new NavigationPages
                    {
                        Section = "Principal",
                        Title = "Dashboard",
                        Controller = "Dashboard",
                        Action = "Index",
                        Icon = "bi-speedometer2"
                    },

                    new NavigationPages
                    {
                        Section = "Principal",
                        Title = "Tickets",
                        Controller = "Ticket",
                        Action = "Index",
                        Icon = "bi-ticket-perforated"
                    },

                    // CATÁLOGOS ( sin acceso a los catalogos )

              

                    // CUENTA

                    new NavigationPages
                    {
                        Section = "Cuenta",
                        Title = "Mi Perfil",
                        Controller = "Profile",
                        Action = "Index",
                        Icon = "bi-person-circle"
                    },

                    new NavigationPages
                    {
                        Section = "Cuenta",
                        Title = "Cerrar sesión",
                        Controller = "Login",
                        Action = "Logout",
                        Icon = "bi-box-arrow-right"
                    }
                };
            }

            // Support Technician
            if (userRole == 2 )
            {
                return new List<NavigationPages>
                {
                    // PRINCIPAL

                    new NavigationPages
                    {
                        Section = "Principal",
                        Title = "Dashboard",
                        Controller = "Dashboard",
                        Action = "Index",
                        Icon = "bi-speedometer2"
                    },

                    new NavigationPages
                    {
                        Section = "Principal",
                        Title = "Tickets",
                        Controller = "Ticket",
                        Action = "Index",
                        Icon = "bi-ticket-perforated"
                    },

                    // CATÁLOGOS

                    new NavigationPages
                    {
                        Section = "Catálogos",
                        Title = "Categorías",
                        Controller = "Category",
                        Action = "Index",
                        Icon = "bi-tags"
                    },

                    new NavigationPages
                    {
                        Section = "Catálogos",
                        Title = "Departamentos",
                        Controller = "Department",
                        Action = "Index",
                        Icon = "bi-building"
                    },


                    // CUENTA

                    new NavigationPages
                    {
                        Section = "Cuenta",
                        Title = "Mi Perfil",
                        Controller = "Profile",
                        Action = "Index",
                        Icon = "bi-person-circle"
                    },

                    new NavigationPages
                    {
                        Section = "Cuenta",
                        Title = "Cerrar sesión",
                        Controller = "Login",
                        Action = "Logout",
                        Icon = "bi-box-arrow-right"
                    }
                };
            }
            // Administrator
            if (userRole == 1)
            {
                return new List<NavigationPages>
                {
                    // PRINCIPAL

                    new NavigationPages
                    {
                        Section = "Principal",
                        Title = "Dashboard",
                        Controller = "Dashboard",
                        Action = "Index",
                        Icon = "bi-speedometer2"
                    },

                    new NavigationPages
                    {
                        Section = "Principal",
                        Title = "Tickets",
                        Controller = "Ticket",
                        Action = "Index",
                        Icon = "bi-ticket-perforated"
                    },

                    // CATÁLOGOS

                    new NavigationPages
                    {
                        Section = "Catálogos",
                        Title = "Categorías",
                        Controller = "Category",
                        Action = "Index",
                        Icon = "bi-tags"
                    },

                    new NavigationPages
                    {
                        Section = "Catálogos",
                        Title = "Departamentos",
                        Controller = "Department",
                        Action = "Index",
                        Icon = "bi-building"
                    },


                    // ADMINISTRACIÓN

                    new NavigationPages
                    {
                        Section = "Administración",
                        Title = "Usuarios",
                        Controller = "User",
                        Action = "Index",
                        Icon = "bi-people"
                    },

                    // CUENTA

                    new NavigationPages
                    {
                        Section = "Cuenta",
                        Title = "Mi Perfil",
                        Controller = "Profile",
                        Action = "Index",
                        Icon = "bi-person-circle"
                    },

                    new NavigationPages
                    {
                        Section = "Cuenta",
                        Title = "Cerrar sesión",
                        Controller = "Login",
                        Action = "Logout",
                        Icon = "bi-box-arrow-right"
                    }
                };
            }



            return new List<NavigationPages>();
        }
    }
}