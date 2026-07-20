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
                    x.Status.Equals(
                        "Pendiente",
                        StringComparison.OrdinalIgnoreCase)),

                InReviewTickets = tickets.Count(x =>
                    x.Status.Equals(
                        "En Revisión",
                        StringComparison.OrdinalIgnoreCase)),

                InProgressTickets = tickets.Count(x =>
                    x.Status.Equals(
                        "En Progreso",
                        StringComparison.OrdinalIgnoreCase)),

                CancelledTickets = tickets.Count(x =>
                    x.Status.Equals(
                        "Cancelado",
                        StringComparison.OrdinalIgnoreCase)),

                CompletedTickets = tickets.Count(x =>
                    x.Status.Equals(
                        "Finalizado",
                        StringComparison.OrdinalIgnoreCase))
            };

            return dashboard;
        }
    }
}