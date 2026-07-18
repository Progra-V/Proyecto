using Proyecto.ViewModels;

namespace Proyecto.Services
{
    public static class DashboardService
    {
        /// <summary>
        /// Obtiene toda la información necesaria para el Dashboard.
        /// </summary>
        public static async Task<DashboardViewModel> GetDashboardAsync()
        {
            var tickets = await TicketService.getAll();

            DashboardViewModel dashboard = new DashboardViewModel
            {
                RecentTickets = tickets
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(10)
                    .ToList(),

                TotalTickets = tickets.Count,

                PendingTickets = tickets.Count(x =>
                    x.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase) ||
                    x.Status.Equals("Open", StringComparison.OrdinalIgnoreCase)),

                InProgressTickets = tickets.Count(x =>
                    x.Status.Equals("In Progress", StringComparison.OrdinalIgnoreCase)),

                ClosedTickets = tickets.Count(x =>
                    x.Status.Equals("Closed", StringComparison.OrdinalIgnoreCase))
            };

            return dashboard;
        }
    }
}