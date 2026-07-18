using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Proyecto.Models
{
    [Table("categorias")]
    public class Category : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("departamento_id")]
        public long DepartmentId { get; set; }

        [Column("nombre")]
        public string Name { get; set; } = string.Empty;

        [Column("descripcion")]
        public string? Description { get; set; }

        [Column("activo")]
        public bool IsActive { get; set; }

        [Column("fecha_creacion")]
        public DateTime CreatedAt { get; set; }

        [Column("fecha_actualizacion")]
        public DateTime? UpdatedAt { get; set; }
    }
}