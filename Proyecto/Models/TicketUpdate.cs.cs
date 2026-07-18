using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Proyecto.Models
{
    [Table("tickets")]
    public class TicketUpdate : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Column("priority")]
        public string Priority { get; set; } = string.Empty;

        [Column("risk")]
        public string Risk { get; set; } = string.Empty;

        [Column("category")]
        public string Category { get; set; } = string.Empty;

        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Column("assigned_to")]
        public int? AssignedTo { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}