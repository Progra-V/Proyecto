using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Proyecto.Models
{
    [Table("tickets")]
    public class TicketStatusUpdate : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}