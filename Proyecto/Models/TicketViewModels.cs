using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Proyecto.Models
{
    public class TicketStatusUpdate : BaseModel
    {
        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    public class TicketViewModels
    {
        public Ticket Ticket { get; set; } = new();

        public List<Comment> Comments { get; set; } = new();

        public string? ActiveSessionUserId { get; set; }
    }
}