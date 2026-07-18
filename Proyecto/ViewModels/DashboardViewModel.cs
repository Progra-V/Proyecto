using Proyecto.Models;

namespace Proyecto.ViewModels
{
    public class DashboardViewModel
    {
        // Lista de tickets recientes
        public List<Ticket> RecentTickets { get; set; } = new();

        // Indicadores del Dashboard
        public int TotalTickets { get; set; }

        public int PendingTickets { get; set; }

        public int InProgressTickets { get; set; }

        public int ClosedTickets { get; set; }
    }
}