using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Proyecto.Models
{
    public class TicketViewModels
    {
        public Ticket Ticket { get; set; } = new();

        public List<Comment> Comments { get; set; } = new();

        public int? ActiveSessionUserId { get; set; }

        public string? DepartmentName { get; set; }

        public List<Department> Departments { get; set; } = new();

        public List<User> Technicians { get; set; } = new();
    }
}