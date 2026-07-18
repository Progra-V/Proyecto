using Proyecto.Models;
using Proyecto.SupabaseClient;

namespace Proyecto.Services

{
    public static class TicketService
    {

        public static readonly List<string> StatusCatalog =
[
            "Pending",
            "In Review",
            "In Progress",
            "Completed",
            "Cancelled"
];

        public static readonly List<string> PriorityCatalog =
        [
            "Low",
             "Medium",
            "High"
        ];

        public static readonly List<string> RiskCatalog =
        [
            "Low",
            "Medium",
            "High"
        ];

        private const int AdminRole = 1;
        private const int TechnicianRole = 2;
        private const int EmployeeRole = 3;
        public static async Task<List<Ticket>> GetAll()
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var tickets = (await client
                .From<Ticket>()
                .Get()).Models;

            var departments = (await client
                .From<Department>()
                .Get()).Models;

            var categories = (await client
                .From<Category>()
                .Get()).Models;

            var users = (await client
                .From<User>()
                .Get()).Models;

            foreach (var ticket in tickets)
            {
                // Departamento
                var department = departments
                    .FirstOrDefault(x => x.Id == ticket.DepartmentId);

                ticket.DepartmentName =
                    department?.Name ?? "Not found";

                // Categoría
                var category = categories
                    .FirstOrDefault(x => x.Id == ticket.CategoryId);

                ticket.CategoryName =
                    category?.Name ?? "Not found";

                // Usuario creador
                var creator = users
                    .FirstOrDefault(x => x.Id == ticket.CreatedBy);

                ticket.CreatedByName = creator != null
                    ? $"{creator.FirstName} {creator.LastName}"
                    : "Unknown";

                // Usuario asignado
                if (ticket.AssignedTo.HasValue)
                {
                    var assignedUser = users
                        .FirstOrDefault(x => x.Id == ticket.AssignedTo.Value);

                    ticket.AssignedToName = assignedUser != null
                        ? $"{assignedUser.FirstName} {assignedUser.LastName}"
                        : "Unassigned";
                }
                else
                {
                    ticket.AssignedToName = "Unassigned";
                }
            }

            return tickets;
        }

        public static async Task UpdateTicket(
            long ticketId,
            string status,
            string priority,
            string risk,
            long categoryId,
            int departmentId,
            int? assignedTo,
            User currentUser)
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var ticket = (await client
                .From<Ticket>()
                .Where(x => x.Id == ticketId)
                .Get()).Model;

            if (ticket == null)
                throw new InvalidOperationException("El ticket no existe.");

            ValidateStatus(status);
            ValidateStatusChange(ticket.Status, status);
            ValidatePriority(priority);
            ValidateRisk(risk);

            if (!await ValidateCategoryDepartment(categoryId, departmentId))
            {
                throw new InvalidOperationException(
                    "La categoría seleccionada no pertenece al departamento.");
            }

            ticket.Status = status;
            ticket.Priority = priority;
            ticket.Risk = risk;
            ticket.CategoryId = categoryId;
            ticket.DepartmentId = departmentId;
            ticket.AssignedTo = assignedTo;

            await ApplyAssignmentRules(ticket, currentUser);

            TicketUpdate update = new TicketUpdate
            {
                Id = ticket.Id,
                Status = ticket.Status,
                Priority = ticket.Priority,
                Risk = ticket.Risk,
                CategoryId = ticket.CategoryId,
                DepartmentId = ticket.DepartmentId,
                AssignedTo = ticket.AssignedTo,
                UpdatedAt = DateTime.UtcNow
            };

            await client
                .From<TicketUpdate>()
                .Update(update);
        }

        private static void ValidateStatus(string status)
        {
            if (!StatusCatalog.Contains(status))
                throw new InvalidOperationException("Estado inválido.");
        }

        private static void ValidatePriority(string priority)
        {
            if (!PriorityCatalog.Contains(priority))
                throw new InvalidOperationException("Prioridad inválida.");
        }

        private static void ValidateRisk(string risk)
        {
            if (!RiskCatalog.Contains(risk))
                throw new InvalidOperationException("Riesgo inválido.");
        }

        // Valida que la categoría pertenezca al departamento.
        private static async Task<bool> ValidateCategoryDepartment(
            long categoryId,
            int departmentId)
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var category = (await client
                .From<Category>()
                .Where(x => x.Id == categoryId)
                .Get()).Model;

            if (category == null)
                return false;

            return category.DepartmentId == departmentId;
        }



        public static async Task<Ticket?> GetByTicketId(long id)
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<Ticket>()
                .Where(x => x.Id == id)
                .Get();

            return result.Model;
        }

        public static async Task Create(
            Ticket ticket,
            User currentUser)
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            ValidatePriority(ticket.Priority);
            ValidateRisk(ticket.Risk);

            if (!await ValidateCategoryDepartment(
                ticket.CategoryId,
                ticket.DepartmentId))
            {
                throw new InvalidOperationException(
                    "La categoría seleccionada no pertenece al departamento.");
            }

            await ApplyAssignmentRules(ticket, currentUser);

            ticket.TicketCode =
                await DepartmentService.GenerateNextTicketCode(
                    ticket.DepartmentId);

            ticket.Status = "Pending";
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.UpdatedAt = DateTime.UtcNow;

            var insertTicket = new Ticket
            {
                TicketCode = ticket.TicketCode,
                Title = ticket.Title,
                Description = ticket.Description,
                Justification = ticket.Justification,
                CategoryId = ticket.CategoryId,
                DepartmentId = ticket.DepartmentId,
                Priority = ticket.Priority,
                Risk = ticket.Risk,
                Status = ticket.Status,
                CreatedBy = ticket.CreatedBy,
                AssignedTo = ticket.AssignedTo,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                DueDate = ticket.DueDate
            };

            await client
                .From<Ticket>()
                .Insert(insertTicket);
        }



        public static async Task UpdateStatus(
            long ticketId,
            string newStatus)
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var ticket = (await client
                .From<Ticket>()
                .Where(x => x.Id == ticketId)
                .Get()).Model;

            if (ticket == null)
                throw new InvalidOperationException("El ticket no existe.");

            ValidateStatus(newStatus);
            ValidateStatusChange(ticket.Status, newStatus);

            ticket.Status = newStatus;
            ticket.UpdatedAt = DateTime.UtcNow;

            await client
                .From<Ticket>()
                .Update(ticket);
        }
        private static void ValidateStatusChange(
    string currentStatus,
    string newStatus)
        {
            if (currentStatus == newStatus)
                return;

            if (currentStatus == "Pending")
            {
                if (newStatus == "In Progress" ||
                    newStatus == "Completed")
                {
                    throw new InvalidOperationException(
                        "No puede pasar de Pending a ese estado sin pasar primero por In Review.");
                }
            }

            if (currentStatus == "Completed" &&
                newStatus == "Cancelled")
            {
                throw new InvalidOperationException(
                    "Un ticket Completed no puede cambiarse a Cancelled.");
            }

            if (currentStatus == "Cancelled" &&
                newStatus == "Completed")
            {
                throw new InvalidOperationException(
                    "Un ticket Cancelled no puede cambiarse a Completed.");
            }
        }

        private static async Task ApplyAssignmentRules(
            Ticket ticket,
            User currentUser)
        {
            // Un empleado nunca puede asignar tickets.
            if (currentUser.RoleId == EmployeeRole)
            {
                ticket.AssignedTo = null;
                return;
            }

            // Si no se asignó a nadie, no hay nada que validar.
            if (!ticket.AssignedTo.HasValue)
                return;

            User? assignedUser =
                await UserService.GetById(ticket.AssignedTo.Value);

            if (assignedUser == null)
            {
                throw new InvalidOperationException(
                    "El usuario asignado no existe.");
            }

            // Un técnico solo puede asignar a técnicos.
            if (currentUser.RoleId == TechnicianRole)
            {
                if (assignedUser.RoleId != TechnicianRole)
                {
                    throw new InvalidOperationException(
                        "Solo puede asignar tickets a técnicos.");
                }

                return;
            }

            // Un administrador puede asignar a técnicos y administradores.
            if (currentUser.RoleId == AdminRole)
            {
                if (assignedUser.RoleId != TechnicianRole &&
                    assignedUser.RoleId != AdminRole)
                {
                    throw new InvalidOperationException(
                        "Solo puede asignar tickets a técnicos o administradores.");
                }
            }
        }
    }
}