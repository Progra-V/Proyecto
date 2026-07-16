using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace Proyecto.Models
{
    [Table("tickets")]
    public class Ticket : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("ticket_code")]
        public string TicketCode { get; set; } = string.Empty;

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("justification")]
        public string? Justification { get; set; }

        [Column("category")]
        public string? Category { get; set; }

        [Column("risk")]
        public string? Risk { get; set; }

        [Column("status")]
        public string Status { get; set; } = "Pending";

        [Column("priority")]
        public string Priority { get; set; } = "Medium";

        [Column("department_id")]
        public int DepartmentId { get; set; }

        [JsonIgnore]
        public string? DepartmentName { get; set; }

        [JsonIgnore]
        public string? CreatedByName { get; set; }

        [JsonIgnore]
        public string? AssignedToName { get; set; }


        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("assigned_to")]
        public int? AssignedTo { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }
    }
}