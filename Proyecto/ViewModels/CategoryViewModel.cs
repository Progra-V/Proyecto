namespace Proyecto.Models.ViewModels
{
    public class CategoryViewModel
    {
        public long Id { get; set; }

        public long DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}