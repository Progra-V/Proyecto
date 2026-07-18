using Proyecto.Models;

namespace Proyecto.Services
{
    public static class NavigationService
    {
        public static List<NavigationPages> GetMenuPages(int userRole)
        {
            // Employee
            if (userRole == 1)
            {
                return new List<NavigationPages>
                {
                    new NavigationPages
                    {
                        Section = "Main",
                        Title = "Dashboard",
                        Controller = "Dashboard",
                        Action = "Index",
                        Icon = "bi-speedometer2"
                    },

                    new NavigationPages
                    {
                        Section = "Main",
                        Title = "Tickets",
                        Controller = "Ticket",
                        Action = "Index",
                        Icon = "bi-ticket-perforated"
                    },

                    new NavigationPages
                    {
                        Section = "Account",
                        Title = "My Profile",
                        Controller = "Profile",
                        Action = "Index",
                        Icon = "bi-person-circle"
                    },

                    new NavigationPages
                    {
                        Section = "Account",
                        Title = "Logout",
                        Controller = "Login",
                        Action = "Logout",
                        Icon = "bi-box-arrow-right"
                    }
                };
            }

            // Support Technician
            if (userRole == 2)
            {
                return new List<NavigationPages>
                {
                    // MAIN

                    new NavigationPages
                    {
                        Section = "Main",
                        Title = "Dashboard",
                        Controller = "Dashboard",
                        Action = "Index",
                        Icon = "bi-speedometer2"
                    },

                    new NavigationPages
                    {
                        Section = "Main",
                        Title = "Tickets",
                        Controller = "Ticket",
                        Action = "Index",
                        Icon = "bi-ticket-perforated"
                    },

                    // CATALOGS

                    new NavigationPages
                    {
                        Section = "Catalogs",
                        Title = "Categories",
                        Controller = "Category",
                        Action = "Index",
                        Icon = "bi-tags"
                    },

                    new NavigationPages
                    {
                        Section = "Catalogs",
                        Title = "Systems",
                        Controller = "System",
                        Action = "Index",
                        Icon = "bi-pc-display"
                    },

                    new NavigationPages
                    {
                        Section = "Catalogs",
                        Title = "Priorities",
                        Controller = "Priority",
                        Action = "Index",
                        Icon = "bi-flag"
                    },

                    new NavigationPages
                    {
                        Section = "Catalogs",
                        Title = "Risks",
                        Controller = "Risk",
                        Action = "Index",
                        Icon = "bi-exclamation-triangle"
                    },

                    new NavigationPages
                    {
                        Section = "Catalogs",
                        Title = "Status",
                        Controller = "Status",
                        Action = "Index",
                        Icon = "bi-list-check"
                    },

                    // ADMINISTRATION

                    new NavigationPages
                    {
                        Section = "Administration",
                        Title = "Users",
                        Controller = "User",
                        Action = "Index",
                        Icon = "bi-people"
                    },

                    // ACCOUNT

                    new NavigationPages
                    {
                        Section = "Account",
                        Title = "My Profile",
                        Controller = "Profile",
                        Action = "Index",
                        Icon = "bi-person-circle"
                    },

                    new NavigationPages
                    {
                        Section = "Account",
                        Title = "Logout",
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