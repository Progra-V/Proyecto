using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Proyecto.Models
{
    [Table("comments")]
    public class Comment : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("ticket_id")]
        public long TicketId { get; set; }

        [Column("comment_text")]
        public string? CommentText { get; set; }

        [Column("created_by")]
        public string? CreatedBy { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}