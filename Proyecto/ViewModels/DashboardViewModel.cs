using Proyecto.Models;

namespace Proyecto.ViewModels
{
    public class DashboardViewModel
    {
        public List<Ticket> RecentTickets { get; set; } = new();

        public int TotalTickets { get; set; }

        public int PendingTickets { get; set; }

        public int InReviewTickets { get; set; }

        public int InProgressTickets { get; set; }

        public int CancelledTickets { get; set; }

        public int CompletedTickets { get; set; }
    }
}