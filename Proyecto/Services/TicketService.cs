using Proyecto.Models;
using Proyecto.SupabaseClient;

namespace Proyecto.Services
{
    public static class TicketService
    {
        public static async Task<List<Ticket>> GetAll()
        {
            Supabase.Client client = await SupabClient.GetSupabaseClientAsync();

            var tickets = (await client
                .From<Ticket>()
                .Get()).Models;

            var departments = (await client
                .From<Department>()
                .Get()).Models;

            var users = (await client
                .From<User>()
                .Get()).Models;

            foreach (var ticket in tickets)
            {
                var department = departments
                    .FirstOrDefault(x => x.Id == ticket.DepartmentId);

                ticket.DepartmentName =
                    department?.Name ?? "No encontrado";

                var creator = users
                    .FirstOrDefault(x => x.Id == ticket.CreatedBy);

                ticket.CreatedByName = creator != null
                    ? $"{creator.FirstName} {creator.LastName}"
                    : "Desconocido";

                if (ticket.AssignedTo.HasValue)
                {
                    var assignedUser = users
                        .FirstOrDefault(x => x.Id == ticket.AssignedTo.Value);

                    ticket.AssignedToName = assignedUser != null
                        ? $"{assignedUser.FirstName} {assignedUser.LastName}"
                        : "No asignado";
                }
                else
                {
                    ticket.AssignedToName = "No asignado";
                }
            }

            return tickets;
        }

        public static async Task UpdateTicket(
            long ticketId,
            string status,
            string priority,
            string risk,
            string category,
            int departmentId,
            int? assignedTo)
        {
            Supabase.Client client = await SupabClient.GetSupabaseClientAsync();

            TicketUpdate update = new TicketUpdate
            {
                Id = ticketId,
                Status = status,
                Priority = priority,
                Risk = risk,
                Category = category,
                DepartmentId = departmentId,
                AssignedTo = assignedTo,
                UpdatedAt = DateTime.UtcNow
            };

            await client
                .From<TicketUpdate>()
                .Update(update);
        }

        public static async Task<Ticket?> GetByTicketId(long id)
        {
            Supabase.Client client = await SupabClient.GetSupabaseClientAsync();

            var result = await client
                .From<Ticket>()
                .Where(x => x.Id == id)
                .Get();

            return result.Model;
        }

        public static async Task Create(Ticket ticket)
        {
            Supabase.Client client = await SupabClient.GetSupabaseClientAsync();

            await client
                .From<Ticket>()
                .Insert(ticket);
        }

        public static async Task UpdateStatus(long ticketId, string newStatus)
        {
            Supabase.Client client = await SupabClient.GetSupabaseClientAsync();

            TicketStatusUpdate update = new TicketStatusUpdate
            {
                Id = ticketId,
                Status = newStatus,
                UpdatedAt = DateTime.UtcNow
            };

            await client
                .From<TicketStatusUpdate>()
                .Where(x => x.Id == ticketId)
                .Update(update);
        }

        public static async Task<string> GenerateTicketCode(int departmentId)
        {
            Supabase.Client client = await SupabClient.GetSupabaseClientAsync();

            var department = (await client
                .From<Department>()
                .Where(x => x.Id == departmentId)
                .Get()).Model;

            if (department == null)
                throw new InvalidOperationException("Departamento no encontrado.");

            var ticketsInDepartment = (await client
                .From<Ticket>()
                .Where(x => x.DepartmentId == departmentId)
                .Get()).Models;

            int nextNumber = 1;

            if (ticketsInDepartment.Count > 0)
            {
                var numbers = ticketsInDepartment
                    .Select(t =>
                    {
                        var parts = t.TicketCode.Split('-');
                        return parts.Length == 2 && int.TryParse(parts[1], out int n) ? n : 0;
                    })
                    .ToList();

                nextNumber = numbers.Max() + 1;
            }

            return $"{department.Code}-{nextNumber:D4}";
        }

        public static async Task<int?> AssignTechnician()
        {
            Supabase.Client client = await SupabClient.GetSupabaseClientAsync();

            var technicians = (await client
                .From<User>()
                .Where(x => x.RoleId == 2)
                .Where(x => x.IsActive == true)
                .Get()).Models;

            if (technicians.Count == 0)
                return null;

            var activeTickets = (await client
                .From<Ticket>()
                .Get()).Models
                .Where(t => t.Status == "Pendiente" || t.Status == "En Revisión" || t.Status == "En Progreso")
                .ToList();

            var loads = technicians.Select(tech => new
            {
                Technician = tech,
                ActiveCount = activeTickets.Count(t => t.AssignedTo == tech.Id)
            }).ToList();

            int minCount = loads.Min(x => x.ActiveCount);
            var candidates = loads.Where(x => x.ActiveCount == minCount).ToList();

            var random = new Random();
            var chosen = candidates[random.Next(candidates.Count)];

            return chosen.Technician.Id;
        }

        public static async Task<Ticket> CreateTicket(Ticket ticket)
        {
            ticket.TicketCode = await GenerateTicketCode(ticket.DepartmentId);
            ticket.AssignedTo = await AssignTechnician();
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.UpdatedAt = DateTime.UtcNow;

            Supabase.Client client = await SupabClient.GetSupabaseClientAsync();

            var result = await client
                .From<Ticket>()
                .Insert(ticket);

            return result.Models.First();
        }
    }
}