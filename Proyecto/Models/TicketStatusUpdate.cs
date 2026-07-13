using Supabase.Postgrest.Attributes;

namespace Proyecto.Models
{
    public class TicketStatusUpdate
    {
        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}