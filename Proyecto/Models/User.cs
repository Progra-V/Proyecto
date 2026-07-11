using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Proyecto.Models
{
    [Table("usuarios")]
    public class User : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("supabase_user_id")]
        public Guid SupabaseUserId { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("foto_url")]
        public string? FotoUrl { get; set; }

        [Column("rol")]
        public int Rol { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }
    }
}