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
            var tickets = await TicketService.GetAll();

            DashboardViewModel dashboard = new DashboardViewModel
            {
                RecentTickets = tickets
        .OrderByDescending(x => x.CreatedAt)
        .Take(10)
        .ToList(),

                TotalTickets = tickets.Count,

                PendingTickets = tickets.Count(x =>
                    x.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase)),

                InReviewTickets = tickets.Count(x =>
                    x.Status.Equals("In Review", StringComparison.OrdinalIgnoreCase)),

                InProgressTickets = tickets.Count(x =>
                    x.Status.Equals("In Progress", StringComparison.OrdinalIgnoreCase)),

                CancelledTickets = tickets.Count(x =>
                    x.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase)),

                CompletedTickets = tickets.Count(x =>
                    x.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase))
            };

            return dashboard;
        }
    }
}