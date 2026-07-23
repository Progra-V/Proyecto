using Newtonsoft.Json;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    [Table("tickets")]
    public class Ticket : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("ticket_code")]
        public string TicketCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título es obligatorio")]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "La justificación es obligatoria")]
        [Column("justification")]
        public string Justification { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Column("category_id")]
        public long? CategoryId { get; set; }

        [JsonIgnore]
        public string? CategoryName { get; set; }

        [Column("risk")]
        public string? Risk { get; set; }

        [Column("status")]
        public string Status { get; set; } = "Pendiente";

        [Column("priority")]
        public string Priority { get; set; } = "Media";

        [Required(ErrorMessage = "El departamento es obligatorio")]
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