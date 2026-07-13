using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Proyecto.Models
{
    [Table("tickets")]
    public class Ticket : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("status")]
        public string Status { get; set; } = "Pendiente";

        [Column("priority")]
        public string Priority { get; set; } = "Media";

        [Column("department_id")]
        public int? DepartmentId { get; set; }

        public string? DepartmentName { get; set; }

        [Column("ticket_type")]
        public string TicketType { get; set; } = "Task";

        [Column("created_by")]
        public string? CreatedBy { get; set; }

        [Column("assigned_to")]
        public string? AssignedTo { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }
    }
}