
using Proyecto.Models.ViewModels;
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

        public List<CategoryViewModel> Categories { get; set; } = new();

        public List<User> AssignableUsers { get; set; } = new();

        public User CurrentUser { get; set; } = new();


    }
}